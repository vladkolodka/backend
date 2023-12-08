using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.Common.Queries;
using Menchul.MCode.Application.Services;
using Menchul.MCode.Infrastructure.Common;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.MCode.Infrastructure.Mongo.QueryRunners.Parameters;
using Menchul.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Queries.ProductUnits
{
    internal abstract class GetProductUnitQueryHandlerBase
    {
        private readonly IRelationManager _relationManager;

        protected GetProductUnitQueryHandlerBase(IRelationManager relationManager)
        {
            _relationManager = relationManager;
        }

        protected async Task<ProductUnitDocument> GetProductUnitById(ObjectId unitId, QueryBase query)
        {
            var queryParameters = new ProductQueryParameters {ExcludeMetadata = !query.UseUiMode}.ToContainer();

            var units = await _relationManager
                .Load<ProductUnitDocument, ObjectId>(f =>
                    f.Match(p => p.Id == unitId), queryParameters);

            await _relationManager.LoadLocalization(units, query);

            return units.FirstOrDefault();
        }

        protected PassportDto PassportFrom(ProductUnitDocument document)
        {
            if (document == null)
            {
                return null;
            }

            return new PassportBuilder()
                .Append(document.Product?.Document?.ToDto()) // append product
                .Append(document.ToDto()) // append product unit
                .Build();
        }
    }
}