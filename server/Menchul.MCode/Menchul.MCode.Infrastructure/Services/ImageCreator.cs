using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.Common.Enums;
using Menchul.MCode.Application.Common.Exceptions;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Infrastructure.Services.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Services
{
    public class ImageCreator : IImageCreator
    {
        private readonly IQRTools _qrTools;

        public ImageCreator(IQRTools qrTools)
        {
            _qrTools = qrTools;
        }

        public async Task<string> CreateImageBase64Async(string data, QRCodeOptionsDto options)
        {
            using Bitmap qr = await _qrTools.GenerateMQR(options, data);

            await using var stream = new MemoryStream();

            var imageFormat = ToImageFormat(options.ImageFormat);

            qr.Save(stream, imageFormat);
            byte[] imageBytes = stream.ToArray();

            var imageBase64 = Base64Image(imageFormat.ToString().ToLower(), imageBytes);

            Array.Clear(imageBytes, 0, imageBytes.Length);

            return imageBase64;
        }

        private ImageFormat ToImageFormat(QRImageFormat qrImageFormat)
        {
            var formatName = qrImageFormat.ToString();
            var raw = new ImageFormatConverter().ConvertFrom(formatName);

            if (raw is null)
            {
                throw new NotSupportedImageFormat(formatName);
            }

            return raw as ImageFormat;
        }

        private string Base64Image(string imageFormat, byte[] data) =>
            $"data:image/{imageFormat.ToLower()};base64," + Convert.ToBase64String(data);
    }
}