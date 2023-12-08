using Menchul.Application;
using System;
using System.Threading;

namespace Menchul.Infrastructure.Contexts
{
    public sealed class AppContext : IAppContext
    {
        public string RequestId { get; }
        public IIdentityContext Identity { get; }
        public CancellationToken CancellationToken { get; set; }

        public AppContext() : this(Guid.NewGuid().ToString("N"), IdentityContext.Empty)
        {
        }

        public AppContext(CorrelationContext context) : this(context.CorrelationId,
            context.User is null ? IdentityContext.Empty : new IdentityContext(context.User))
        {
        }

        public AppContext(string requestId, IIdentityContext identity)
        {
            RequestId = requestId;
            Identity = identity;
        }

        internal static IAppContext Empty => new AppContext();
    }
}