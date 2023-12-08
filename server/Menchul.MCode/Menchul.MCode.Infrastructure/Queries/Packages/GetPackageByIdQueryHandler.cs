using Convey.CQRS.Queries;
using Menchul.MCode.Application.Packages.Dto;
using Menchul.MCode.Application.Packages.Queries;
using Menchul.Mongo;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Queries.Packages
{
    internal class GetPackageByIdQueryHandler : GetPackageQueryHandlerBase, IQueryHandler<GetPackageByIdQuery, PackageDto>
    {
        public GetPackageByIdQueryHandler(IRelationManager relationManager) : base(relationManager)
        {
        }

        public async Task<PackageDto> HandleAsync(GetPackageByIdQuery query)
        {
            var packageId = new ObjectId(query.Id);
            var package = await GetPackageById(packageId, query);

            return package?.ToDto();
        }
    }
}