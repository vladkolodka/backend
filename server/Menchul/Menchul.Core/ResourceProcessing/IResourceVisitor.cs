using Menchul.Core.ResourceProcessing.Entities;

namespace Menchul.Core.ResourceProcessing
{
    public interface IResourceVisitor
    {
        void Visit(IResource resource);

        void Visit(IResourceContainer resourceContainer);
    }
}