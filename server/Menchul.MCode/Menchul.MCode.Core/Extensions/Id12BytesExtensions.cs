using Menchul.Core.Types;
using Menchul.MCode.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Menchul.MCode.Core.Extensions
{
    public static class Id12BytesExtensions
    {
        public static HashSet<Id12Bytes> ToIdsHashSet(this IEnumerable<IBsonId> ids) => ids != null
            ? ids.Distinct().Select(i => new Id12Bytes(i)).ToHashSet()
            : new HashSet<Id12Bytes>();
    }
}