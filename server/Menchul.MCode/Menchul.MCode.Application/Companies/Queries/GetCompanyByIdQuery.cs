using Convey.CQRS.Queries;
using Menchul.MCode.Application.Common.Queries;
using Menchul.MCode.Application.Companies.Dto;
using System;

namespace Menchul.MCode.Application.Companies.Queries
{
    public class GetCompanyByIdQuery : QueryBase, IQuery<CompanyDto>
    {
        public Guid Id { get; set; }
    }
}