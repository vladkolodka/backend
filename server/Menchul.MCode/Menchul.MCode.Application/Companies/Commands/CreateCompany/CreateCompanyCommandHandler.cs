using Convey.CQRS.Commands;
using Menchul.Application;
using Menchul.Convey;
using Menchul.Core.Entities;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.Exceptions;
using Menchul.MCode.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Companies.Commands.CreateCompany
{
    public class CreateCompanyCommandHandler : ICommandHandler<ICommandInfo<CreateCompanyCommand, Guid>>
    {
        private readonly ICompanyRepository _repository;
        private readonly IdGuid _newId;
        private readonly ILogger<CreateCompanyCommandHandler> _logger;
        private readonly IClientCompanyRepository _clientCompanyRepository;
        private readonly Guid _clientCompanyId;

        public CreateCompanyCommandHandler(ICompanyRepository repository,
            IClientCompanyRepository clientCompanyRepository,
            IAppContext appContext, IdGuid newId, ILogger<CreateCompanyCommandHandler> logger)
        {
            _repository = repository;
            _clientCompanyRepository = clientCompanyRepository;
            _newId = newId;
            _logger = logger;

            _clientCompanyId = appContext.Identity.ClientCompanyId;
        }

        private async Task<Guid> HandleAsync(CreateCompanyCommand command)
        {
            if (!await _clientCompanyRepository.ExistsAsync(_clientCompanyId))
            {
                throw new ClientCompanyNotFoundException(_clientCompanyId);
            }

            var company = new Company(_newId, command.Name, command.Description, command.Phone, command.Email,
                command.Url,
                command.Address.ToValueObject(), command.Certificates, _clientCompanyId);

            await _repository.AddAsync(company);

            _logger.LogInformation("Created company {CompanyId}", company.Id.Value);

            return _newId;
        }

        public async Task HandleAsync(ICommandInfo<CreateCompanyCommand, Guid> command)
        {
            command.Result = await HandleAsync(command.Command);
        }
    }
}