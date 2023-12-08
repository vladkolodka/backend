using Menchul.Core.Services;
using Menchul.Core.Types;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.Repositories;
using Menchul.MCode.Infrastructure.Common;
using Menchul.MCode.Infrastructure.Exceptions;
using Menchul.MCode.Infrastructure.Mongo.AggregateExtensions;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.MCode.Infrastructure.Services.Interfaces;
using Menchul.Mongo;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Mongo.Repositories
{
    internal class ProductMongoRepository : IProductRepository
    {
        private readonly IMenchulMongoRepository<ProductDocument, ObjectId> _repository;
        private readonly IRelationManager _relationManager;
        private readonly IAggregateIdMutator _idMutator;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger<ProductMongoRepository> _logger;

        public ProductMongoRepository(IMenchulMongoRepository<ProductDocument, ObjectId> repository,
            IRelationManager relationManager,
            IAggregateIdMutator idMutator, ILocalizationService localizationService,
            ILogger<ProductMongoRepository> logger)
        {
            _repository = repository;
            _relationManager = relationManager;
            _idMutator = idMutator;
            _localizationService = localizationService;
            _logger = logger;
        }

        public async Task AddAsync(ProductInformation product)
        {
            _logger.LogDebug("AddAsync {ProductId}", product.Id.Value);

            await _localizationService.ExtractSaveOrUpdate(product);

            var document = new ProductDocument(product);

            var isAdded = await _repository.TryAddAsync<ProductDocument, Id12Bytes, IBsonId>(document, product.Id,
                _idMutator);

            _logger.LogDebug("AddAsync {ProductId} - saved: {ProductSaved}", product.Id.Value, isAdded);

            if (!isAdded)
            {
                throw new EntityNotSavedException();
            }

            await _repository.TrackAsync(document);

            _logger.LogDebug("AddAsync {ProductId} - tracking enabled", product.Id.Value);
        }

        public async Task<Product> GetAsync(Id12Bytes id)
        {
            _logger.LogDebug("GetAsync {ProductId}", id.Value);

            var document = await _repository.GetAsync(id.ToObjectId());

            await _relationManager.LoadLocalization(document);

            var product = document?.ToEntity();

            _logger.LogDebug("GetAsync {ProductId} - loaded: {ProductLoaded}", id.Value, product != null);

            return product;
        }

        public async Task<ProductInformation> GetInformationAsync(Id12Bytes id)
        {
            _logger.LogDebug("GetInformationAsync {ProductId}", id.Value);

            var document = await _repository.GetAsync(id.ToObjectId());

            await _relationManager.LoadLocalization(document);

            _logger.LogDebug("GetInformationAsync {ProductId} - loaded localization", id.Value);

            var productInformation = document?.ToEntityInformation();

            _logger.LogDebug("GetInformationAsync {ProductId} - loaded: {ProductLoaded}",
                id.Value, productInformation != null);

            return productInformation;
        }

        public async Task<bool> EANExistsAsync(long ean)
        {
            _logger.LogDebug("GetInformationAsync {Ean}", ean);

            var exists = await _repository.ExistsAsync(p => p.EAN == ean);

            _logger.LogDebug("GetInformationAsync {Ean} - exists: {ProductExists}", ean, exists);

            return exists;
        }

        public async Task UpdateAsync(ProductInformation product)
        {
            _logger.LogDebug("UpdateAsync {ProductId}", product.Id.Value);

            await _localizationService.ExtractSaveOrUpdate(product);

            _logger.LogDebug("UpdateAsync {ProductId} - updated localizations", product.Id.Value);

            var document = new ProductDocument(product);
            await _repository.UpdateAsync(document);

            _logger.LogDebug("UpdateAsync {ProductId} - updated", product.Id.Value);
        }

        public Task DeleteAsync(Id12Bytes id) => _repository.DeleteTrackedAsync(id.ToObjectId());
    }
}