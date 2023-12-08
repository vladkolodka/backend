using Microsoft.AspNetCore.Http;

namespace Menchul.Infrastructure.Contexts
{
    public interface IHttpCorrelationContextAccessor
    {
        CorrelationContext GetCorrelationContext();

        IHttpContextAccessor HttpContextAccessor { get; }
    }
}