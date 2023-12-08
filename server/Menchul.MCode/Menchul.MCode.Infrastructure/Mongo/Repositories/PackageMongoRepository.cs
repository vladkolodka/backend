using Menchul.Core.Services;
using Menchul.Core.Types;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.Enums;
using Menchul.MCode.Core.Exceptions;
using Menchul.MCode.Core.Repositories;
using Menchul.MCode.Infrastructure.Exceptions;
using Menchul.MCode.Infrastructure.Mongo.AggregateExtensions;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.Mongo;
using Menchul.Mongo.QueryRunners;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Mongo.Repositories
{
    internal class PackageMongoRepository : IPackageRepository
    {
        private readonly IMenchulMongoRepository<PackageDocument, ObjectId> _repository;
        private readonly IRelationManager _relationManager;
        private readonly IAggregateIdMutator _idMutator;
        private readonly ILogger<PackageMongoRepository> _logger;

        public PackageMongoRepository(IMenchulMongoRepository<PackageDocument, ObjectId> repository,
            IRelationManager relationManager, IAggregateIdMutator idMutator, ILogger<PackageMongoRepository> logger)
        {
            _repository = repository;
            _relationManager = relationManager;
            _idMutator = idMutator;
            _logger = logger;
        }

        public async Task AddAsync(Package package)
        {
            _logger.LogDebug("AddAsync {PackageId}", package.Id.Value);

            var document = new PackageDocument(package);

            var unitRefs = await _relationManager.ValidateReferences<ProductUnitDocument, ObjectId>(document);
            var packageRefs = await _relationManager.ValidateReferences<PackageDocument, ObjectId>(document);

            _logger.LogDebug("AddAsync {PackageId} - validated references", package.Id.Value);

            if (!unitRefs.IsValid() || !packageRefs.IsValid())
            {
                _logger.LogDebug("AddAsync {PackageId} - reference(s) is/are invalid", package.Id.Value);
                throw new PackageContentsInvalidException();
            }

            var isAdded = await _repository.TryAddAsync<PackageDocument, Id12Bytes, IBsonId>(document, package.Id,
                _idMutator);

            _logger.LogDebug("AddAsync {PackageId} - saved: {PackageSaved}", package.Id.Value, isAdded);

            if (!isAdded)
            {
                throw new EntityNotSavedException();
            }
        }

        public async Task<List<Package>> GetConflictingAsync(Package package)
        {
            _logger.LogDebug("GetConflictingAsync {PackageId}", package.Id.Value);

            var packageId = package.Id.ToObjectId();

            var packageIds = (package.Packages ?? new HashSet<Id12Bytes>()).Select(id => id.ToObjectId())
                .ToHashSet();
            var unitIds = (package.ProductUnits ?? new HashSet<Id12Bytes>()).Select(id => id.ToObjectId())
                .ToHashSet();

            var foundPackages = await _repository.Collection.Aggregate()
                .Match(Builders<PackageDocument>.Filter.And(new[]
                    {
                        Builders<PackageDocument>.Filter.Or(new[]
                        {
                            Builders<PackageDocument>.Filter.AnyIn(
                                $"{PackageDocument.ProductUnitsName}.{Constants.Properties.RefId}", unitIds),
                            Builders<PackageDocument>.Filter.AnyIn(
                                $"{PackageDocument.PackagesName}.{Constants.Properties.RefId}", packageIds)
                        }),
                        new ExpressionFilterDefinition<PackageDocument>(p => p.State == PackageState.Valid)
                    }
                ))
                .GraphLookup<PackageDocument, PackageDocument, ObjectId, ObjectId, ObjectId, List<PackageDocument>,
                    PackageDocumentWithHierarchy>(
                    from: _repository.Collection,
                    startWith: new ExpressionAggregateExpressionDefinition<PackageDocument, ObjectId>(d => d.Id,
                        new ExpressionTranslationOptions()),
                    connectFromField: new ExpressionFieldDefinition<PackageDocument, ObjectId>(d => d.Id),
                    connectToField: $"{PackageDocument.PackagesName}.{Constants.Properties.RefId}",
                    @as: new ExpressionFieldDefinition<PackageDocumentWithHierarchy, List<PackageDocument>>(h =>
                        h.Hierarchy),
                    options: new
                        AggregateGraphLookupOptions<PackageDocument, PackageDocument, PackageDocumentWithHierarchy>
                        {
                            RestrictSearchWithMatch =
                                new ExpressionFilterDefinition<PackageDocument>(p => p.State == PackageState.Valid)
                        }
                )
                .ToListAsync();

            _logger.LogDebug("GetConflictingAsync {PackageId} - loaded conflicts", package.Id.Value);

            return foundPackages
                .Concat(foundPackages.FlattenHierarchies())
                .Where(p => p.Id != packageId)
                .Distinct(PackageDocument.IdComparer)
                .Select(p => p.ToEntity())
                .ToList();
        }

        public async Task UpdateStateAllAsync(List<Package> packages)
        {
            var packageIds = packages.Select(p => p.Id.Value).ToList();

            _logger.LogDebug("UpdateStateAllAsync {PackageIds}", packageIds);

            if (!packages.Any())
            {
                return;
            }

            var updates = packages.GroupBy(p => p.State)
                .Select(g => new {State = g.Key, Ids = g.Select(p => p.Id.ToObjectId())})
                .Select(g => new UpdateManyModel<PackageDocument>(
                    new ExpressionFilterDefinition<PackageDocument>(d => g.Ids.Contains(d.Id)),
                    Builders<PackageDocument>.Update.Set(d => d.State, g.State)));

            await _repository.Collection.BulkWriteAsync(updates);

            _logger.LogDebug("UpdateStateAllAsync {PackageIds} - updated", packageIds);
        }
    }
}