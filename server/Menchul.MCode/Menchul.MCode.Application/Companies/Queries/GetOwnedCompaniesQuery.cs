using Convey.CQRS.Queries;
using Menchul.MCode.Application.Common.Queries;
using Menchul.MCode.Application.Companies.Dto;
using System.Collections.Generic;

namespace Menchul.MCode.Application.Companies.Queries
{
    public class GetOwnedCompaniesQuery : QueryBase, IQuery<List<CompanyDto>>
    {
    }
}