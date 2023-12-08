using Menchul.MCode.Application.Packages.Queries;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.Mongo.QueryRunners;

namespace Menchul.MCode.Infrastructure.Mongo.QueryRunners.Parameters
{
    internal class PackageQueryParameters : IQueryParameters<PackageDocument>
    {
        public bool IsDeep { get; }

        public PackageQueryParameters(GetPackageQueryBase query)
        {
            IsDeep = query.Deep;
        }

        public PackageQueryParameters()
        {
        }
    }
}