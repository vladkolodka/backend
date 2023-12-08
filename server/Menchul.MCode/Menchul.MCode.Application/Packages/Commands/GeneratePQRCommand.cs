using Convey.CQRS.Commands;
using Menchul.MCode.Application.Common;
using Menchul.MCode.Application.Common.Dto;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Menchul.MCode.Application.Packages.Commands
{
    public class GeneratePQRCommand : ICommand, IPropertiesContainer
    {
        public DateTime? DateOfPackaging { get; set; }
        public IEnumerable<string> Packages { get; set; }
        public IEnumerable<string> Products { get; set; }
        public JObject Properties { get; set; }
        public QRCodeOptionsDto QRImageOptions { get; set; }
    }
}