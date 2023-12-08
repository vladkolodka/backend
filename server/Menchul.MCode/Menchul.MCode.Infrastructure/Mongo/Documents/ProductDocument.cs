using Menchul.Core.ResourceProcessing.Entities;
using Menchul.Core.Types;
using Menchul.MCode.Application.Products.Dto;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.ValueObjects;
using Menchul.MCode.Infrastructure.Common;
using Menchul.MCode.Infrastructure.Mongo.AggregateExtensions;
using Menchul.Mongo.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menchul.MCode.Infrastructure.Mongo.Documents
{
    internal class ProductDocument : IDocumentRoot<ObjectId>, IIdMutable<Id12Bytes, IBsonId>
    {
        public ObjectId Id { get; set; }

        public LocalizationRef BrandName { get; set; }

        public DocumentRef<CompanyDocument, Guid> BrandOwnerCompany { get; set; }

        public DocumentRef<CompanyDocument, Guid> DistributorCompany { get; set; }

        public DocumentRef<CompanyDocument, Guid> ManufacturerCompany { get; set; }

        public AddressDocument ManufacturerAddressOfManufacturing { get; set; }

        public AddressDocument BrandOwnerAddressOfManufacturing { get; set; }

        [BsonElement(nameof(EAN))]
        public long EAN { get; set; }

        public CodeCollection Codes { get; set; }

        public string DefaultLanguage { get; set; }

        public LocalizationRef Name { get; set; }

        public LocalizationRef Description { get; set; }

        public IList<UrlDocument> Urls { get; set; }

        public IList<string> Certificates { get; set; }

        public string Properties { get; set; }

        public DocumentRef<ClientCompanyDocument, Guid> ClientCompany { get; set; }

        public DocumentRef<ProductDocument, ObjectId> ParentProduct { get; set; }

        public MetadataDocument Metadata { get; set; }

        public int Version { get; set; }

        public ProductDocument(ProductInformation product)
        {
            SetId(product.Id);
            BrandName = product.BrandName;
            BrandOwnerCompany = product.BrandOwnerCompanyId?.Value;
            DistributorCompany = product.DistributorCompanyId?.Value;
            ManufacturerCompany = product.ManufacturerCompanyId?.Value;
            ManufacturerAddressOfManufacturing = AddressDocument.NewOrNull(product.ManufacturerAddressOfManufacturing);
            BrandOwnerAddressOfManufacturing = AddressDocument.NewOrNull(product.BrandOwnerAddressOfManufacturing);
            EAN = product.EAN;
            Codes = product.Codes;
            DefaultLanguage = product.DefaultLanguage;
            Name = product.Name;
            Description = product.Description;
            Urls = product.Urls?.Select(u => new UrlDocument(u)).ToList();
            Certificates = product.Certificates;
            Properties = product.Properties;
            ClientCompany = product.ClientCompanyId.Value;
            ParentProduct = product.ParentProduct?.Id.ToObjectId();
            Version = product.Version;
            Metadata = product.MetadataDocument;
        }

        public virtual IEnumerable<IResource> GetResources()
        {
            yield return ClientCompany;
            yield return ParentProduct;
            yield return BrandName;
            yield return BrandOwnerCompany;
            yield return ManufacturerCompany;
            yield return DistributorCompany;
            yield return ManufacturerAddressOfManufacturing;
            yield return BrandOwnerAddressOfManufacturing;
            yield return Name;
            yield return Description;
        }

        public Product ToEntity() => new(Id.ToAggregate(), ClientCompany.RefId, EAN, Version);

        public ProductInformation ToEntityInformation()
        {
            var urls = Urls.Select(d => d.ToValueObject()).ToList();

            var entity = new ProductInformation(Id.ToAggregate(), Name, BrandName, BrandOwnerCompany?.RefId, EAN,
                DistributorCompany?.RefId,
                ManufacturerCompany?.RefId, Codes, ManufacturerAddressOfManufacturing?.ToValueObject(),
                BrandOwnerAddressOfManufacturing?.ToValueObject(), DefaultLanguage, Description, urls, Certificates,
                Properties, ClientCompany, Version);

            entity.SetMetadata(Metadata);

            return entity;
        }

        public ProductDto ToDto()
        {
            var urls = Urls?.Select(d => d.ToValueObject()).ToList();

            return new ProductDto
            {
                Id = Id.ToString(),
                BrandName = BrandName,
                BrandOwnerCompany = BrandOwnerCompany?.Document?.ToDto(),
                DistributorCompany = DistributorCompany?.Document?.ToDto(),
                ManufacturerCompany = ManufacturerCompany?.Document?.ToDto(),
                ManufacturerAddressOfManufacturing = ManufacturerAddressOfManufacturing?.ToDto(),
                BrandOwnerAddressOfManufacturing = BrandOwnerAddressOfManufacturing?.ToDto(),
                EAN = EAN,
                Codes = Codes,
                DefaultLanguage = DefaultLanguage,
                Name = Name,
                Description = Description,
                Urls = urls,
                Certificates = Certificates,
                PropertiesRaw = Properties,
                Metadata = MetadataDocumentDto.NewOrNull(Metadata),
                ParentProduct = ParentProduct?.Document?.ToDto()
            };
        }

        public ProductDocument()
        {
        }

        public void SetId(Id12Bytes aggregate)
        {
            Id = aggregate.ToObjectId();
        }
    }

    internal class ProductDocumentWithHierarchy : ProductDocument, IHierarchy<ProductDocument>
    {
        public List<ProductDocument> Hierarchy { get; set; }

        public override IEnumerable<IResource> GetResources() => base.GetResources().Concat(Hierarchy);
    }

}