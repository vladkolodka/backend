using Convey.CQRS.Commands;
using Menchul.Application;
using Menchul.MCode.Core.Exceptions;
using Menchul.MCode.Core.Policies;
using Menchul.MCode.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Companies.Commands.DeleteCompany
{
    public class DeleteCompanyCommandHandler : ICommandHandler<DeleteCompanyCommand>
    {
        private readonly ICompanyRepository _repository;
        private readonly IClientCompanyOwnerPolicy _ownerPolicy;
        private readonly ILogger<DeleteCompanyCommandHandler> _logger;
        private readonly Guid _clientCompanyId;

        public DeleteCompanyCommandHandler(ICompanyRepository repository, IClientCompanyOwnerPolicy ownerPolicy,
            IAppContext appContext, ILogger<DeleteCompanyCommandHandler> logger)
        {
            _repository = repository;
            _ownerPolicy = ownerPolicy;
            _logger = logger;

            _clientCompanyId = appContext.Identity.ClientCompanyId;
        }

        public async Task HandleAsync(DeleteCompanyCommand command)
        {
            _logger.LogInformation("Trying to delete the company: {CompanyId}", command.Id);

            var company = await _repository.GetAsync(command.Id);

            if (company == null)
            {
                throw new CompanyNotFoundException(command.Id);
            }

            if (!_ownerPolicy.IsOwnedBy(company, _clientCompanyId))
            {
                throw new CompanyNotOwnedException(command.Id);
            }

            await _repository.DeleteAsync(command.Id);

            _logger.LogInformation("Deleted company {CompanyId}", command.Id);
        }
    }
}