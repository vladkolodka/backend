using Convey.CQRS.Queries;
using Menchul.MCode.Application.Products.Dto;

namespace Menchul.MCode.Application.Products.Queries
{
    public class GetProductByIdQuery : IQuery<ProductDto>
    {
        public string Id { get; set; }
    }
}