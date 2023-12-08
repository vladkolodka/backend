using Menchul.MCode.Application.Common;
using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.Products.Dto;
using Menchul.MCode.Core.ValueObjects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menchul.MCode.Application.Products.Commands
{
    public abstract record ProductCommandBase : IPropertiesContainer
    {
        public LocalizedString BrandName { get; set; }

        public Guid? BrandOwnerCompanyId { get; set; }

        public Guid? DistributorCompanyId { get; set; }

        public Guid? ManufacturerCompanyId { get; set; }

        public CodeCollection Codes { get; set; }

        public AddressDto ManufacturerAddressOfManufacturing { get; set; }

        public AddressDto BrandOwnerAddressOfManufacturing { get; set; }

        public string DefaultLanguage { get; set; }

        public LocalizedString Name { get; set; }

        public LocalizedString Description { get; set; }

        public IList<Url> Urls { get; set; }

        public IList<string> Certificates { get; set; }

        public JObject Properties { get; set; }

        public MetadataDocumentDto Metadata { get; set; }

        public List<Guid> GetCompanyIds() => new[] {BrandOwnerCompanyId, DistributorCompanyId, ManufacturerCompanyId}
            .Where(id => id.HasValue).Select(id => id.Value).Distinct().ToList();
    }
}