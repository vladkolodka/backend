using Menchul.Core.Entities;

namespace Menchul.Core.Services
{
    /// <summary>
    /// Update IDs for existing aggregate identifiers
    /// </summary>
    public interface IAggregateIdMutator
    {
        AggregateId<T> GenerateId<T>(AggregateId<T> aggregateId, bool force = false);
    }
}