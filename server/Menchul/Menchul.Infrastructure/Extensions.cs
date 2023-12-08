using Menchul.Core.Events;
using Menchul.Infrastructure.Contexts;
using Menchul.Infrastructure.Kernel;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Menchul.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
            services.Scan(s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }

        public static IServiceCollection AddCommonInfrastructure(this IServiceCollection services)
        {
            return services
                .AddDomainEvents()
                .AddTransient<IHttpCorrelationContextAccessor, HttpCorrelationContextAccessor>()
                .AddTransient<IAppContextFactory, AppContextFactory>();
        }

        // public static IServiceCollection AddCommandLogging(this IServiceCollection services)
        // {
        //     services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        //     return services;
        // }
        //
        // public static IServiceCollection AddEventLogging(this IServiceCollection services)
        // {
        //     services.TryDecorate(typeof(IEventHandler<>), typeof(LoggingEventHandlerDecorator<>));
        //     return services;
        // }
    }
}