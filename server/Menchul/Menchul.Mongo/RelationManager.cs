using Convey.Persistence.MongoDB;
using Menchul.Core.ResourceProcessing;
using Menchul.Core.ResourceProcessing.Behaviors;
using Menchul.Mongo.Common;
using Menchul.Mongo.Exceptions;
using Menchul.Mongo.QueryRunners;
using Menchul.Mongo.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.Mongo
{
    // TODO add logs
    public class RelationManager : IRelationManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RelationManager> _logger;

        public RelationManager(IServiceProvider serviceProvider, ILogger<RelationManager> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<List<TDocument>> MapLoad<TDocument, TId>(IEnumerable<IDocumentResource> parentResources,
            QueryParametersContainer parametersContainer = null, QueryRunnerType? runnerKey = null)
            where TDocument : IDocumentRoot<TId>
        {
            var parentResourcesList = parentResources?.ToList();

            if (parentResourcesList == null || !parentResourcesList.Any())
            {
                _logger.LogDebug("Parent resource(s) are empty");
                return null;
            }

            return await MapLoad<TDocument, TId>(new DocumentResourceContainer(parentResourcesList),
                parametersContainer,
                runnerKey);
        }

        public async Task<List<TDocument>> MapLoad<TDocument, TId>(IDocumentResource parentResource,
            QueryParametersContainer parametersContainer = null, QueryRunnerType? runnerKey = null)
            where TDocument : IDocumentRoot<TId>
        {
            var parentDocumentName = parentResource.GetType().Name;

            _logger.LogDebug("Trying to load child documents for parent document {ParentDocumentName}",
                parentDocumentName);

            if (!TryExtractReferences(parentResource, out IReadOnlyCollection<DocumentRef<TDocument, TId>> references))
            {
                _logger.LogDebug(
                    "Child documents ({ChildDocumentName}) not found for parent document {ParentDocumentName}",
                    typeof(TDocument).Name, parentDocumentName);

                return null;
            }

            var childIds = references.Select(r => r.RefId).ToList();

            _logger.LogDebug(
                "Extracted child document ids of type {ChildDocumentName} for document type {ParentDocumentName}: {DocumentIds}",
                typeof(TDocument).Name, parentDocumentName, childIds);

            var filter = CreateIdInFilter<TDocument, TId>(childIds);

            var childDocuments = await Load<TDocument, TId>(f => f.Match(filter), parametersContainer, runnerKey);

            return Map(references, childDocuments);
        }

        public async Task<ReferenceMap<TId>> ValidateReferences<TDocument, TId>(
            IEnumerable<IDocumentResource> parentResources) where TDocument : IDocumentRoot<TId>
        {
            var parentResourcesList = parentResources?.ToList();

            if (parentResourcesList == null || !parentResourcesList.Any())
            {
                return null;
            }

            return await ValidateReferences<TDocument, TId>(new DocumentResourceContainer(parentResourcesList));
        }

        public async Task<ReferenceMap<TId>> ValidateReferences<TDocument, TId>(IDocumentResource parentResource)
            where TDocument : IDocumentRoot<TId>
        {
            if (!TryExtractReferences(parentResource, out IReadOnlyCollection<DocumentRef<TDocument, TId>> references))
            {
                return ReferenceMap<TId>.True;
            }

            var childIds = references.Select(r => r.RefId).Distinct().ToList();

            var filter = CreateIdInFilter<TDocument, TId>(childIds);

            using var serviceScope = _serviceProvider.CreateScope();

            var repository = serviceScope.ServiceProvider.GetService<IMongoRepository<TDocument, TId>>();

            if (repository == null)
            {
                throw new RepositoryNotResolvedException<TDocument>();
            }

            var foundIdRefs = await repository.Collection
                .Aggregate()
                .Match(filter)
                .Project(d => new {d.Id})
                .ToListAsync();

            var foundIds = foundIdRefs.Select(r => r.Id);

            var match = childIds.GroupJoin(foundIds, id => id, id => id,
                (id, existingIds) => new KeyValuePair<TId, bool>(id, existingIds.Any()));

            return new ReferenceMap<TId>(match);
        }

        public async Task<List<TDocument>> Load<TDocument, TId>(AggregateFluent<TDocument> queryModifier = null,
            QueryParametersContainer parametersContainer = null, QueryRunnerType? runnerKey = null)
            where TDocument : IDocumentRoot<TId>
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var queryResolver = serviceScope.ServiceProvider.GetService<QueryRunnerResolver<TDocument>>();

            if (queryResolver == null)
            {
                throw new QueryRunnerNotResolvedException<TDocument>(runnerKey);
            }

            var runner = queryResolver.Invoke(runnerKey);

            return await runner.Run(queryModifier, parametersContainer);
        }

        public List<TDocument> Map<TDocument, TId>(IEnumerable<IDocumentResource> parentResources,
            List<TDocument> children)
            where TDocument : IDocumentRoot<TId>
        {
            var parentResourcesList = parentResources?.ToList();

            if (parentResourcesList == null || !parentResourcesList.Any())
            {
                return null;
            }

            return Map<TDocument, TId>(new DocumentResourceContainer(parentResourcesList), children);
        }

        public List<TDocument> Map<TDocument, TId>(IDocumentResource parentResource, List<TDocument> children)
            where TDocument : IDocumentRoot<TId>
        {
            if (!TryExtractReferences(parentResource, out IReadOnlyCollection<DocumentRef<TDocument, TId>> references,
                true))
            {
                return null;
            }

            return Map(references, children);
        }

        private static bool TryExtractReferences<TDocument, TId>(IDocumentResource parentResource,
            out IReadOnlyCollection<DocumentRef<TDocument, TId>> references, bool all = false)
            where TDocument : IDocumentRoot<TId>
        {
            references = null;

            if (parentResource == null)
            {
                return false;
            }

            var scanner = new ResourceScannerBehavior<DocumentRef<TDocument, TId>>();
            parentResource.Accept(new ResourceBehaviorProcessor {scanner});

            // extract only not loaded references
            var resources = scanner.GetResources();

            references = all
                ? resources.ToList()
                : resources.Where(r => r.Document == null).ToList();

            return references.Any();
        }

        private List<TDocument> Map<TDocument, TId>(IReadOnlyCollection<DocumentRef<TDocument, TId>> references,
            List<TDocument> children) where TDocument : IDocumentRoot<TId>
        {
            var set = children.GroupBy(d => d.Id)
                .ToDictionary(d => d.Key, d => d.First());

            foreach (var reference in references)
            {
                if (!set.ContainsKey(reference.RefId))
                {
                    continue;
                }

                reference.Document = set[reference.RefId];
            }

            return children;
        }

        private FilterDefinition<TDocument> CreateIdInFilter<TDocument, TId>(
            IEnumerable<TId> ids) where TDocument : IDocumentRoot<TId> =>
            new FilterDefinitionBuilder<TDocument>().In(d => d.Id, ids);
    }
}