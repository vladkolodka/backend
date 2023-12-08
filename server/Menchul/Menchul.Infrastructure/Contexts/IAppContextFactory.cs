using Menchul.Application;

namespace Menchul.Infrastructure.Contexts
{
    public interface IAppContextFactory
    {
        IAppContext Create();
        IAppContext CreateLocal();
    }
}