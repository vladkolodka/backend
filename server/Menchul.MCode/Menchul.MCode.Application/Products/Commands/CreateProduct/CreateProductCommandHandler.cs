using Convey.CQRS.Commands;
using Menchul.Application;
using Menchul.Convey;
using Menchul.MCode.Application.Common;
using Menchul.MCode.Application.Products.Exceptions;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.Exceptions;
using Menchul.MCode.Core.Policies;
using Menchul.MCode.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : ICommandHandler<ICommandInfo<CreateProductCommand, string>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IClientCompanyRepository _clientCompanyRepository;
        private readonly Id12BytesCrypto _newId;
        private readonly IAggregateIdParser _idParser;
        private readonly IClientCompanyOwnerPolicy _ownerPolicy;
        private readonly ILogger<CreateProductCommandHandler> _logger;
        private readonly Guid _clientCompanyId;

        public CreateProductCommandHandler(IProductRepository productRepository, ICompanyRepository companyRepository,
            IClientCompanyRepository clientCompanyRepository, Id12BytesCrypto newId, IAppContext appContext,
            IAggregateIdParser idParser, IClientCompanyOwnerPolicy ownerPolicy,
            ILogger<CreateProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _companyRepository = companyRepository;
            _clientCompanyRepository = clientCompanyRepository;
            _newId = newId;
            _idParser = idParser;
            _ownerPolicy = ownerPolicy;
            _logger = logger;

            _clientCompanyId = appContext.Identity.ClientCompanyId;
        }

        private async Task<string> HandleAsync(CreateProductCommand command)
        {
            _logger.LogInformation("Trying to create a new product with EAN {Ean} for parent {ParentProductId}",
                command.EAN, command.ParentProductId);

            if (!await _clientCompanyRepository.ExistsAsync(_clientCompanyId))
            {
                throw new CompanyNotFoundException(_clientCompanyId);
            }

            if (await _productRepository.EANExistsAsync(command.EAN))
            {
                throw new EanAlreadyTakenException(command.EAN);
            }

            var companyIds = command.GetCompanyIds();

            if (await _companyRepository.ExistsAllAsync(companyIds) is var existingIds &&
                existingIds.Count != companyIds.Count)
            {
                throw new CompanyNotFoundException(companyIds, existingIds);
            }

            var product = new ProductInformation(_newId, command.Name, command.BrandName,
                command.BrandOwnerCompanyId, command.EAN, command.DistributorCompanyId,
                command.ManufacturerCompanyId, command.Codes,
                command.ManufacturerAddressOfManufacturing?.ToValueObject(),
                command.BrandOwnerAddressOfManufacturing?.ToValueObject(), command.DefaultLanguage, command.Description,
                command.Urls, command.Certificates, (command as IPropertiesContainer).GetProperties(),
                _clientCompanyId);

            var metadata = command.Metadata.ToValueObject();
            product.SetMetadata(metadata);

            if (!string.IsNullOrEmpty(command.ParentProductId))
            {
                var parentProductId = _idParser.Id12BytesFromString(command.ParentProductId);
                var parentProduct = await _productRepository.GetAsync(parentProductId);

                if (parentProduct == null)
                {
                    throw new ProductNotFoundException(command.ParentProductId);
                }

                if (!_ownerPolicy.IsOwnedBy(parentProduct, _clientCompanyId))
                {
                    throw new ProductNotOwnedException(parentProduct.Id);
                }

                product.SetParentProduct(parentProduct);
            }

            await _productRepository.AddAsync(product);

            _logger.LogInformation("Created product {ProductId} with EAN {Ean} for parent {ParentProductId}",
                product.Id.Value, product.EAN, product.ParentProduct?.Id.Value);

            return _newId;
        }

        public async Task HandleAsync(ICommandInfo<CreateProductCommand, string> info)
        {
            info.Result = await HandleAsync(info.Command);
        }
    }
}