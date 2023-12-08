using Menchul.MCode.Application.Companies.Dto;
using Menchul.MCode.Application.Products.Dto;
using Menchul.MCode.Core.ValueObjects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Menchul.MCode.Application.Common.Dto
{
    public class PassportDto
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

        public HashSet<Url> Urls { get; set; }

        public HashSet<string> Certificates { get; set; }

        public JObject Properties { get; set; }

        public DateTime DateOfManufacture { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int Version { get; set; }

        // TODO consider moving to other place
        public MetadataDocumentDto Metadata { get; set; }
    }
}