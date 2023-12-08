using Convey.CQRS.Commands;
using Menchul.Convey;
using Menchul.Core.Entities;
using Menchul.MCode.Application.Companies.Exceptions;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Companies.Commands.CreateClientCompany
{
    public class CreateClientCompanyCommandHandler : ICommandHandler<ICommandInfo<CreateClientCompanyCommand, Guid>>
    {
        private readonly IClientCompanyRepository _repository;
        private readonly IdGuid _newId;
        private readonly Id12Bytes _newInternalId;
        private readonly ILogger<CreateClientCompanyCommandHandler> _logger;

        public CreateClientCompanyCommandHandler(IClientCompanyRepository repository, IdGuid newId,
            Id12Bytes newInternalId,
            ILogger<CreateClientCompanyCommandHandler> logger)
        {
            _repository = repository;
            _newId = newId;
            _newInternalId = newInternalId;
            _logger = logger;
        }

        private async Task<Guid> HandleAsync(CreateClientCompanyCommand command)
        {
            IdGuid id = _newId;

            _logger.LogInformation("Trying to create a new client company '{ClientCompanyId}' with email '{Email}'",
                command.Id, command.Email);

            var existingCompanyByEmail = await _repository.GetByEmailAsync(command.Email, loadLocalization: false);

            if (existingCompanyByEmail is { })
            {
                throw new CompanyAlreadyExistsException(existingCompanyByEmail.Id.Value, command.Email);
            }

            if (command.Id != Guid.Empty)
            {
                var companyId = new IdGuid(command.Id);

                var companyExistsWithId = await _repository.ExistsAsync(companyId);

                if (companyExistsWithId)
                {
                    throw new CompanyAlreadyExistsException(companyId);
                }

                id = companyId;
            }

            var company = new ClientCompany(id, _newInternalId, command.Name, command.Email, command.Phone,
                command.Url, command.LegalAddress.ToValueObject(), command.MailAddress.ToValueObject());

            await _repository.AddAsync(company);

            _logger.LogInformation("Created new client company: '{ClientCompanyId}'", command.Id);

            return company.Id;
        }

        public async Task HandleAsync(ICommandInfo<CreateClientCompanyCommand, Guid> info)
        {
            info.Result = await HandleAsync(info.Command);
        }
    }
}