using Menchul.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.Infrastructure.Kernel
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(IServiceProvider serviceProvider, ILogger<DomainEventDispatcher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task DispatchAsync(params IDomainEvent[] events)
        {
            if (events is null || !events.Any())
            {
                return;
            }

            _logger.LogInformation("Events will be handled: {Events}",
                events.Select(e => e.GetType().Name));

            using var scope = _serviceProvider.CreateScope();
            foreach (var @event in events)
            {
                var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(@event.GetType());
                var handlers = scope.ServiceProvider.GetServices(handlerType);

                var tasks = handlers.Select(x => (Task)handlerType
                    .GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync))
                    ?.Invoke(x, new[] {@event}));

                await Task.WhenAll(tasks);

                _logger.LogInformation("Events handled");
            }
        }
    }
}