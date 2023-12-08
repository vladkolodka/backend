using Menchul.Core.Types;
using Menchul.MCode.Infrastructure.Mongo.AggregateExtensions;
using MongoDB.Bson;

namespace Menchul.MCode.Infrastructure.Types
{
    /// <summary>
    /// Warning! This class should be the only implementation of the <see cref="IBsonId"/>. Modify the
    /// <see cref="Crypto12BitIdExtensions.ToObjectId"/> to remove this restriction.
    /// </summary>
    public class BsonId : IBsonId
    {
        public ObjectId ObjectId { get; }

        public BsonId(ObjectId objectId)
        {
            ObjectId = objectId;
        }

        public byte[] ToByteArray() => ObjectId.ToByteArray();

        private bool Equals(BsonId other)
        {
            return ObjectId.Equals(other.ObjectId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((BsonId)obj);
        }

        public override int GetHashCode()
        {
            return ObjectId.GetHashCode();
        }

        public override string ToString() => ObjectId.ToString();
    }
}