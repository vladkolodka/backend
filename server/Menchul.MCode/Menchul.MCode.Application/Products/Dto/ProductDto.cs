using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.Companies.Dto;
using Menchul.MCode.Core.ValueObjects;
using System.Collections.Generic;

namespace Menchul.MCode.Application.Products.Dto
{
    public class ProductDto : PropertiesDto
    {
        public string Id { get; set; }

        public LocalizedString BrandName { get; set; }

        public CompanyDto BrandOwnerCompany { get; set; }

        public CompanyDto DistributorCompany { get; set; }

        public CompanyDto ManufacturerCompany { get; set; }

        public AddressDto ManufacturerAddressOfManufacturing { get; set; }
        public AddressDto BrandOwnerAddressOfManufacturing { get; set; }
        public long EAN { get; set; }

        public CodeCollection Codes { get; set; }

        public string DefaultLanguage { get; set; }

        public LocalizedString Name { get; set; }

        public LocalizedString Description { get; set; }

        public IList<Url> Urls { get; set; }

        public IList<string> Certificates { get; set; }

        public MetadataDocumentDto Metadata { get; set; }

        public ProductDto ParentProduct { get; set; }
    }
}