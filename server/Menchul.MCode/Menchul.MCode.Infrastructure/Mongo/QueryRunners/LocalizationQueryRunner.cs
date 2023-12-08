using Convey.Persistence.MongoDB;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.MCode.Infrastructure.Mongo.QueryRunners.Parameters;
using Menchul.Mongo;
using Menchul.Mongo.QueryRunners;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Mongo.QueryRunners
{
    internal class LocalizationQueryRunner : IQueryRunner<LocalizationDocument>
    {
        private readonly IMongoRepository<LocalizationDocument, ObjectId> _repository;

        public LocalizationQueryRunner(IMongoRepository<LocalizationDocument, ObjectId> repository)
        {
            _repository = repository;
        }

        public Task<List<LocalizationDocument>> Run(AggregateFluent<LocalizationDocument> queryModifier = null,
            QueryParametersContainer parametersContainer = null)
        {
            var parameters = parametersContainer.GetSafe<LocalizationQueryParameters, LocalizationDocument>();

            var query = _repository.Collection.Aggregate()
                .ApplyModifier(queryModifier);

            if (parameters.Languages?.Any() == true)
            {
                query = query.AppendStage<LocalizationDocument>(GetLocalizationFilteringState(parameters.Languages));
            }

            return query.ToListAsync();
        }

        private BsonDocument GetLocalizationFilteringState(IEnumerable<string> includeLocalizations)
        {
            var translations = nameof(LocalizationDocument.Translations).ToLower();
            var language = nameof(LocalizationDocument.TranslationDocument.Language).ToLower();

            const string translation = "t";

            return new BsonDocument("$addFields",
                new BsonDocument(
                    new BsonElement("translations",
                        new BsonDocument(
                            new BsonElement("$filter",
                                new BsonDocument(new List<BsonElement>
                                {
                                    new("input", $"${translations}"),
                                    new("as", translation),
                                    new("cond",
                                        new BsonDocument(
                                            new BsonElement("$in",
                                                new BsonArray(new List<BsonValue>
                                                {
                                                    $"$${translation}.{language}",
                                                    new BsonArray(includeLocalizations)
                                                }))))
                                })
                            )
                        )
                    )
                )
            );
        }
    }
}