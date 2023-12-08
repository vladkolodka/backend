using Convey.CQRS.Queries;
using Menchul.MCode.Application.Packages.Dto;

namespace Menchul.MCode.Application.Packages.Queries
{
    public class GetPackageByHashQuery : GetPackageQueryBase, IQuery<PackageDto>
    {
        public string QR { get; set; }
    }
}