using Convey.Persistence.MongoDB;
using Menchul.Core.Entities;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.Repositories;
using Menchul.MCode.Infrastructure.Common;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.MCode.Infrastructure.Services.Interfaces;
using Menchul.Mongo;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Mongo.Repositories
{
    internal class ClientCompanyMongoRepository : IClientCompanyRepository
    {
        private readonly IMongoRepository<ClientCompanyDocument, Guid> _repository;
        private readonly ILocalizationService _localizationService;
        private readonly IRelationManager _relationManager;
        private readonly ILogger<ClientCompanyMongoRepository> _logger;

        public ClientCompanyMongoRepository(IMongoRepository<ClientCompanyDocument, Guid> repository,
            ILocalizationService localizationService,
            IRelationManager relationManager, ILogger<ClientCompanyMongoRepository> logger)
        {
            _repository = repository;
            _localizationService = localizationService;
            _relationManager = relationManager;
            _logger = logger;
        }

        public async Task<ClientCompany> GetAsync(IdGuid id)
        {
            _logger.LogDebug("GetAsync {ClientCompanyId}", id);

            var clientCompanyDocument = await _repository.GetAsync(c => c.Id == id);

            var company = clientCompanyDocument?.ToEntity();

            _logger.LogDebug("GetAsync {ClientCompanyId} - loaded: {ClientCompanyLoaded}", id, company != null);

            return company;
        }

        public async Task<ClientCompany> GetByEmailAsync(string email, bool loadLocalization = true)
        {
            _logger.LogDebug("GetByEmailAsync {ClientCompanyEmail}", email);

            var companyDocuments = await _repository.FindAsync(d => d.Email == email);
            var companyDocument = companyDocuments.FirstOrDefault();

            if (companyDocument != null && loadLocalization)
            {
                await _relationManager.LoadLocalization(companyDocument);
                _logger.LogDebug("GetByEmailAsync {ClientCompanyEmail} - loaded localizations", email);
            }

            var company = companyDocument?.ToEntity();

            _logger.LogDebug("GetByEmailAsync {ClientCompanyEmail} - loaded: {ClientCompanyLoaded}",
                email, company != null);

            return company;
        }

        public async Task<bool> ExistsAsync(IdGuid id)
        {
            _logger.LogDebug("ExistsAsync {ClientCompanyId}", id.Value);

            var exists = await _repository.ExistsAsync(c => c.Id == id.Value);

            _logger.LogDebug("ExistsAsync {ClientCompanyId} - exists: {ClientCompanyExists}", id.Value, exists);

            return exists;
        }

        public async Task AddAsync(ClientCompany clientCompany)
        {
            _logger.LogDebug("AddAsync {ClientCompanyId}", clientCompany.Id.Value);

            await _localizationService.ExtractSaveOrUpdate(clientCompany);

            _logger.LogDebug("AddAsync {ClientCompanyId} - saved localizations", clientCompany.Id.Value);

            var document = new ClientCompanyDocument(clientCompany);

            await _repository.AddAsync(document);

            _logger.LogDebug("AddAsync {ClientCompanyId} - saved", clientCompany.Id.Value);
        }
    }
}