using System.Threading;

namespace Menchul.Application
{
    public interface IAppContext
    {
        string RequestId { get; }
        IIdentityContext Identity { get; }
        public CancellationToken CancellationToken { get; set; }
    }
}