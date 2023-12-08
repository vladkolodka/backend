using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.Mongo.QueryRunners;

namespace Menchul.MCode.Infrastructure.Mongo.QueryRunners.Parameters
{
    public class ProductQueryParameters : IQueryParameters<ProductDocument>
    {
        public bool ExcludeMetadata { get; set; }
    }
}