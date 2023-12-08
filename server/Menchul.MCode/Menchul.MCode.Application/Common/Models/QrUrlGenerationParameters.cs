using Menchul.MCode.Application.Common.Enums;
using Menchul.MCode.Core.Entities;
using System;

namespace Menchul.MCode.Application.Common.Models
{
    public class QrUrlGenerationParameters
    {
        public Id12Bytes Id { get; set; }
        public DateTime DateTime { get; set; }
        public Id12Bytes ParentId { get; set; }
        public QrCodeType CodeType { get; set; }

        /// <summary>
        /// Additional URL section
        /// </summary>
        public string UrlPayloadFormat { get; set; }
        public object UrlPayloadContent { get; set; }

        public CodeId ToCodeId() => new()
        {
            Id = Id.Value.ToByteArray(),
            DateTime = DateTime,
            ParentId = ParentId.Value.ToByteArray()
        };
    }
}