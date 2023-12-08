using System.Collections.Generic;

namespace Menchul.Resources.ReferenceBooks.Models
{
    public record CurrencyModel : IReferenceBookModel
    {
        public ushort Code { get; set; }
        public Dictionary<string, string> Name { get; set; }

        public string Alpha3 { get; set; }

        public byte Exponent { get; set; }

        public string Symbol { get; set; }
    }
}