using Menchul.MCode.Application.Packages.Queries;
using Menchul.MCode.Infrastructure.Common;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.MCode.Infrastructure.Mongo.QueryRunners.Parameters;
using Menchul.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Queries.Packages
{
    internal class GetPackageQueryHandlerBase
    {
        private readonly IRelationManager _relationManager;

        protected GetPackageQueryHandlerBase(IRelationManager relationManager)
        {
            _relationManager = relationManager;
        }

        protected async Task<PackageDocument> GetPackageById(ObjectId packageId, GetPackageQueryBase query)
        {
            var packages = await _relationManager.Load<PackageDocument, ObjectId>(f =>
                f.Match(p => p.Id == packageId), new PackageQueryParameters(query).ToContainer());

            await _relationManager.LoadLocalization(packages, query);

            return packages.FirstOrDefault();
        }
    }
}