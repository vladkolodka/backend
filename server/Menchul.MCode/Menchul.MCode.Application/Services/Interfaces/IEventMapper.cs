using Convey.CQRS.Events;
using Menchul.Core.Events;
using System.Collections.Generic;

namespace Menchul.MCode.Application.Services.Interfaces
{
    public interface IEventMapper
    {
        IEnumerable<IEvent> Map(params IDomainEvent[] events);
    }
}