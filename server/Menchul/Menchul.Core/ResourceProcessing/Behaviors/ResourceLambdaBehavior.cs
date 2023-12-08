using Menchul.Core.ResourceProcessing.Entities;
using System;

namespace Menchul.Core.ResourceProcessing.Behaviors
{
    public class ResourceLambdaBehavior<T> : IResourceVisitorBehavior where T: class, IResource
    {
        private readonly Action<T> _action;

        public ResourceLambdaBehavior(Action<T> action)
        {
            _action = action;
        }

        public void Process(IResource resource)
        {
            _action.Invoke(resource as T);
        }

        public bool CanProcess(IResource resource) => resource is T;
    }
}