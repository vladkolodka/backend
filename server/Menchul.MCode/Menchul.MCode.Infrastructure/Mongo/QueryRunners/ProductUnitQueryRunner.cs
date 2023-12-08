using Convey.Persistence.MongoDB;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.Mongo;
using Menchul.Mongo.QueryRunners;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Mongo.QueryRunners
{
    internal class ProductUnitQueryRunner : IQueryRunner<ProductUnitDocument>
    {
        private readonly IMongoRepository<ProductUnitDocument, ObjectId> _repository;
        private readonly IRelationManager _relationManager;

        public ProductUnitQueryRunner(IMongoRepository<ProductUnitDocument, ObjectId> repository,
            IRelationManager relationManager)
        {
            _repository = repository;
            _relationManager = relationManager;
        }

        public async Task<List<ProductUnitDocument>> Run(AggregateFluent<ProductUnitDocument> queryModifier = null,
            QueryParametersContainer parametersContainer = null)
        {
            var units = await _repository.Collection.Aggregate().ApplyModifier(queryModifier).ToListAsync();
            await _relationManager.MapLoad<ProductDocument, ObjectId>(units, parametersContainer);

            return units;
        }
    }
}