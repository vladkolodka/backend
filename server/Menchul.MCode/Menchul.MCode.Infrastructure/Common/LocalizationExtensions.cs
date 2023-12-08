using Menchul.MCode.Application.Common.Queries;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.MCode.Infrastructure.Mongo.QueryRunners.Parameters;
using Menchul.Mongo;
using Menchul.Mongo.Resources;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Common
{
    internal static class LocalizationExtensions
    {
        public static Task<List<LocalizationDocument>> LoadLocalization(this IRelationManager relationManager,
            IEnumerable<IDocumentResource> parentResources, QueryBase query)
        {
            if (parentResources == null)
            {
                return null;
            }

            return relationManager.MapLoad<LocalizationDocument, ObjectId>(parentResources,
                new LocalizationQueryParameters(query.GetLanguages()).ToContainer());
        }

        public static async Task<List<LocalizationDocument>> LoadLocalization(this IRelationManager relationManager,
            IDocumentResource parentResource, QueryBase query)
        {
            if (parentResource == null)
            {
                return null;
            }

            return await
                relationManager.MapLoad<LocalizationDocument, ObjectId>(parentResource,
                    new LocalizationQueryParameters(query.GetLanguages()).ToContainer());
        }

        public static Task<List<LocalizationDocument>> LoadLocalization(this IRelationManager relationManager,
            IEnumerable<IDocumentResource> parentResources)
        {
            if (parentResources == null)
            {
                return null;
            }

            return relationManager.MapLoad<LocalizationDocument, ObjectId>(parentResources);
        }

        public static async Task<List<LocalizationDocument>> LoadLocalization(this IRelationManager relationManager,
            IDocumentResource parentResource)
        {
            if (parentResource == null)
            {
                return null;
            }

            return await relationManager.MapLoad<LocalizationDocument, ObjectId>(parentResource);
        }
    }
}