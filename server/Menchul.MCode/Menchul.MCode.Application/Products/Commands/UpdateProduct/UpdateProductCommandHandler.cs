using Convey.CQRS.Commands;
using Menchul.Application;
using Menchul.MCode.Application.Common;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Core.Exceptions;
using Menchul.MCode.Core.Policies;
using Menchul.MCode.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _repository;
        private readonly IClientCompanyOwnerPolicy _ownerPolicy;
        private readonly IAggregateIdParser _idParser;
        private readonly ILogger<UpdateProductCommandHandler> _logger;
        private readonly Guid _clientCompanyId;

        public UpdateProductCommandHandler(IProductRepository repository, IClientCompanyOwnerPolicy ownerPolicy,
            IAggregateIdParser idParser, ILogger<UpdateProductCommandHandler> logger, IAppContext appContext)
        {
            _repository = repository;
            _ownerPolicy = ownerPolicy;
            _idParser = idParser;
            _logger = logger;

            _clientCompanyId = appContext.Identity.ClientCompanyId;
        }

        public async Task HandleAsync(UpdateProductCommand command)
        {
            _logger.LogInformation("Trying to update product {ProductId}", command.Id);
            _logger.LogDebug("Update product command: {@UpdateProductCommand}", command);

            var productId = _idParser.Id12BytesFromString(command.Id);
            var product = await _repository.GetInformationAsync(productId);

            if (product == null)
            {
                throw new ProductNotFoundException(command.Id);
            }

            if (!_ownerPolicy.IsOwnedBy(product, _clientCompanyId))
            {
                throw new ProductNotOwnedException(product.Id);
            }

            var metadata = command.Metadata.ToValueObject();

            product.Rename(command.Name);
            product.ChangeDescription(command.Description);
            product.ChangeDefaultLanguage(command.DefaultLanguage);
            product.ChangeManufacturingAddressForManufacturer(
                command.ManufacturerAddressOfManufacturing?.ToValueObject());
            product.ChangeManufacturingAddressForBrandOwner(command.BrandOwnerAddressOfManufacturing?.ToValueObject());
            product.ChangeBrandName(command.BrandName);
            product.ChangeBrandOwnerCompany(command.BrandOwnerCompanyId);
            product.ChangeDistributorCompany(command.DistributorCompanyId);
            product.ChangeManufacturerCompany(command.ManufacturerCompanyId);
            product.ReplaceCodes(command.Codes);
            product.ReplaceUrls(command.Urls);
            product.ReplaceCertificates(command.Certificates);
            product.ReplaceProperties((command as IPropertiesContainer).GetProperties());
            product.SetMetadata(metadata);

            await _repository.UpdateAsync(product);

            _logger.LogInformation("Updated product {ProductId}", product.Id.Value);
        }
    }
}