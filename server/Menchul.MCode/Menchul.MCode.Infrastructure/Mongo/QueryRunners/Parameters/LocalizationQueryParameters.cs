using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.Mongo.QueryRunners;
using System.Collections.Generic;

namespace Menchul.MCode.Infrastructure.Mongo.QueryRunners.Parameters
{
    internal class LocalizationQueryParameters : IQueryParameters<LocalizationDocument>
    {
        public List<string> Languages { get; set; }

        public LocalizationQueryParameters(List<string> languages)
        {
            Languages = languages;
        }

        public LocalizationQueryParameters()
        {
        }
    }
}