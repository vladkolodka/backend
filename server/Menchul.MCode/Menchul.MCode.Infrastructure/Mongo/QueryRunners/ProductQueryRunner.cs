using Convey.Persistence.MongoDB;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.MCode.Infrastructure.Mongo.QueryRunners.Parameters;
using Menchul.Mongo;
using Menchul.Mongo.QueryRunners;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Mongo.QueryRunners
{
    internal class ProductQueryRunner : IQueryRunner<ProductDocument>
    {
        private readonly IMongoRepository<ProductDocument, ObjectId> _repository;
        private readonly IRelationManager _relationManager;

        private static readonly string NestedMetadataProperty =
            $"{nameof(ProductDocumentWithHierarchy.Hierarchy).ToLower()}.{nameof(ProductDocument.Metadata).ToLower()}";

        public ProductQueryRunner(IMongoRepository<ProductDocument, ObjectId> repository,
            IRelationManager relationManager)
        {
            _repository = repository;
            _relationManager = relationManager;
        }

        public async Task<List<ProductDocument>> Run(AggregateFluent<ProductDocument> queryModifier = null,
            QueryParametersContainer parametersContainer = null)
        {
            var excludeMetadata =
                parametersContainer.GetSafe<ProductQueryParameters, ProductDocument>().ExcludeMetadata;

            var query = _repository.Collection.Aggregate()
                .ApplyModifier(queryModifier)
                .GraphLookup(
                    from: _repository.Collection,
                    connectFromField: d => d.ParentProduct.RefId,
                    connectToField: d => d.Id,
                    startWith: d => d.ParentProduct.RefId,
                    @as: (ProductDocumentWithHierarchy h) => h.Hierarchy);

            if (excludeMetadata)
            {
                query = query.Project<ProductDocumentWithHierarchy>(Builders<ProductDocumentWithHierarchy>.Projection
                    .Exclude(h => h.Metadata)
                    .Exclude(NestedMetadataProperty));
            }

            var productsHierarchies = await query.ToListAsync();

            var allProducts = productsHierarchies.FlattenHierarchies();

            if (productsHierarchies.Count != allProducts.Count)
            {
                _relationManager.Map<ProductDocument, ObjectId>(productsHierarchies, allProducts);
            }

            await _relationManager.MapLoad<CompanyDocument, Guid>(productsHierarchies);

            return productsHierarchies.Cast<ProductDocument>().ToList();
        }
    }
}