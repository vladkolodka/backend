namespace Menchul.Core.ResourceProcessing.Entities
{
    public interface IResource
    {
        void Accept(IResourceVisitor visitor);
    }
}