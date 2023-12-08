using Convey.CQRS.Commands;
using Menchul.MCode.Application.Common;
using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Core.ValueObjects;
using Newtonsoft.Json.Linq;
using System;

namespace Menchul.MCode.Application.ProductUnits.Commands
{
    public record GenerateMQRCommand : ICommand, IPropertiesContainer
    {
        public string ProductId { get; set; }

        //https://en.wikipedia.org/wiki/Shelf_life
        public DateTime? DateOfManufacture { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public CodeCollection Codes { get; set; }

        public QRCodeOptionsDto QRImageOptions { get; set; }

        public JObject Properties { get; set; }
    }
}