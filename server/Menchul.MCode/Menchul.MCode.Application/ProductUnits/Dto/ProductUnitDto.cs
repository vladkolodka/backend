using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Core.ValueObjects;
using System;

namespace Menchul.MCode.Application.ProductUnits.Dto
{
    public class ProductUnitDto : PropertiesDto
    {
        public string Id { get; set; }

        public DateTime DateOfManufacture { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public CodeCollection Codes { get; set; }

        public int Version { get; set; }
    }
}