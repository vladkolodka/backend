using Menchul.Core.Services;
using Menchul.Core.Types;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.Repositories;
using Menchul.MCode.Infrastructure.Exceptions;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.Mongo;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Mongo.Repositories
{
    internal class ProductUnitMongoRepository : IProductUnitRepository
    {
        private readonly IMenchulMongoRepository<ProductUnitDocument, ObjectId> _repository;
        private readonly IAggregateIdMutator _idMutator;
        private readonly ILogger<ProductUnitMongoRepository> _logger;

        public ProductUnitMongoRepository(IMenchulMongoRepository<ProductUnitDocument, ObjectId> repository,
            IAggregateIdMutator idMutator, ILogger<ProductUnitMongoRepository> logger)
        {
            _repository = repository;
            _idMutator = idMutator;
            _logger = logger;
        }

        public async Task AddAsync(ProductUnit unit)
        {
            _logger.LogDebug("AddAsync {ProductUnitId}", unit.Id.Value);

            var document = new ProductUnitDocument(unit);

            var isAdded = await _repository.TryAddAsync<ProductUnitDocument, Id12Bytes, IBsonId>(document, unit.Id,
                _idMutator);

            _logger.LogDebug("AddAsync {ProductUnitId} - saved: {ProductUnitSaved}", unit.Id.Value, isAdded);

            if (!isAdded)
            {
                throw new EntityNotSavedException();
            }

            await _repository.TrackAsync(document);

            _logger.LogDebug("AddAsync {ProductUnitId} - tracking enabled", unit.Id.Value);
        }
    }
}