using Menchul.Core.Types;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Infrastructure.Types;
using MongoDB.Bson;
using System;

namespace Menchul.MCode.Infrastructure.Mongo.AggregateExtensions
{
    public static class Crypto12BitIdExtensions
    {
        public static ObjectId ToObjectId(this Id12Bytes id) =>
            (id.Value as BsonId)?.ObjectId ??
            throw new Exception($"Only [{nameof(BsonId)}] type can implement [{nameof(IBsonId)}] interface");

        public static Id12Bytes ToAggregate(this ObjectId id) => new(new BsonId(id));
    }
}