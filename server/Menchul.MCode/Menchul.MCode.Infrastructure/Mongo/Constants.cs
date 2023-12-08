using Menchul.Mongo.Common;

namespace Menchul.MCode.Infrastructure.Mongo
{
    internal static class Constants
    {
        internal static class Collections
        {
            public const string Localizations = "localizations";
            public const string ClientCompanies = "client-companies";
            public const string Companies = "companies";
            public const string Products = "products";
            public const string ProductUnits = "product-units";
            public const string Packages = "packages";
        }

        internal static class Properties
        {
            /// <summary>
            /// The name should match with <see cref="DocumentRef{TDocument,TId}.RefId"/>
            ///
            /// The string constant is used because currently MongoDB driver does not support strongly-typed index builder
            /// for nested documents of the collection.
            /// </summary>
            public const string RefId = "refId";
        }
    }
}