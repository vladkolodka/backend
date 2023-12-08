using Menchul.Core.ResourceProcessing.Entities;
using Menchul.Core.Types;
using Menchul.MCode.Application.ProductUnits.Dto;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.ValueObjects;
using Menchul.MCode.Infrastructure.Mongo.AggregateExtensions;
using Menchul.Mongo.Common;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Menchul.MCode.Infrastructure.Mongo.Documents
{
    internal class ProductUnitDocument : IDocumentRoot<ObjectId>, IIdMutable<Id12Bytes, IBsonId>
    {
        public ObjectId Id { get; set; }

        public DateTime DateOfManufacture { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public CodeCollection Codes { get; set; }

        public DocumentRef<ProductDocument, ObjectId> Product { get; set; }

        public string Properties { get; set; }
        public int CodeVersion { get; set; }
        public int Version { get; set; }

        public IEnumerable<IResource> GetResources()
        {
            yield return Product;
        }

        public ProductUnitDocument(ProductUnit unit)
        {
            SetId(unit.Id);
            DateOfManufacture = unit.DateOfManufacture;
            ExpiryDate = unit.ExpiryDate;
            Codes = unit.Codes;
            Product = unit.Product.Id.ToObjectId();
            CodeVersion = unit.CodeVersion;
            Properties = unit.Properties;
            Version = unit.Version;
        }

        public ProductUnitDto ToDto() => new ()
        {
            Id = Id.ToString(),
            Codes = Codes,
            PropertiesRaw = Properties,
            ExpiryDate = ExpiryDate,
            DateOfManufacture = DateOfManufacture,
            Version = CodeVersion,
            // Product = Product?.Document?.ToDto()
        };

        public void SetId(Id12Bytes aggregate)
        {
            Id = aggregate.ToObjectId();
        }
    }
}