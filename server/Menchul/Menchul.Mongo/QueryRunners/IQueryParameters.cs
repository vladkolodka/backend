using Menchul.Mongo.Common;

namespace Menchul.Mongo.QueryRunners
{
    /// <summary>
    /// Parameters for the <see cref="IQueryRunner{TDocument}"/>
    /// </summary>
    /// <typeparam name="TForDocument">Query runner' document type</typeparam>
    public interface IQueryParameters<TForDocument> where TForDocument: IDocumentRoot
    {
    }
}