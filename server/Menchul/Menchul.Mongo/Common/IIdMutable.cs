using Menchul.Core.Entities;

namespace Menchul.Mongo.Common
{
    /// <summary>
    /// Allows to change the id of the document
    /// </summary>
    /// <typeparam name="TAggregateId">ID type</typeparam>
    /// <typeparam name="TAggregateValue">Aggregate ID type</typeparam>
    public interface IIdMutable<in TAggregateId, TAggregateValue> where TAggregateId : AggregateId<TAggregateValue>
    {
        void SetId(TAggregateId aggregate);
    }
}