using Convey.Persistence.MongoDB;
using Menchul.Core.Entities;
using Menchul.Core.Services;
using Menchul.Mongo.Common;
using System.Threading.Tasks;

namespace Menchul.Mongo
{
    public interface IMenchulMongoRepository<TDocument, in TIdentifiable> : IMongoRepository<TDocument, TIdentifiable>
        where TDocument : IDocumentRoot<TIdentifiable>
    {
        Task TrackAsync(TDocument entity);

        Task DeleteTrackedAsync(TIdentifiable identifiable);

        Task<bool> TryAddAsync<TIdMutableDocument, TAggregate, TAggregateValue>(TIdMutableDocument entity,
            TAggregate aggregate, IAggregateIdMutator idMutator, int maxAttempts = 10)
            where TAggregate : AggregateId<TAggregateValue>
            where TIdMutableDocument : TDocument, IIdMutable<TAggregate, TAggregateValue>;
    }
}