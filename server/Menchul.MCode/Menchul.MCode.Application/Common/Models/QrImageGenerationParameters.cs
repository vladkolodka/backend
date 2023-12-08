using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.Common.Enums;
using Menchul.MCode.Core.Entities;

namespace Menchul.MCode.Application.Common.Models
{
    public class QrImageGenerationParameters : QrUrlGenerationParameters
    {
        public QRCodeOptionsDto Options { get; set; }

        public QrImageGenerationParameters()
        {
        }

        public QrImageGenerationParameters(ProductUnit unit, QRCodeOptionsDto codeOptions, QrCodeType type)
        {
            Id = unit.Id;
            DateTime = unit.DateOfManufacture;
            ParentId = unit.Product.Id;
            CodeType = type;
            Options = codeOptions;
            UrlPayloadFormat = unit.Product.EAN.ToString();
        }

        public QrImageGenerationParameters(Package package, QRCodeOptionsDto codeOptions, QrCodeType type, ClientCompany clientCompany)
        {
            Id = package.Id;
            DateTime = package.DateOfPackaging;
            ParentId = clientCompany.InternalId;
            CodeType = type;
            Options = codeOptions;
        }
    }
}