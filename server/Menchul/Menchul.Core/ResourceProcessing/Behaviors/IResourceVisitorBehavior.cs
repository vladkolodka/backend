using Menchul.Core.ResourceProcessing.Entities;

namespace Menchul.Core.ResourceProcessing.Behaviors
{
    public interface IResourceVisitorBehavior
    {
        void Process(IResource resource);
        bool CanProcess(IResource resource);
    }
}