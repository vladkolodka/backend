using Menchul.Core.ResourceProcessing.Entities;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Services.Interfaces
{
    public interface ILocalizationService
    {
        // Task LoadAndApplyAsync(IDocumentResource idsResource, IAppResource targetResource,
        //     List<string> languages = null);
        //
        // Task LoadAsync(IDocumentResource idsResource, List<string> languages = null);

        Task ExtractSaveOrUpdate(IAppResource resourceApp);
    }
}