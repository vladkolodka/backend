using Convey.Persistence.MongoDB;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.Mongo.QueryRunners;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Mongo.QueryRunners
{
    internal class CompanyQueryRunner : IQueryRunner<CompanyDocument>
    {
        private readonly IMongoRepository<CompanyDocument, Guid> _repository;

        public CompanyQueryRunner(IMongoRepository<CompanyDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task<List<CompanyDocument>> Run(AggregateFluent<CompanyDocument> queryModifier = null,
            QueryParametersContainer parametersContainer = null) =>
            _repository.Collection.Aggregate().ApplyModifier(queryModifier).ToListAsync();
    }
}