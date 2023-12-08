using Convey;
using Convey.Persistence.MongoDB;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.MCode.Infrastructure.Mongo.QueryRunners;
using Menchul.MCode.Infrastructure.Mongo.Repositories;
using Menchul.Mongo;
using Menchul.Mongo.Serialization;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace Menchul.MCode.Infrastructure.Mongo
{
    internal static class Extensions
    {
        public static IConveyBuilder AddMongoDb(this IConveyBuilder builder)
        {
            BsonSerializer.RegisterSerializationProvider(new EnumAsStringSerializationProvider());

            builder.Services
                .AddTransient<MongoInitializer>()
                .AddQueryRunnersImplementations()
                .AddQueryRunnerResolver<CompanyDocument, CompanyQueryRunner>()
                .AddQueryRunnerResolver<ProductDocument, ProductQueryRunner>()
                .AddQueryRunnerResolver<ProductUnitDocument, ProductUnitQueryRunner>()
                .AddQueryRunnerResolver<PackageDocument, PackageQueryRunner>()
                .AddQueryRunnerResolver<LocalizationDocument, LocalizationQueryRunner>();

            builder.AddInitializer<MongoInitializer>();

            return builder
                .AddMongo(seederType: typeof(MongoDbSeeder))
                .AddMongoRepositories();
        }

        private static IServiceCollection AddQueryRunnersImplementations(this IServiceCollection services) => services
            .AddSingleton<LocalizationQueryRunner>()
            .AddSingleton<CompanyQueryRunner>()
            .AddSingleton<ProductQueryRunner>()
            .AddSingleton<ProductUnitQueryRunner>()
            .AddSingleton<PackageQueryRunner>();
    }
}