using Menchul.Core.Entities;
using Menchul.Core.ResourceProcessing;
using Menchul.Core.ResourceProcessing.Entities;
using Menchul.MCode.Core.Exceptions;
using Menchul.MCode.Core.Extensions;
using Menchul.MCode.Core.ValueObjects;
using System;
using System.Collections.Generic;

namespace Menchul.MCode.Core.Entities
{
    public class Product : AggregateRoot<Id12Bytes>, IClientCompanyOwned
    {
        public IdGuid ClientCompanyId { get; }
        public long EAN { get; }
        public Product ParentProduct { get; protected set; }

        public Product(Id12Bytes id, IdGuid clientCompanyId,long ean, int version = 0) : base(id, version)
        {
            if (clientCompanyId.IsEmpty)
            {
                throw new ClientCompanyNotFoundException();
            }

            ClientCompanyId = clientCompanyId;
            EAN = ean;
        }

        IdGuid IClientCompanyOwned.GetCompanyId() => ClientCompanyId;
    }

    public class ProductInformation : Product, IAppResourceContainer
    {
        public LocalizedString BrandName { get; private set; }

        public IdGuid BrandOwnerCompanyId { get; private set; }

        public IdGuid DistributorCompanyId { get; private set; }

        public IdGuid ManufacturerCompanyId { get; private set; }

        public Address ManufacturerAddressOfManufacturing { get; private set; }

        public Address BrandOwnerAddressOfManufacturing { get; private set; }

        public CodeCollection Codes { get; private set; }

        public string DefaultLanguage { get; private set; }

        public LocalizedString Name { get; private set; }

        public LocalizedString Description { get; private set; }

        public IList<Url> Urls { get; private set; }

        public IList<string> Certificates { get; private set; }

        public MetadataDocument MetadataDocument { get; private set; }

        public string Properties { get; private set; }

        public ProductInformation(Id12Bytes id, LocalizedString name, LocalizedString brandName,
            IdGuid brandOwnerCompanyId, long ean, IdGuid distributorCompanyId,
            IdGuid manufacturerCompanyId, CodeCollection codes, Address manufacturerAddressOfManufacturing,
            Address brandOwnerAddressOfManufacturing, string defaultLanguage, LocalizedString description,
            IList<Url> urls, IList<string> certificates, string properties, Guid clientCompanyId, int version = 0) :
            base(id, clientCompanyId, ean, version)
        {
            ChangeBrandName(brandName);
            ChangeBrandOwnerCompany(brandOwnerCompanyId);
            ChangeDistributorCompany(distributorCompanyId);
            ChangeManufacturerCompany(manufacturerCompanyId);
            ReplaceCodes(codes);
            ChangeManufacturingAddressForManufacturer(manufacturerAddressOfManufacturing);
            ChangeManufacturingAddressForBrandOwner(brandOwnerAddressOfManufacturing);
            ChangeDefaultLanguage(defaultLanguage);
            Rename(name);
            ChangeDescription(description);
            ReplaceUrls(urls);
            ReplaceCertificates(certificates);
            ReplaceProperties(properties);
        }

        public void SetParentProduct(Product product)
        {
            if (!product.ClientCompanyId.Equals(ClientCompanyId))
            {
                throw new ParentProductOwnedByAnotherCompanyException();
            }

            ParentProduct = product;
        }

        public void Accept(IResourceVisitor visitor) => visitor.Visit(this);

        public IEnumerable<IResource> GetResources()
        {
            if (ParentProduct is ProductInformation parentProductInformation)
            {
                yield return parentProductInformation;
            }

            yield return BrandName;
            yield return ManufacturerAddressOfManufacturing;
            yield return BrandOwnerAddressOfManufacturing;
            yield return Name;
            yield return Description;
        }

        public void Rename(LocalizedString newName)
        {
            Name = newName ?? throw new InvalidProductDetailException(nameof(Name));
        }

        public void ChangeDescription(LocalizedString newDescription)
        {
            Description = newDescription;
        }

        public void ChangeDefaultLanguage(string newDefaultLanguage)
        {
            if (!string.IsNullOrEmpty(newDefaultLanguage) && !newDefaultLanguage.IsValidIsoLanguageName())
            {
                throw new InvalidProductDetailException(nameof(DefaultLanguage), newDefaultLanguage);
            }

            DefaultLanguage = newDefaultLanguage;
        }

        public void ChangeManufacturingAddressForManufacturer(Address newAddress)
        {
            ManufacturerAddressOfManufacturing = newAddress;
        }

        public void ChangeManufacturingAddressForBrandOwner(Address newAddress)
        {
            BrandOwnerAddressOfManufacturing = newAddress;
        }

        public void ChangeBrandName(LocalizedString newBrandName)
        {
            BrandName = newBrandName;
        }

        public void ChangeBrandOwnerCompany(IdGuid newBrandOwner)
        {
            BrandOwnerCompanyId = newBrandOwner;
        }

        public void ChangeDistributorCompany(IdGuid newDistributor)
        {
            DistributorCompanyId = newDistributor;
        }

        public void ChangeManufacturerCompany(IdGuid newManufacturer)
        {
            ManufacturerCompanyId = newManufacturer;
        }

        public void ReplaceCodes(CodeCollection newCodes)
        {
            Codes = newCodes;
        }

        public void ReplaceUrls(IList<Url> newUrls)
        {
            Urls = newUrls;
        }

        public void ReplaceCertificates(IList<string> newCertificates)
        {
            Certificates = newCertificates;
        }

        public void ReplaceProperties(string newProperties)
        {
            Properties = newProperties;
        }

        public void SetMetadata(MetadataDocument metadataDocument)
        {
            if (string.IsNullOrWhiteSpace(metadataDocument.Version))
            {
                throw new InvalidProductDetailException(
                    $"{nameof(MetadataDocument)}.{nameof(MetadataDocument.Version)}", metadataDocument.Version);
            }

            if (string.IsNullOrWhiteSpace(metadataDocument.Descriptors))
            {
                throw new InvalidProductDetailException(
                    $"{nameof(MetadataDocument)}.{nameof(MetadataDocument.Descriptors)}", metadataDocument.Descriptors);
            }

            MetadataDocument = metadataDocument;
        }
    }
}