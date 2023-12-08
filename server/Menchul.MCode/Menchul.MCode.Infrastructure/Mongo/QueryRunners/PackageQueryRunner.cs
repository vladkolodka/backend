using Convey.Persistence.MongoDB;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.MCode.Infrastructure.Mongo.QueryRunners.Parameters;
using Menchul.Mongo;
using Menchul.Mongo.QueryRunners;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Mongo.QueryRunners
{
    internal class PackageQueryRunner : IQueryRunner<PackageDocument>
    {
        private readonly IMongoRepository<PackageDocument, ObjectId> _repository;
        private readonly IRelationManager _relationManager;

        public PackageQueryRunner(IMongoRepository<PackageDocument, ObjectId> repository,
            IRelationManager relationManager)
        {
            _repository = repository;
            _relationManager = relationManager;
        }

        public async Task<List<PackageDocument>> Run(AggregateFluent<PackageDocument> queryModifier = null,
            QueryParametersContainer parametersContainer = null)
        {
            var isDeep = parametersContainer.GetSafe<PackageQueryParameters, PackageDocument>().IsDeep;

            var query = _repository.Collection.Aggregate().ApplyModifier(queryModifier);

            IEnumerable<PackageDocument> packages;

            if (isDeep)
            {
                var packageHierarchies = await query.GraphLookup(
                        from: _repository.Collection,
                        startWith: $"${PackageDocument.PackagesName}.{Constants.Properties.RefId}",
                        connectFromField: $"{nameof(PackageDocument.Packages)}.{Constants.Properties.RefId}",
                        connectToField: nameof(PackageDocument.Id),
                        @as: PackageDocumentWithHierarchy.HierarchyName
                    ).As<PackageDocumentWithHierarchy>()

                    .ToListAsync();

                var allPackages = packageHierarchies.FlattenHierarchies();

                if (packageHierarchies.Count != allPackages.Count)
                {
                    _relationManager.Map<PackageDocument, ObjectId>(packageHierarchies, allPackages);
                }

                packages = packageHierarchies;
            }
            else
            {
                packages = await query.ToListAsync();
            }

            await _relationManager.MapLoad<ProductUnitDocument, ObjectId>(packages);

            return packages.ToList();
        }
    }
}