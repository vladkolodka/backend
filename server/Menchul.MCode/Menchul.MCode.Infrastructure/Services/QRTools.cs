using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Core.Enums;
using Menchul.MCode.Infrastructure.Services.Interfaces;
using Menchul.Resources;
using Microsoft.Extensions.Caching.Memory;
using Svg;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;

namespace Menchul.MCode.Infrastructure.Services
{
    public class QRTools : IQRTools
    {
        private const string BarcodeWriter = "bw_";
        private const string Svg = "svg_";

        private readonly IMemoryCache _cache;

        public QRTools(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<Bitmap> GenerateMQR(QRCodeOptionsDto qrCodeOptions, string content)
        {
            var resolution = CalculateResolution(qrCodeOptions);
            var margin = CalculateMargin(qrCodeOptions, resolution);

            BarcodeWriter barcodeWriter = GetBarcodeWriter(qrCodeOptions.Resolution, qrCodeOptions.ErrorCorrectionLevel);
            using Bitmap bitmap = barcodeWriter.Write(content);
            IntPtr qrBitmap = bitmap.GetHbitmap();
            Bitmap qr = Image.FromHbitmap(qrBitmap);

            using Bitmap bmpMenchul = await GetMCodeImage(resolution);
            using Graphics graphics = Graphics.FromImage(qr);
            var point = new Point(margin, margin);
            graphics.DrawImage(bmpMenchul, point);

            if (qrCodeOptions.ToBlackAndWhite)
            {
                BitmapToBlackAndWhite(qr);
            }

            return qr;
        }

        private BarcodeWriter GetBarcodeWriter(int qrResolution, QRErrorCorrectionLevel qrErrorCorrectionLevel)
        {
            string barcodeWriterKey = BarcodeWriter + qrResolution + qrErrorCorrectionLevel;
            BarcodeWriter barcodeWriter;

            if (_cache.TryGetValue(barcodeWriterKey, out object barcodeWriterKeyOut))
            {
                barcodeWriter = (BarcodeWriter)barcodeWriterKeyOut;
            }
            else
            {
                ErrorCorrectionLevel errorCorrectionLevel;

                switch (qrErrorCorrectionLevel)
                {
                    case QRErrorCorrectionLevel.L:
                        errorCorrectionLevel = ErrorCorrectionLevel.L;
                        break;
                    case QRErrorCorrectionLevel.M:
                        errorCorrectionLevel = ErrorCorrectionLevel.M;
                        break;
                    case QRErrorCorrectionLevel.Q:
                        errorCorrectionLevel = ErrorCorrectionLevel.Q;
                        break;
                    case QRErrorCorrectionLevel.H:
                        errorCorrectionLevel = ErrorCorrectionLevel.H;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(qrErrorCorrectionLevel), qrErrorCorrectionLevel, null);
                }

                barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Height = qrResolution,
                        Width = qrResolution,
                        Margin = 0,
                        PureBarcode = true,
                        GS1Format = false,
                        Hints =
                        {
                            {EncodeHintType.CHARACTER_SET, "UTF-8"},
                            {EncodeHintType.ERROR_CORRECTION, errorCorrectionLevel}
                        }
                    }
                };

                _cache.Set(barcodeWriterKey, barcodeWriter);
            }

            return barcodeWriter;
        }

        private async Task<Bitmap> GetMCodeImage(int resolution)
        {
            string svgKey = Svg + resolution;
            SvgDocument svgDoc;

            if (_cache.TryGetValue(svgKey, out object svgDocKeyOut))
            {
                svgDoc = (SvgDocument)svgDocKeyOut;
            }
            else
            {
                await using Stream stream = new MemoryStream(ImagesResource.logo);
                svgDoc = SvgDocument.Open<SvgDocument>(stream, null);

                _cache.Set(svgKey, svgDoc);
            }

            Bitmap menchulBmp = svgDoc.Draw(resolution, resolution);

            return menchulBmp;
        }

        private int CalculateResolution(QRCodeOptionsDto qrCodeOptions)
        {
            decimal mCodeCoefficient = 3 - (byte) qrCodeOptions.CodeSize * .5m;
            return decimal.ToInt32(qrCodeOptions.Resolution / mCodeCoefficient);
        }

        private int CalculateMargin(QRCodeOptionsDto qrCodeOptions, int calculatedResolution) =>
            (qrCodeOptions.Resolution - calculatedResolution) / 2;

        private static void BitmapToBlackAndWhite(Bitmap bitmap)
        {
            for (int y = 0; y < bitmap.Width; y++)
            {
                for (int x = 0; x < bitmap.Height; x++)
                {
                    Color p = bitmap.GetPixel(x, y);
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;
                    int avg = (a + r + g + b) / 4;
                    avg = avg < 128 ? 0 : 255; // Converting gray pixels to either pure black or pure white
                    Color color = Color.FromArgb(avg, avg, avg, avg);
                    bitmap.SetPixel(x, y, color);
                }
            }
        }
    }
}