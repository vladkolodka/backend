using Convey.HTTP;
using Menchul.Application;
using Menchul.Base.Extensions;
using Microsoft.AspNetCore.Http;
using OpenTracing;
using System;
using System.Linq;
using System.Security.Claims;

namespace Menchul.Infrastructure.Contexts
{
    internal sealed class AppContextFactory : IAppContextFactory
    {
        // private static readonly JsonSerializerOptions SerializerOptions = new()
        // {
        //     PropertyNameCaseInsensitive = true,
        //     PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //     Converters = {new JsonStringEnumConverter()}
        // };

        // private readonly ICorrelationContextAccessor _contextAccessor;
        // private readonly IJsonSerializer _serializer;
        private readonly IHttpCorrelationContextAccessor _httpCorrelationContextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICorrelationIdFactory _correlationIdFactory;
        private readonly ITracer _tracer;

        public AppContextFactory( /*IJsonSerializer serializer, */
            IHttpCorrelationContextAccessor httpCorrelationContextAccessor,
            IHttpContextAccessor httpContextAccessor, ICorrelationIdFactory correlationIdFactory, ITracer tracer
            /*, ICorrelationContextAccessor contextAccessor*/)
        {
            // _contextAccessor = contextAccessor;
            // _serializer = serializer;
            _httpCorrelationContextAccessor = httpCorrelationContextAccessor;
            _httpContextAccessor = httpContextAccessor;
            _correlationIdFactory = correlationIdFactory;
            _tracer = tracer;
        }

        public IAppContext Create()
        {
            // TODO uncomment to use with RabbitMQ
            // if (_contextAccessor.CorrelationContext is { })
            // {
            //     var payload = _serializer.Serialize(_contextAccessor.CorrelationContext/*, SerializerOptions*/);
            //
            //     return string.IsNullOrWhiteSpace(payload)
            //         ? AppContext.Empty
            //         : new AppContext(_serializer.Deserialize<CorrelationContext>(payload/*, SerializerOptions*/));
            // }

            var context = _httpCorrelationContextAccessor.GetCorrelationContext();
            // var context = _httpContextAccessor.GetCorrelationContext();

            return context is null ? AppContext.Empty : new AppContext(context);
        }

        // temporary solution, this code should be on the API Gateway side
        public IAppContext CreateLocal()
        {
            // TODO remove correlation context completely for now
            // TODO or change source (construct not from HTTP header) - better

            var correlationId = _correlationIdFactory.Create();
            var spanContext = _tracer.ActiveSpan is null ? string.Empty : _tracer.ActiveSpan.Context.ToString();

            var context = BuildContext(_httpCorrelationContextAccessor.HttpContextAccessor.HttpContext, correlationId,
                spanContext);

            return new AppContext(context) {CancellationToken = _httpContextAccessor.HttpContext!.RequestAborted};
        }

        private CorrelationContext BuildContext(HttpContext context, string correlationId, string spanContext,
            string name = null, string resourceId = null)
            => new()
            {
                CorrelationId = correlationId,
                Name = name ?? string.Empty,
                ResourceId = resourceId ?? string.Empty,
                SpanContext = spanContext ?? string.Empty,
                TraceId = context.TraceIdentifier,
                ConnectionId = context.Connection.Id,
                CreatedAt = DateTime.UtcNow,
                User = new CorrelationContext.UserContext
                {
                    // fake auth
                    // ClientCompanyId = "ff2f0392-fba2-43aa-85cd-24d8af7c1c30",
                    // IsAuthenticated = true,

                    Id = "", // TODO
                    ClientCompanyId = context.User.GetCompanyId(),
                    IsAuthenticated = context.User.Identity?.IsAuthenticated ?? false,
                    Role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                    Principal = context.User
                }
            };
    }
}