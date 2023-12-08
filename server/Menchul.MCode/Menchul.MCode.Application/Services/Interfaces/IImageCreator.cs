using Menchul.MCode.Application.Common.Dto;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Services.Interfaces
{
    public interface IImageCreator
    {
        Task<string> CreateImageBase64Async(string data, QRCodeOptionsDto options);
    }
}