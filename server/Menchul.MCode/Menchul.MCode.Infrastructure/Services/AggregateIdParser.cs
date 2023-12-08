using Menchul.Core.Types;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Infrastructure.Types;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace Menchul.MCode.Infrastructure.Services
{
    public class AggregateIdParser : IAggregateIdParser
    {
        public Id12Bytes Id12BytesFromString(string id) => new(new BsonId(new ObjectId(id)));

        public IEnumerable<IBsonId> BsonIdsFromStrings(IEnumerable<string> ids) =>
            ids.Select(id => new BsonId(new ObjectId(id)));

        public Id12Bytes BsonIdFromBytes(byte[] id) => new(new BsonId(new ObjectId(id)));
    }
}