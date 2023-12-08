using Menchul.MCode.Infrastructure.Services.Interfaces;
using MongoDB.Bson;

namespace Menchul.MCode.Infrastructure.Services
{
    public class IdProviderFull : IdProvider, IIdProviderFull
    {
        public ObjectId NextObjectId() => ObjectId.GenerateNewId();
    }
}