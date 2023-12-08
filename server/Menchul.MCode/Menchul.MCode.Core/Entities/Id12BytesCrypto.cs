using Menchul.Core.Types;

namespace Menchul.MCode.Core.Entities
{
    public class Id12BytesCrypto: Id12Bytes
    {
        public Id12BytesCrypto(IBsonId id) : base(id)
        {
        }

        public static implicit operator string(Id12BytesCrypto id) => id.Value.ToString();
    }
}