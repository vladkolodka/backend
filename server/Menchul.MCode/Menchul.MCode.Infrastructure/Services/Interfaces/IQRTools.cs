using Menchul.MCode.Application.Common.Dto;
using System.Drawing;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Services.Interfaces
{
    public interface IQRTools
    {
        Task<Bitmap> GenerateMQR(QRCodeOptionsDto qrCodeOptions, string content);
    }
}