using Convey.CQRS.Queries;
using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.Common.Queries;

namespace Menchul.MCode.Application.ProductUnits.Queries
{
    public class GetProductUnitByIdQuery : QueryBase, IQuery<PassportDto>
    {
        public string Id { get; set; }
    }
}