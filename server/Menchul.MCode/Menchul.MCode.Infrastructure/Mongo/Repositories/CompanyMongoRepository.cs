using Menchul.Core.Entities;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.Repositories;
using Menchul.MCode.Infrastructure.Common;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.MCode.Infrastructure.Services.Interfaces;
using Menchul.Mongo;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Mongo.Repositories
{
    internal class CompanyMongoRepository : ICompanyRepository
    {
        private readonly IMenchulMongoRepository<CompanyDocument, Guid> _repository;
        private readonly ILocalizationService _localizationService;
        private readonly IRelationManager _relationManager;
        private readonly ILogger<CompanyMongoRepository> _logger;

        public CompanyMongoRepository(IMenchulMongoRepository<CompanyDocument, Guid> repository,
            ILocalizationService localizationService, IRelationManager relationManager,
            ILogger<CompanyMongoRepository> logger)
        {
            _repository = repository;
            _localizationService = localizationService;
            _relationManager = relationManager;
            _logger = logger;
        }

        public async Task AddAsync(Company company)
        {
            _logger.LogDebug("AddAsync {CompanyId}", company.Id.Value);

            await _localizationService.ExtractSaveOrUpdate(company);

            var companyDocument = new CompanyDocument(company);

            await _repository.AddAsync(companyDocument);

            _logger.LogDebug("AddAsync {CompanyId} - saved", company.Id.Value);

            await _repository.TrackAsync(companyDocument);

            _logger.LogDebug("AddAsync {CompanyId} - tracking enabled", company.Id.Value);
        }

        public async Task UpdateAsync(Company company)
        {
            _logger.LogDebug("UpdateAsync {CompanyId}", company.Id.Value);

            await _localizationService.ExtractSaveOrUpdate(company);

            _logger.LogDebug("UpdateAsync {CompanyId} - saved localizations", company.Id.Value);

            var companyDocument = new CompanyDocument(company);

            await _repository.UpdateAsync(companyDocument);

            _logger.LogDebug("UpdateAsync {CompanyId} - saved", company.Id.Value);
        }

        public async Task<Company> GetAsync(IdGuid id)
        {
            _logger.LogDebug("GetAsync {CompanyId}", id.Value);

            var companyDocument = await _repository.GetAsync(id.Value);

            await _relationManager.LoadLocalization(companyDocument);

            _logger.LogDebug("GetAsync {CompanyId} - localizations loaded", id.Value);

            var company = companyDocument?.ToEntity();

            _logger.LogDebug("GetAsync {CompanyId} - loaded: {CompanyLoaded}", id.Value, company != null);

            return company;
        }

        public async Task DeleteAsync(IdGuid id)
        {
            _logger.LogDebug("DeleteAsync {CompanyId}", id.Value);

            await _repository.DeleteTrackedAsync(id.Value);

            _logger.LogDebug("DeleteAsync {CompanyId} - deleted tracked", id.Value);
        }

        public async Task<List<Guid>> ExistsAllAsync(List<Guid> ids)
        {
            _logger.LogDebug("ExistsAllAsync {CompanyIds}", ids);

            var existingIds = await _repository.Collection.AsQueryable()
                .Where(c => ids.Contains(c.Id)).Select(d => d.Id).ToListAsync();

            _logger.LogDebug("ExistsAllAsync existing: {CompanyIds}", existingIds);

            return existingIds;
        }
    }
}