using Menchul.MCode.Application.Common.Dto;
using Menchul.MCode.Application.Products.Dto;
using Menchul.MCode.Application.ProductUnits.Dto;
using Menchul.MCode.Core.ValueObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Menchul.MCode.Application.Services
{
    public class PassportBuilder
    {
        private readonly PassportDto _passport = new();
        private readonly JObject _properties = new();
        private readonly List<Url> _urls = new();
        private readonly List<string> _certificates = new();

        public PassportBuilder Append(ProductDto product, bool isParentProduct = false)
        {
            if (product == null)
            {
                return this;
            }

            #region recursion: before appending the parent product

            if (!isParentProduct)
            {
                // properties where the top-level product (a product that has no children) has the priority
                _passport.Id = product.Id ?? _passport.Id;
                _passport.EAN = product.EAN == default ? _passport.EAN : product.EAN;
                _passport.Name = product.Name ?? _passport.Name;
                _passport.Description = product.Description ?? _passport.Description;

                _passport.Metadata = product.Metadata ?? _passport.Metadata;

                // this duplication ensures that the next top-level product added to the builder will override this properties
                _passport.BrandName = product.BrandName ?? _passport.BrandName;
                _passport.BrandOwnerCompany = product.BrandOwnerCompany ?? _passport.BrandOwnerCompany;
                _passport.DistributorCompany = product.DistributorCompany ?? _passport.DistributorCompany;
                _passport.ManufacturerCompany = product.ManufacturerCompany ?? _passport.ManufacturerCompany;

                _passport.ManufacturerAddressOfManufacturing = product.ManufacturerAddressOfManufacturing ??
                                                               _passport.ManufacturerAddressOfManufacturing;
                _passport.BrandOwnerAddressOfManufacturing = product.BrandOwnerAddressOfManufacturing ??
                                                             _passport.BrandOwnerAddressOfManufacturing;

                _passport.DefaultLanguage = product.DefaultLanguage ?? _passport.DefaultLanguage;

                // end of duplicated properties section
            }
            else
            {
                // use values from the parent product if they wasn't provided by the child product
                _passport.BrandName ??= product.BrandName;
                _passport.BrandOwnerCompany ??= product.BrandOwnerCompany;
                _passport.DistributorCompany ??= product.DistributorCompany;
                _passport.ManufacturerCompany ??= product.ManufacturerCompany;
                _passport.ManufacturerAddressOfManufacturing ??= product.ManufacturerAddressOfManufacturing;
                _passport.BrandOwnerAddressOfManufacturing ??= product.BrandOwnerAddressOfManufacturing;
                _passport.DefaultLanguage ??= product.DefaultLanguage;
            }

            #endregion

            if (product.ParentProduct != null)
            {
                Append(product.ParentProduct, true);
            }

            #region recursion: after appending the parent product

            MergeUrls(product.Urls);
            MergeCertificates(product.Certificates);
            MergeCodes(product.Codes);
            MergeProperties(product.PropertiesRaw);

            #endregion

            return this;
        }

        private void MergeUrls(IList<Url> urls)
        {
            if (urls == null || !urls.Any())
            {
                return;
            }

            _urls.AddRange(urls);
        }

        private void MergeCertificates(IList<string> certificates)
        {
            if (certificates == null || !certificates.Any())
            {
                return;
            }

            _certificates.AddRange(certificates);
        }

        public PassportBuilder Append(ProductUnitDto unit)
        {
            if (unit == null)
            {
                return this;
            }

            _passport.Id = unit.Id;
            _passport.DateOfManufacture = unit.DateOfManufacture;
            _passport.ExpiryDate = unit.ExpiryDate;
            _passport.Version = unit.Version;

            MergeCodes(unit.Codes);
            MergeProperties(unit.PropertiesRaw);

            return this;
        }

        private void MergeCodes(CodeCollection codes)
        {
            if (_passport.Codes == null)
            {
                _passport.Codes = codes;
            }
            else if (codes != null)
            {
                foreach (var (codeKey, codeValue) in codes)
                {
                    _passport.Codes[codeKey] = codeValue;
                }
            }
        }

        private void MergeProperties(string properties)
        {
            if (!string.IsNullOrWhiteSpace(properties))
            {
                try
                {
                    var propertiesJson = JObject.Parse(properties);
                    _properties.Merge(propertiesJson);
                }
                catch (JsonReaderException)
                {
                }
            }
        }

        public PassportDto Build()
        {
            _passport.Properties = _properties;

            if (_urls.Any())
            {
                _passport.Urls = _urls.ToHashSet();
            }

            if (_certificates.Any())
            {
                _passport.Certificates = _certificates.ToHashSet();
            }

            return _passport;
        }
    }
}