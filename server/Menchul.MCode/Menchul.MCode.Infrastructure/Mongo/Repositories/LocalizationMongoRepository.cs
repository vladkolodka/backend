using Convey.Persistence.MongoDB;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Mongo.Repositories
{
    internal class LocalizationMongoRepository : ILocalizationRepository
    {
        private readonly IMongoRepository<LocalizationDocument, ObjectId> _repository;

        public LocalizationMongoRepository(IMongoRepository<LocalizationDocument, ObjectId> repository) =>
            _repository = repository;

        public async Task SaveOrUpdateAllAsync(List<LocalizationDocument> localizations)
        {
            var operations = localizations.Select(document =>
                new ReplaceOneModel<LocalizationDocument>(
                    new ExpressionFilterDefinition<LocalizationDocument>(d => d.Id == document.Id), document)
                {
                    IsUpsert = true
                }).ToList();

            if (operations.Any())
            {
                await _repository.Collection.BulkWriteAsync(operations);
            }
        }
    }
}