using Convey.CQRS.Commands;
using Menchul.Application;
using Menchul.Convey;
using Menchul.Core.Events;
using Menchul.MCode.Application.Common;
using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.Common.Enums;
using Menchul.MCode.Application.Common.Models;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.Exceptions;
using Menchul.MCode.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Packages.Commands
{
    public class GeneratePQRCommandHandler : ICommandHandler<ICommandInfo<GeneratePQRCommand, QrResultDto>>
    {
        private readonly Id12BytesCrypto _newId;
        private readonly IDomainEventDispatcher _eventDispatcher;
        private readonly IClientCompanyRepository _clientCompanyRepository;
        private readonly IPackageRepository _repository;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IAggregateIdParser _idParser;
        private readonly ILogger<GeneratePQRCommandHandler> _logger;
        private readonly Guid _clientCompanyId;

        public GeneratePQRCommandHandler(Id12BytesCrypto newId, IAppContext appContext,
            IDomainEventDispatcher eventDispatcher,
            IClientCompanyRepository clientCompanyRepository, IPackageRepository repository,
            ICodeGenerator codeGenerator,
            IAggregateIdParser idParser, ILogger<GeneratePQRCommandHandler> logger)
        {
            _clientCompanyId = appContext.Identity.ClientCompanyId;
            _newId = newId;
            _eventDispatcher = eventDispatcher;
            _clientCompanyRepository = clientCompanyRepository;
            _repository = repository;
            _codeGenerator = codeGenerator;
            _idParser = idParser;
            _logger = logger;
        }

        private async Task<QrResultDto> HandleAsync(GeneratePQRCommand command)
        {
            _logger.LogInformation("Trying to generate a new PQR with parameters: {@Parameters}",
                new { command.DateOfPackaging, command.Packages, command.Products, command.QRImageOptions });

            var clientCompany = await _clientCompanyRepository.GetAsync(_clientCompanyId);

            if (clientCompany == null)
            {
                throw new ClientCompanyNotFoundException(_clientCompanyId);
            }

            var nestedPackages = command.Packages == null ? null : _idParser.BsonIdsFromStrings(command.Packages);
            var nestedProducts = command.Products == null ? null : _idParser.BsonIdsFromStrings(command.Products);

            var package = Package.CreateNew(_newId, _clientCompanyId, command.DateOfPackaging, nestedPackages,
                nestedProducts, (command as IPropertiesContainer).GetProperties());

            await _repository.AddAsync(package);

            _logger.LogInformation("Created new package: {PackageId}", package.Id.Value);

            var generationParameters =
                new QrImageGenerationParameters(package, command.QRImageOptions, QrCodeType.PackageV1, clientCompany);

            // TODO do not hold the request https://www.quartz-scheduler.net/
            var _ = Task.Run(() => _eventDispatcher.DispatchAsync(package.Events.ToArray()));

            var code = await _codeGenerator.CreateCodeImage(generationParameters);

            _logger.LogInformation("Generated QR code for package: {PackageId}", code.Id);

            return code;
        }

        public async Task HandleAsync(ICommandInfo<GeneratePQRCommand, QrResultDto> command)
        {
            command.Result = await HandleAsync(command.Command);
        }
    }
}