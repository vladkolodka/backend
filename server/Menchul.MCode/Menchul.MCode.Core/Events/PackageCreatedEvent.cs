using Menchul.Core.Events;
using Menchul.MCode.Core.Entities;

namespace Menchul.MCode.Core.Events
{
    public class PackageCreatedEvent : IDomainEvent
    {
        public Package Package { get; }

        public PackageCreatedEvent(Package package)
        {
            Package = package;
        }
    }
}