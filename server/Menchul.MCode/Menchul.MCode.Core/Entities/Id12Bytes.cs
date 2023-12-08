using Menchul.Core.Entities;
using Menchul.Core.Types;

namespace Menchul.MCode.Core.Entities
{
    public class Id12Bytes : AggregateId<IBsonId>
    {
        public const int StringLength = 24;
        public const int Size = 12;

        public Id12Bytes(IBsonId id) : base(id)
        {
        }

        public static implicit operator string(Id12Bytes id) => id.Value.ToString();
    }
}