using System.Threading.Tasks;

namespace Menchul.Core.Events
{
    public interface IDomainEventHandler<in T> where T : class, IDomainEvent
    {
        Task HandleAsync(T domainEvent);
    }
}