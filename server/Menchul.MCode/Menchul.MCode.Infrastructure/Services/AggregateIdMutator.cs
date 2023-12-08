using Menchul.Core.Entities;
using Menchul.Core.Services;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Core.Entities;
using System;

namespace Menchul.MCode.Infrastructure.Services
{
    public class AggregateIdMutator : AggregateIdModifier, IAggregateIdMutator
    {
        private readonly IIdProvider _idProvider;

        public AggregateIdMutator(IIdProvider idProvider)
        {
            _idProvider = idProvider;
        }

        public AggregateId<T> GenerateId<T>(AggregateId<T> aggregateId, bool force = false)
        {
            if (!force && !Equals(aggregateId.Value, default(T)))
            {
                return aggregateId;
            }

            switch (aggregateId)
            {
                case AggregateId<Guid> id:
                    GenerateId(id);
                    return aggregateId;

                case Id12BytesCrypto id:
                    GenerateId(id);
                    return aggregateId;

                case Id12Bytes id:
                    GenerateId(id);
                    return aggregateId;

                default:
                    throw new ApplicationException($"The ID for type {typeof(T).Name} can't be generated.");
            }
        }

        private void GenerateId(AggregateId<Guid> id) => SetId(id, _idProvider.NextGuid());

        private void GenerateId(Id12BytesCrypto id) => SetId(id, _idProvider.NextCrypto12ByteId());

        private void GenerateId(Id12Bytes id) => SetId(id, _idProvider.Next12ByteId());
    }
}