using Menchul.MCode.Application.Common.Queries;

namespace Menchul.MCode.Application.Packages.Queries
{
    public class GetPackageQueryBase : QueryBase
    {
        public bool Deep { get; set; }
    }
}