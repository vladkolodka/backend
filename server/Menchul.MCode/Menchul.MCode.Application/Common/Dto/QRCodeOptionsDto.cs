using Menchul.MCode.Application.Common.Enums;
using Menchul.MCode.Core.Enums;

namespace Menchul.MCode.Application.Common.Dto
{
    public record QRCodeOptionsDto
    {
        public int Resolution { get; set; }

        public QRErrorCorrectionLevel ErrorCorrectionLevel { get; set; }

        public MCodeSize CodeSize { get; set; }

        public bool ToBlackAndWhite { get; set; }

        /// <summary>
        /// Jpeg/Png/Bmp/Gif/...
        /// </summary>
        public QRImageFormat ImageFormat { get; set; }
    }
}