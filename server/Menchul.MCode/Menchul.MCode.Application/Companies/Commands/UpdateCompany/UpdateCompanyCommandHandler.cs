using Convey.CQRS.Commands;
using Menchul.Application;
using Menchul.MCode.Core.Exceptions;
using Menchul.MCode.Core.Policies;
using Menchul.MCode.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Companies.Commands.UpdateCompany
{
    public class UpdateCompanyCommandHandler : ICommandHandler<UpdateCompanyCommand>
    {
        private readonly ICompanyRepository _repository;
        private readonly IClientCompanyOwnerPolicy _ownerPolicy;
        private readonly ILogger<UpdateCompanyCommandHandler> _logger;
        private readonly Guid _clientCompanyId;

        public UpdateCompanyCommandHandler(ICompanyRepository repository, IAppContext appContext,
            IClientCompanyOwnerPolicy ownerPolicy, ILogger<UpdateCompanyCommandHandler> logger)
        {
            _repository = repository;
            _ownerPolicy = ownerPolicy;
            _logger = logger;
            _clientCompanyId = appContext.Identity.ClientCompanyId;
        }

        public async Task HandleAsync(UpdateCompanyCommand command)
        {
            _logger.LogInformation("Trying to update the company {CompanyId}", command.Id);
            _logger.LogDebug("Update company command: {@UpdateCompanyCommand}", command);

            var company = await _repository.GetAsync(command.Id);

            if (company == null)
            {
                throw new CompanyNotFoundException(command.Id);
            }

            if (!_ownerPolicy.IsOwnedBy(company, _clientCompanyId))
            {
                throw new CompanyNotOwnedException(command.Id);
            }

            company.Rename(command.Name);
            company.ChangeDescription(command.Description);
            company.ChangePhone(command.Phone);
            company.ChangeEmail(command.Email);
            company.ChangeUrl(command.Url);
            company.ChangeAddress(command.Address.ToValueObject());
            company.ReplaceCertificates(command.Certificates);

            await _repository.UpdateAsync(company);

            _logger.LogInformation("Updated the company {CompanyId}", command.Id);
        }
    }
}