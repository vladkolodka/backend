using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.Common.Models;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Services.Interfaces
{
    public interface ICodeGenerator
    {
        string CreateCode(QrUrlGenerationParameters parameters);
        Task<QrResultDto> CreateCodeImage(QrImageGenerationParameters parameters);
    }
}