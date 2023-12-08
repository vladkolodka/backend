using System.Collections.Generic;

namespace Menchul.Core.ResourceProcessing.Entities
{
    public interface IResourceContainer : IResource
    {
        IEnumerable<IResource> GetResources();

        /// <summary>
        /// Determines if the container should also be processed as a resource
        /// </summary>
        bool ProcessValue => false;
    }
}