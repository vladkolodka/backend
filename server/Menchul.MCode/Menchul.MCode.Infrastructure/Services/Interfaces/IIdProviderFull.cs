using Menchul.MCode.Application.Services.Interfaces;
using MongoDB.Bson;

namespace Menchul.MCode.Infrastructure.Services.Interfaces
{
    public interface IIdProviderFull : IIdProvider
    {
        ObjectId NextObjectId();
    }
}