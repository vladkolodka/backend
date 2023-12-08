using Menchul.Core.ResourceProcessing;
using Menchul.Core.ResourceProcessing.Entities;
using System.Collections.Generic;

namespace Menchul.Mongo.Resources
{
    /// <summary>
    /// A helper class allowing to scan multiple resources from different hierarchies
    /// </summary>
    public class DocumentResourceContainer : IDocumentResourceContainer
    {
        private readonly IEnumerable<IDocumentResource> _resources;

        public DocumentResourceContainer(IEnumerable<IDocumentResource> resources)
        {
            _resources = resources;
        }

        public void Accept(IResourceVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IEnumerable<IResource> GetResources() => _resources;
    }
}