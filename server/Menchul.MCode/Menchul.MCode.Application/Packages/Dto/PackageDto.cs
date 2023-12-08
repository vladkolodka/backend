using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.ProductUnits.Dto;
using Menchul.MCode.Core.Enums;
using System;
using System.Collections.Generic;

namespace Menchul.MCode.Application.Packages.Dto
{
    public class PackageDto : PropertiesDto
    {
        public string Id { get; set; }
        public PackageState State { get; set; }
        public DateTime? DateOfPackaging { get; set; }

        public List<PackageDto> Packages { get; set; }

        // TODO convert to passports
        public List<ProductUnitDto> Products { get; set; }
    }
}