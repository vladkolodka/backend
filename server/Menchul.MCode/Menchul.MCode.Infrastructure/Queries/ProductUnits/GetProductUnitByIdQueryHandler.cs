using Convey.CQRS.Queries;
using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.ProductUnits.Queries;
using Menchul.Mongo;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Queries.ProductUnits
{
    internal class GetProductUnitByIdQueryHandler : GetProductUnitQueryHandlerBase, IQueryHandler<GetProductUnitByIdQuery, PassportDto>
    {
        public GetProductUnitByIdQueryHandler(IRelationManager relationManager) : base(relationManager)
        {
        }

        public async Task<PassportDto> HandleAsync(GetProductUnitByIdQuery query)
        {
            var unitId = new ObjectId(query.Id);
            var unit = await GetProductUnitById(unitId, query);

            return PassportFrom(unit);
        }
    }
}