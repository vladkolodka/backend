using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Mongo
{
    internal class MongoDbSeeder : IMongoDbSeeder
    {
        // TODO implement
        public Task SeedAsync(IMongoDatabase database) => Task.CompletedTask;
    }
}