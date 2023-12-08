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
    internal class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IRelationManager _relationManager;

        public GetProductByIdQueryHandler(IRelationManager relationManager)
        {
            _relationManager = relationManager;
        }

        public async Task<ProductDto> HandleAsync(GetProductByIdQuery query)
        {
            var productId = new ObjectId(query.Id);

            var productDocument = (await _relationManager.Load<ProductDocument, ObjectId>(
                f => f.Match(d => d.Id == productId))).FirstOrDefault();

            await _relationManager.LoadLocalization(productDocument);

            return productDocument?.ToDto();
        }
    }
}