using Microsoft.AspNetCore.Http;
using Open.Serialization.Json;
using System.Linq;

namespace Menchul.Infrastructure.Contexts
{
    public class HttpCorrelationContextAccessor : IHttpCorrelationContextAccessor
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IJsonSerializer _serializer;

        public HttpCorrelationContextAccessor(IHttpContextAccessor httpContextAccessor, IJsonSerializer serializer)
        {
            _accessor = httpContextAccessor;
            _serializer = serializer;
        }

        public CorrelationContext GetCorrelationContext()
        {
            if (_accessor.HttpContext is null)
            {
                return null;
            }

            if (!_accessor.HttpContext.Request.Headers.TryGetValue("Correlation-Context", out var json))
            {
                return null;
            }

            var value = json.FirstOrDefault();

            return string.IsNullOrWhiteSpace(value) ? null : _serializer.Deserialize<CorrelationContext>(value);
        }

        public IHttpContextAccessor HttpContextAccessor => _accessor;
    }
}