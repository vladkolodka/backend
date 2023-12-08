using Menchul.Mongo.Common;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Menchul.Mongo.QueryRunners
{
    public static class Extensions
    {
        public static IAggregateFluent<TDocument> ApplyModifier<TDocument>(this IAggregateFluent<TDocument> fluent,
            AggregateFluent<TDocument> queryModifier) =>
            queryModifier == null ? fluent : queryModifier.Invoke(fluent);

        public static List<T> FlattenHierarchies<T>(this IEnumerable<IHierarchy<T>> hierarchies)
        {
            var hierarchiesList = hierarchies.ToList();
            return hierarchiesList.Cast<T>().Concat(hierarchiesList.SelectMany(g => g.Hierarchy)).ToList();
        }
    }
}