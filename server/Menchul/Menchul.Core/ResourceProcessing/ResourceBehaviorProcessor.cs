using Menchul.Core.ResourceProcessing.Behaviors;
using Menchul.Core.ResourceProcessing.Entities;
using System.Collections;
using System.Collections.Generic;

namespace Menchul.Core.ResourceProcessing
{
    public class ResourceBehaviorProcessor: IResourceVisitor, IEnumerable
    {
        private readonly List<IResourceVisitorBehavior> _behaviors = new();

        public void Visit(IResource resource)
        {
            ExecuteBehaviors(resource);
        }

        public void Visit(IResourceContainer resourceContainer)
        {
            if (resourceContainer.ProcessValue)
            {
                ExecuteBehaviors(resourceContainer);
            }

            var nestedResources = resourceContainer.GetResources();

            if (nestedResources == null)
            {
                return;
            }

            foreach (var resource in nestedResources)
            {
                resource?.Accept(this);
            }
        }

        // this allows to pass multiple behaviors to object initializer
        public void Add(IResourceVisitorBehavior behavior) => _behaviors.Add(behavior);
        public IEnumerator GetEnumerator() => _behaviors.GetEnumerator();

        private void ExecuteBehaviors(IResource resource)
        {
            _behaviors.ForEach(behavior =>
            {
                if (resource != null && behavior.CanProcess(resource))
                {
                    behavior.Process(resource);
                }
            });
        }
    }
}