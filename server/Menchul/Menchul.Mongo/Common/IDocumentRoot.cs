using Convey.Types;
using Menchul.Mongo.Resources;

namespace Menchul.Mongo.Common
{
    /// <summary>
    /// Top-level Mongo collection document
    /// </summary>
    public interface IDocumentRoot : IDocumentResourceContainer
    {
    }

    /// <summary>
    /// Top-level Mongo collection document
    /// </summary>
    public interface IDocumentRoot<out TId> : IIdentifiable<TId>, IDocumentRoot
    {
    }
}