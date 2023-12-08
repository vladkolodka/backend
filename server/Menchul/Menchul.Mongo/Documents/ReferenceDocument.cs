using Convey.Types;
using System.Collections.Generic;

namespace Menchul.Mongo.Documents
{
    public class ReferenceDocument<TId> : IIdentifiable<TId>
    {
        public TId Id { get; set; }
        public bool Permanent { get; set; }
        public Dictionary<string, List<object>> Refs { get; set; } = new();
    }
}