using Convey;
using Menchul.MCode.Core.Repositories;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.Mongo;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Menchul.MCode.Infrastructure.Mongo.Repositories
{
    public static class Extensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection builder)
        {
            return builder
                .AddScoped<IClientCompanyRepository, ClientCompanyMongoRepository>()
                .AddScoped<ICompanyRepository, CompanyMongoRepository>()
                .AddScoped<IProductRepository, ProductMongoRepository>()
                .AddScoped<IProductUnitRepository, ProductUnitMongoRepository>()
                .AddScoped<IPackageRepository, PackageMongoRepository>()
                .AddScoped<ILocalizationRepository, LocalizationMongoRepository>();
        }

        public static IConveyBuilder AddMongoRepositories(this IConveyBuilder builder)
        {
            var collections = new MongoCollections();

            collections.Add<LocalizationDocument>(Constants.Collections.Localizations);
            collections.Add<ClientCompanyDocument>(Constants.Collections.ClientCompanies);
            collections.Add<CompanyDocument>(Constants.Collections.Companies);
            collections.Add<ProductDocument>(Constants.Collections.Products);
            collections.Add<ProductUnitDocument>(Constants.Collections.ProductUnits);
            collections.Add<PackageDocument>(Constants.Collections.Packages);

            var relationTrackers = new List<IRelationTracker>
            {
                // company
                new RelationTracker<CompanyDocument, Guid, LocalizationDocument, ObjectId>(collections),

                // product
                new RelationTracker<ProductDocument, ObjectId, CompanyDocument, Guid>(collections),
                new RelationTracker<ProductDocument, ObjectId, ProductDocument, ObjectId>(collections), // parent
                new RelationTracker<ProductDocument, ObjectId, LocalizationDocument, ObjectId>(collections),

                // product unit
                new RelationTracker<ProductUnitDocument, ObjectId, ProductDocument, ObjectId>(collections, true),

                // package
                new RelationTracker<PackageDocument, ObjectId, PackageDocument, ObjectId>(collections),
            };

            return builder
                .WithRepositoryContext(collections, relationTrackers)
                .AddMongoRepository<LocalizationDocument, ObjectId>()
                .AddMongoRepository<ClientCompanyDocument, Guid>()
                .AddMongoRepository<CompanyDocument, Guid>()
                .AddMongoRepository<ProductDocument, ObjectId>()
                .AddMongoRepository<ProductUnitDocument, ObjectId>()
                .AddMongoRepository<PackageDocument, ObjectId>();;
        }
    }
}