using Menchul.Core.ResourceProcessing;
using Menchul.Core.ResourceProcessing.Entities;
using System.Collections.Generic;

namespace Menchul.Mongo.Resources
{
    /// <summary>
    /// A helper interface which allows recursive resource scanning
    /// </summary>
    public interface IDocumentResourceContainer : IResourceContainer, IDocumentResource
    {
        IEnumerable<IResource> IResourceContainer.GetResources() => null;

        void IResource.Accept(IResourceVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}