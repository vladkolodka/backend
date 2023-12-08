using Menchul.MCode.Application.Common.Models;

namespace Menchul.MCode.Application.Services.Interfaces
{
    public interface IUrlManager
    {
        string BuildQrInfoUrl(byte[] data, QrUrlGenerationParameters parameters);
        byte[] DecodeQr(string qr);
    }
}