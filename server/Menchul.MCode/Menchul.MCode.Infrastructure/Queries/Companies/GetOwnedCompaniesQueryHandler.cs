using Convey.CQRS.Queries;
using Menchul.Application;
using Menchul.MCode.Application.Companies.Dto;
using Menchul.MCode.Application.Companies.Queries;
using Menchul.MCode.Infrastructure.Common;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.Mongo;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Queries.Companies
{
    internal class GetOwnedCompaniesQueryHandler : IQueryHandler<GetOwnedCompaniesQuery, List<CompanyDto>>
    {
        private readonly IRelationManager _relationManager;
        private readonly Guid _clientCompanyId;

        public GetOwnedCompaniesQueryHandler(IRelationManager relationManager, IAppContext appContext)
        {
            _relationManager = relationManager;
            _clientCompanyId = appContext.Identity.ClientCompanyId;
        }

        public async Task<List<CompanyDto>> HandleAsync(GetOwnedCompaniesQuery query)
        {
            var companies =
                await _relationManager.Load<CompanyDocument, Guid>(f =>
                    f.Match(c => c.ClientCompany.RefId == _clientCompanyId));

            await _relationManager.LoadLocalization(companies);

            return companies?.Select(c => c.ToDto()).ToList();
        }
    }
}