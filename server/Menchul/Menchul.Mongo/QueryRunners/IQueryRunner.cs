using Menchul.Mongo.Common;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menchul.Mongo.QueryRunners
{
    /// <summary>
    /// Reusable query
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    public interface IQueryRunner<TDocument> where TDocument : IDocumentRoot
    {
        Task<List<TDocument>> Run(AggregateFluent<TDocument> queryModifier = null,
            QueryParametersContainer parametersContainer = null);
    }

    public delegate IAggregateFluent<TDocument> AggregateFluent<TDocument>(IAggregateFluent<TDocument> fluent);

    public delegate IQueryRunner<TDocument> QueryRunnerResolver<TDocument>(QueryRunnerType? key = null)
        where TDocument : IDocumentRoot;
}