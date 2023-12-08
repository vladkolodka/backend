using System.Collections.Generic;

namespace Menchul.Resources.ReferenceBooks.Models
{
    public record CountryModel : IReferenceBookModel
    {
        public ushort Code { get; set; }
        public Dictionary<string, string> Name { get; set; }

        public string Alpha2 { get; set; }

        public string Alpha3 { get; set; }

        public string FlagUrl { get; set; }

        public string Currency { get; set; }

        public string PhoneCode { get; set; }

        public string TimeZone { get; set; }

        public ICollection<Subdivision> Subdivisions { get; set; }
    }
}