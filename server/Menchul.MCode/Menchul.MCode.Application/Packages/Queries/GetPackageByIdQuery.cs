using Convey.CQRS.Queries;
using Menchul.MCode.Application.Packages.Dto;

namespace Menchul.MCode.Application.Packages.Queries
{
    public class GetPackageByIdQuery : GetPackageQueryBase, IQuery<PackageDto>
    {
        public string Id { get; set; }

    }
}