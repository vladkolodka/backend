using Menchul.Mongo.Common;
using Menchul.Mongo.QueryRunners;
using Menchul.Mongo.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menchul.Mongo
{
    public interface IRelationManager
    {
        Task<List<TDocument>> MapLoad<TDocument, TId>(IEnumerable<IDocumentResource> parentResources,
            QueryParametersContainer parametersContainer = null, QueryRunnerType? runnerKey = null)
            where TDocument : IDocumentRoot<TId>;


        /// <summary>
        /// Load referenced child documents.
        /// </summary>
        /// <param name="parentResource"></param>
        /// <param name="parametersContainer"></param>
        /// <param name="runnerKey"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <returns></returns>
        Task<List<TDocument>> MapLoad<TDocument, TId>(IDocumentResource parentResource,
            QueryParametersContainer parametersContainer = null, QueryRunnerType? runnerKey = null)
            where TDocument : IDocumentRoot<TId>;

        Task<ReferenceMap<TId>> ValidateReferences<TDocument, TId>(IEnumerable<IDocumentResource> parentResources)
            where TDocument : IDocumentRoot<TId>;

        /// <summary>
        /// Ensures that referenced documents exist.
        /// </summary>
        /// <param name="parentResource"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <returns></returns>
        Task<ReferenceMap<TId>> ValidateReferences<TDocument, TId>(IDocumentResource parentResource)
            where TDocument : IDocumentRoot<TId>;

        /// <summary>
        /// Execute appropriate query runner for document type.
        /// </summary>
        /// <param name="queryModifier"></param>
        /// <param name="parametersContainer"></param>
        /// <param name="runnerKey"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <returns></returns>
        Task<List<TDocument>> Load<TDocument, TId>(AggregateFluent<TDocument> queryModifier = null,
            QueryParametersContainer parametersContainer = null, QueryRunnerType? runnerKey = null)
            where TDocument : IDocumentRoot<TId>;

        /// <summary>
        /// Fill REFs of the parent document with child documents
        /// </summary>
        /// <param name="parentResources"></param>
        /// <param name="children"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <returns></returns>
        List<TDocument> Map<TDocument, TId>(IEnumerable<IDocumentResource> parentResources, List<TDocument> children)
            where TDocument : IDocumentRoot<TId>;

        List<TDocument> Map<TDocument, TId>(IDocumentResource parentResource, List<TDocument> children)
            where TDocument : IDocumentRoot<TId>;
    }
}