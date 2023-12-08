using System.Threading.Tasks;

namespace Menchul.Core.Events
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(params IDomainEvent[] events);
    }
}