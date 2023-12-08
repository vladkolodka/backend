using System.Collections.Generic;
using System.Linq;

namespace Menchul.Mongo.Common
{
    /// <summary>
    /// Indicates whether the reference id points to an existing resource
    /// </summary>
    /// <typeparam name="T">ID type</typeparam>
    public class ReferenceMap<T> : Dictionary<T, bool>
    {
        private readonly bool? _isValid;

        public ReferenceMap()
        {
        }

        private ReferenceMap(bool isValid)
        {
            _isValid = isValid;
        }

        public ReferenceMap(IEnumerable<KeyValuePair<T, bool>> collection) : base(collection)
        {
        }

        public bool IsValid() => _isValid ?? Values.All(exists => exists);

        public HashSet<T> GetMissing() => this.Where(p => p.Value == false).Select(p => p.Key).ToHashSet();

        public HashSet<T> GetExisting() => this.Where(p => p.Value).Select(p => p.Key).ToHashSet();

        public static ReferenceMap<T> True => new(true);
        public static ReferenceMap<T> False => new(false);
    }
}