using Menchul.Core.ResourceProcessing;
using Menchul.Core.ResourceProcessing.Behaviors;
using Menchul.Core.ResourceProcessing.Entities;
using Menchul.MCode.Core.ValueObjects;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.MCode.Infrastructure.Mongo.Repositories;
using Menchul.MCode.Infrastructure.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Services
{
    internal class LocalizationService : ILocalizationService
    {
        private readonly ILocalizationRepository _repository;
        private readonly IIdProviderFull _idProvider;

        public LocalizationService(ILocalizationRepository repository, IIdProviderFull idProvider)
        {
            _repository = repository;
            _idProvider = idProvider;
        }

        // TODO add logs
        public async Task ExtractSaveOrUpdate(IAppResource resourceApp)
        {
            var scannerBehavior = new ResourceScannerBehavior<LocalizedString>();

            var scannerProcessor = new ResourceBehaviorProcessor {scannerBehavior};
            resourceApp.Accept(scannerProcessor);

            var localizations = scannerBehavior.GetResources()
                .Select(s => new LocalizationDocument(s, _idProvider)).ToList();

            await _repository.SaveOrUpdateAllAsync(localizations);
        }
    }
}