using Menchul.Core.Entities;
using Menchul.MCode.Core.ValueObjects;
using System;

namespace Menchul.MCode.Core.Entities
{
    public class ProductUnit : AggregateRoot<Id12Bytes>
    {
        public const int CurrentVersion = 1;
        public DateTime DateOfManufacture { get; }

        public DateTime? ExpiryDate { get; }

        public CodeCollection Codes { get; }

        public Product Product { get; }

        public int CodeVersion { get; }
        public string Properties { get; }

        public ProductUnit(Id12Bytes id, DateTime? dateOfManufacture, DateTime? expiryDate, CodeCollection codes,
            Product product, string properties, int codeVersion, int version = CurrentVersion) : base(id, version)
        {

            DateOfManufacture = dateOfManufacture ?? DateTime.Now.ToLocalTime();
            ExpiryDate = expiryDate;
            Codes = codes;
            Product = product;
            CodeVersion = codeVersion;
            Properties = properties;
        }
    }
}