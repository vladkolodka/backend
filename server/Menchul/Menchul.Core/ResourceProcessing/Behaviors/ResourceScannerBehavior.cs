using Menchul.Core.ResourceProcessing.Entities;
using System.Collections.Generic;

namespace Menchul.Core.ResourceProcessing.Behaviors
{
    public class ResourceScannerBehavior<T> : IResourceVisitorBehavior where T : IResource
    {
        private readonly List<T> _capturedResources = new();

        public void Process(IResource resource)
        {
            _capturedResources.Add((T) resource);
        }

        public bool CanProcess(IResource resource)
        {
            return resource is T;
        }

        public IReadOnlyCollection<T> GetResources()
        {
            return _capturedResources;
        }
    }

}