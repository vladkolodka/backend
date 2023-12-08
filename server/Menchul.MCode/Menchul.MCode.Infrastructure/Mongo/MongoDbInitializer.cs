using Convey.Types;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Mongo
{
    internal class MongoInitializer : IInitializer
    {
        private readonly IMongoDatabase _database;



        public MongoInitializer(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task InitializeAsync()
        {
            await InitializeClientCompaniesAsync();
            await InitializeProductsAsync();
            await InitializePackagesAsync();
        }

        private async Task InitializeClientCompaniesAsync()
        {
            // company email index
            await CreateIndex<ClientCompanyDocument>(Constants.Collections.ClientCompanies,
                clientCompanyBuilder => clientCompanyBuilder.Ascending(c => c.Email));
        }

        private async Task InitializeProductsAsync()
        {
            // product EAN index
            await CreateIndex<ProductDocument>(Constants.Collections.Products,
                productDocumentBuilder => productDocumentBuilder.Ascending(c => c.EAN),
                new CreateIndexOptions {Unique = true});
        }

        private async Task InitializePackagesAsync()
        {
            // productUnits.refId index
            await CreateIndex<PackageDocument>(Constants.Collections.Packages, builder => builder.Ascending(
                $"{PackageDocument.ProductUnitsName}.{Constants.Properties.RefId}"));

            // packages.refId index
            await CreateIndex<PackageDocument>(Constants.Collections.Packages, builder => builder.Ascending(
                $"{PackageDocument.PackagesName}.{Constants.Properties.RefId}"));
        }

        private async Task CreateIndex<TDocument>(string collection,
            Func<IndexKeysDefinitionBuilder<TDocument>, IndexKeysDefinition<TDocument>> indexConfigurator,
            CreateIndexOptions options = null)
        {
            var indexBuilder = Builders<TDocument>.IndexKeys;
            var builtIndex = indexConfigurator(indexBuilder);

            await _database.GetCollection<TDocument>(collection).Indexes
                .CreateOneAsync(new CreateIndexModel<TDocument>(builtIndex, options));
        }
    }
}