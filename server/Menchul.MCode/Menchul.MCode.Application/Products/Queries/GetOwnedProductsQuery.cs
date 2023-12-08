using Convey.CQRS.Queries;
using Menchul.MCode.Application.Common.Queries;
using Menchul.MCode.Application.Products.Dto;
using System.Collections.Generic;

namespace Menchul.MCode.Application.Products.Queries
{
    public class GetOwnedProductsQuery : QueryBase, IQuery<List<ProductDto>>
    {
    }
}