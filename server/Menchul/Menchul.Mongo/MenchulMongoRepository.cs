using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Menchul.Core.Entities;
using Menchul.Core.ResourceProcessing;
using Menchul.Core.ResourceProcessing.Behaviors;
using Menchul.Core.Services;
using Menchul.Mongo.Common;
using Menchul.Mongo.Documents;
using Menchul.Mongo.Exceptions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Menchul.Mongo
{
    /// <summary>
    /// Advanced mongo repository.
    /// The Convey.MongoRepository can't be extended as it is internal, so decoration is used instead.
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    /// <typeparam name="TIdentifiable"></typeparam>
    public class MenchulMongoRepository<TDocument, TIdentifiable> : IMenchulMongoRepository<TDocument, TIdentifiable>
        where TDocument : IDocumentRoot<TIdentifiable>
    {
        private const int __defaultMaxAddAttempts = 10;
        private const int __mongoDuplicateIdErrorCOde = 11000;

        private readonly IMongoRepository<TDocument, TIdentifiable> _repository;
        private readonly MongoCollections _collections;
        private readonly List<IRelationTracker> _relationTrackers;

        public MenchulMongoRepository(IMongoRepository<TDocument, TIdentifiable> repository,
            MongoCollections collections, List<IRelationTracker> relationTrackers)
        {
            _repository = repository;
            _collections = collections;
            _relationTrackers = relationTrackers;
        }

        #region Decorated members

        public Task<TDocument> GetAsync(TIdentifiable id)
        {
            return _repository.GetAsync(id);
        }

        public Task<TDocument> GetAsync(Expression<Func<TDocument, bool>> predicate)
        {
            return _repository.GetAsync(predicate);
        }

        public Task<IReadOnlyList<TDocument>> FindAsync(Expression<Func<TDocument, bool>> predicate)
        {
            return _repository.FindAsync(predicate);
        }

        public Task<PagedResult<TDocument>> BrowseAsync<TQuery>(Expression<Func<TDocument, bool>> predicate, TQuery query)
            where TQuery : IPagedQuery
        {
            return _repository.BrowseAsync(predicate, query);
        }

        public Task AddAsync(TDocument entity)
        {
            return _repository.AddAsync(entity);
        }

        public Task UpdateAsync(TDocument entity)
        {
            return _repository.UpdateAsync(entity);
        }

        public Task UpdateAsync(TDocument entity, Expression<Func<TDocument, bool>> predicate)
        {
            return _repository.UpdateAsync(entity, predicate);
        }

        public Task DeleteAsync(TIdentifiable id)
        {
            return _repository.DeleteAsync(id);
        }

        public Task DeleteAsync(Expression<Func<TDocument, bool>> predicate)
        {
            return _repository.DeleteAsync(predicate);
        }

        public Task<bool> ExistsAsync(Expression<Func<TDocument, bool>> predicate)
        {
            return _repository.ExistsAsync(predicate);
        }

        public IMongoCollection<TDocument> Collection => _repository.Collection;

        #endregion

        public async Task<bool> TryAddAsync<TIdMutableDocument, TAggregate, TAggregateValue>(TIdMutableDocument entity,
            TAggregate aggregate, IAggregateIdMutator idMutator, int maxAttempts = __defaultMaxAddAttempts)
            where TAggregate : AggregateId<TAggregateValue>
            where TIdMutableDocument : TDocument, IIdMutable<TAggregate, TAggregateValue>
        {
            bool isDuplicateKey;
            int attemptsMade = 0;

            do
            {
                try
                {
                    await _repository.AddAsync(entity);
                    isDuplicateKey = false;
                }
                catch (MongoWriteException e) when(e.WriteError.Code == __mongoDuplicateIdErrorCOde)
                {
                    idMutator.GenerateId(aggregate, force: true);
                    entity.SetId(aggregate);

                    isDuplicateKey = true;
                }

                attemptsMade++;
            } while (isDuplicateKey && attemptsMade < maxAttempts);

            return !isDuplicateKey;
        }

        public async Task TrackAsync(TDocument entity)
        {
            var entityTrackers = _relationTrackers.OfType<IRelationTracker<TDocument, TIdentifiable>>().ToList();
            if (!entityTrackers.Any())
            {
                return;
            }

            var refs = GetAllRefs(entity);
            if (!refs.Any())
            {
                return;
            }

            var bindTasks = entityTrackers
                .Select(t => t.BindAsync(entity.Id, refs, Collection.Database)).ToList();

            await Task.WhenAll(bindTasks);
        }

        public async Task DeleteTrackedAsync(TIdentifiable identifiable)
        {
            var referenceDocument = await Collection.Database
                .GetCollection<ReferenceDocument<TIdentifiable>>(_collections.ReferenceCollection<TDocument>())
                .Find(r => r.Id.Equals(identifiable)).SingleOrDefaultAsync();

            if (referenceDocument?.Refs.Any(p => p.Value.Any()) == true)
            {
                var idsByEntity = referenceDocument.Refs
                    .Where(p => p.Value.Any())
                    .ToDictionary(p => _collections.Entity(p.Key),
                        p => p.Value.Select(o => o.ToString()));

                throw new EntityReferencedException(idsByEntity);
            }

            if (referenceDocument?.Permanent == true)
            {
                throw new EntityReferencedException();
            }

            var entityTrackers = _relationTrackers.OfType<IRelationTracker<TDocument, TIdentifiable>>().ToList();

            if (entityTrackers.Any())
            {
                if (entityTrackers.Any(t => t.Permanent))
                {
                    throw new PermanentRelationDeleteException();
                }

                var entity = await _repository.GetAsync(identifiable);
                var refs = GetAllRefs(entity);

                if (!refs.Any())
                {
                    return;
                }

                var unbindTasks = entityTrackers
                    .Select(t => t.UnbindAsync(identifiable, refs, Collection.Database));

                await Task.WhenAll(unbindTasks);
            }

            await _repository.DeleteAsync(identifiable);

            if (referenceDocument != null)
            {
                await Collection.Database
                    .GetCollection<ReferenceDocument<TIdentifiable>>(_collections.ReferenceCollection<TDocument>())
                    .DeleteOneAsync(r => r.Id.Equals(identifiable));
            }
        }

        private static List<IDocumentRef> GetAllRefs(TDocument entity)
        {
            var behavior = new ResourceScannerBehavior<IDocumentRef>();
            var processor = new ResourceBehaviorProcessor {behavior};
            entity.Accept(processor);
            // TODO scan with refs?

            return behavior.GetResources().ToList();
        }
    }
}