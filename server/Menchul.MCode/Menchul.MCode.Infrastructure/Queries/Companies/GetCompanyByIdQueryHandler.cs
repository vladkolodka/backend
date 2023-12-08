using Convey.CQRS.Queries;
using Menchul.MCode.Application.Companies.Dto;
using Menchul.MCode.Application.Companies.Queries;
using Menchul.MCode.Infrastructure.Common;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.Mongo;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Queries.Companies
{
    internal class GetCompanyByIdQueryHandler : IQueryHandler<GetCompanyByIdQuery, CompanyDto>
    {
        private readonly IRelationManager _relationManager;

        public GetCompanyByIdQueryHandler(IRelationManager relationManager)
        {
            _relationManager = relationManager;
        }

        public async Task<CompanyDto> HandleAsync(GetCompanyByIdQuery query)
        {
            var company = (await _relationManager.Load<CompanyDocument, Guid>(f => f.Match(c => c.Id == query.Id)))
                ?.FirstOrDefault();

            await _relationManager.LoadLocalization(company);

            return company?.ToDto();
        }
    }
}