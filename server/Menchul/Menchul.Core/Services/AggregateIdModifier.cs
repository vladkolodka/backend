using Menchul.Core.Entities;

namespace Menchul.Core.Services
{
    public abstract class AggregateIdModifier
    {
        /// <summary>
        /// Used to call internal method of the aggregate to modify the id value
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="id">New id value</param>
        /// <typeparam name="T"></typeparam>
        protected void SetId<T>(AggregateId<T> aggregateId, T id) => aggregateId.Value = id;
    }
}