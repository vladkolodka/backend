using Convey.CQRS.Queries;
using Menchul.MCode.Application.Products.Dto;
using Menchul.MCode.Application.Products.Queries;
using Menchul.MCode.Infrastructure.Common;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Queries.Products
{
    internal class GetProductByEANQueryHandler : IQueryHandler<GetProductByEanQuery, ProductDto>
    {
        private readonly IRelationManager _relationManager;

        public GetProductByEANQueryHandler(IRelationManager relationManager)
        {
            _relationManager = relationManager;
        }

        public async Task<ProductDto> HandleAsync(GetProductByEanQuery query)
        {
            var foundProducts =
                await _relationManager.Load<ProductDocument, ObjectId>(f => f.Match(p => p.EAN == query.EAN));

            if (foundProducts?.Count != 1)
            {
                return null;
            }

            var productDocument = foundProducts.First();

            await _relationManager.LoadLocalization(productDocument, query);

            return productDocument.ToDto();
        }
    }
}