using Menchul.Core.ResourceProcessing;
using Menchul.Core.ResourceProcessing.Entities;

namespace Menchul.Mongo.Resources
{
    /// <summary>
    /// Represents an entity that can be scanned to find values
    /// </summary>
    public interface IDocumentResource : IResource
    {
        void IResource.Accept(IResourceVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}