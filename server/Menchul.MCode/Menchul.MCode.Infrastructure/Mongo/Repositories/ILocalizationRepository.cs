using Menchul.MCode.Infrastructure.Mongo.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Mongo.Repositories
{
    internal interface ILocalizationRepository
    {
        Task SaveOrUpdateAllAsync(List<LocalizationDocument> localizations);
    }
}