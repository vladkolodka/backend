using Convey.CQRS.Queries;
using Menchul.MCode.Application.Common.Queries;
using Menchul.MCode.Application.Products.Dto;

namespace Menchul.MCode.Application.Products.Queries
{
    public class GetProductByEanQuery : QueryBase, IQuery<ProductDto>
    {
        public long EAN { get; set; }
    }
}