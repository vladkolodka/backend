using System;

namespace Menchul.Core.Entities
{
    public class IdGuid : AggregateId<Guid>
    {
        public IdGuid() : this(Guid.NewGuid())
        {
        }

        public IdGuid(Guid value) : base(value)
        {
        }

        public bool IsEmpty => Value == Guid.Empty;

        public static implicit operator Guid(IdGuid guidId) => guidId.Value;

        public static implicit operator IdGuid(Guid id) => new(id);
    }
}