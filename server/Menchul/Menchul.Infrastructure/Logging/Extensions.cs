using Convey;
using Convey.Logging.CQRS;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Menchul.Infrastructure.Logging
{
    public static class Extensions
    {
        public static IConveyBuilder AddCQRSLogging(this IConveyBuilder builder, Assembly assembly, IMessageToLogTemplateMapper mapper = null)
        {
            builder.Services.AddSingleton(mapper ?? new SimpleMessageToLogTemplateMapper());

            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }
}