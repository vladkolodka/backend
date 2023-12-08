using Menchul.Core.Types;
using Menchul.MCode.Core.Entities;
using System.Collections.Generic;

namespace Menchul.MCode.Application.Services.Interfaces
{
    public interface IAggregateIdParser
    {
        Id12Bytes Id12BytesFromString(string id);
        IEnumerable<IBsonId> BsonIdsFromStrings(IEnumerable<string> ids);
        Id12Bytes BsonIdFromBytes(byte[] id);
    }
}