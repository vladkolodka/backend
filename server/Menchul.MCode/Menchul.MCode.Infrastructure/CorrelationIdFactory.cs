using Convey.HTTP;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;

namespace Menchul.MCode.Infrastructure
{
    internal class CorrelationIdFactory : ICorrelationIdFactory
    {
        private const string Header = "x-correlation-id";
        private static readonly AsyncLocal<CorrelationIdHolder> Holder = new();

        // TODO uncomment for using with message broker
        // private readonly IMessagePropertiesAccessor _messagePropertiesAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        // private readonly string _header;

        public CorrelationIdFactory(/*IMessagePropertiesAccessor messagePropertiesAccessor,*/
            IHttpContextAccessor httpContextAccessor/*, HttpClientOptions httpClientOptions*/)
        {
            // _messagePropertiesAccessor = messagePropertiesAccessor;
            _httpContextAccessor = httpContextAccessor;

            // _header = httpClientOptions.CorrelationIdHeader;
        }

        private static string CorrelationId
        {
            get => Holder.Value?.Id;
            set
            {
                var holder = Holder.Value;
                if (holder is {})
                {
                    holder.Id = null;
                }

                if (value is {})
                {
                    Holder.Value = new CorrelationIdHolder {Id = value};
                }
            }
        }

        private class CorrelationIdHolder
        {
            public string Id;
        }

        public string Create()
        {
            if (!string.IsNullOrWhiteSpace(CorrelationId))
            {
                return CorrelationId;
            }

            // TODO uncomment for using with message broker
            // var correlationId = _messagePropertiesAccessor.MessageProperties?.CorrelationId;
            // if (!string.IsNullOrWhiteSpace(correlationId))
            // {
            //     CorrelationId = correlationId;
            //     return CorrelationId;
            // }

            if (string.IsNullOrWhiteSpace(Header) || _httpContextAccessor.HttpContext is null)
            {
                CorrelationId = CreateId();
                return CorrelationId;
            }

            if (!_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(Header, out var id))
            {
                CorrelationId = CreateId();
                return CorrelationId;
            }

            var correlationId = id.ToString();
            CorrelationId = string.IsNullOrWhiteSpace(correlationId) ? CreateId() : correlationId;

            return CorrelationId;
        }

        private static string CreateId() => Guid.NewGuid().ToString("N");
    }
}