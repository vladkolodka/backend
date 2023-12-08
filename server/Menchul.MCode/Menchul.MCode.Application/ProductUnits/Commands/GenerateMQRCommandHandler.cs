using Convey.CQRS.Commands;
using Menchul.Application;
using Menchul.Convey;
using Menchul.MCode.Application.Common;
using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.Common.Enums;
using Menchul.MCode.Application.Common.Models;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.Exceptions;
using Menchul.MCode.Core.Policies;
using Menchul.MCode.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.ProductUnits.Commands
{
    public class GenerateMQRCommandHandler : ICommandHandler<ICommandInfo<GenerateMQRCommand, QrResultDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductUnitRepository _unitRepository;
        private readonly ILogger<GenerateMQRCommandHandler> _logger;
        private readonly IClientCompanyOwnerPolicy _ownerPolicy;
        private readonly Id12BytesCrypto _newId;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IAggregateIdParser _idParser;
        private readonly Guid _clientCompanyId;

        public GenerateMQRCommandHandler(IProductRepository productRepository, IProductUnitRepository unitRepository,
            ILogger<GenerateMQRCommandHandler> logger, IClientCompanyOwnerPolicy ownerPolicy, Id12BytesCrypto newId,
            ICodeGenerator codeGenerator, IAppContext appContext, IAggregateIdParser idParser)
        {
            _productRepository = productRepository;
            _unitRepository = unitRepository;
            _logger = logger;
            _ownerPolicy = ownerPolicy;
            _newId = newId;
            _codeGenerator = codeGenerator;
            _idParser = idParser;

            _clientCompanyId = appContext.Identity.ClientCompanyId;
        }

        private async Task<QrResultDto> HandleAsync(GenerateMQRCommand command)
        {
            _logger.LogInformation("Trying to generate a new MQR with parameters: {@Parameters}",
                new
                {
                    command.ProductId,
                    command.DateOfManufacture,
                    command.ExpiryDate,
                    command.Codes,
                    command.QRImageOptions
                });

            var productId = _idParser.Id12BytesFromString(command.ProductId);
            var product = await _productRepository.GetAsync(productId);

            if (product == null)
            {
                throw new ProductNotFoundException(command.ProductId);
            }

            if (!_ownerPolicy.IsOwnedBy(product, _clientCompanyId))
            {
                throw new ProductNotOwnedException(command.ProductId);
            }

            var unit = new ProductUnit(_newId, command.DateOfManufacture, command.ExpiryDate, command.Codes,
                product, (command as IPropertiesContainer).GetProperties(), QrCodeType.ProductUnitV2.Id);

            await _unitRepository.AddAsync(unit);

            _logger.LogInformation("Created new product unit: {ProductUnitId}", unit.Id.Value);

            var generationParameters =
                new QrImageGenerationParameters(unit, command.QRImageOptions, QrCodeType.ProductUnitV2);

            var code = await _codeGenerator.CreateCodeImage(generationParameters);

            _logger.LogInformation("Generated QR code for product unit: {ProductUnitId}", code.Id);

            return code;
        }

        public async Task HandleAsync(ICommandInfo<GenerateMQRCommand, QrResultDto> info)
        {
            info.Result = await HandleAsync(info.Command);
        }
    }
}