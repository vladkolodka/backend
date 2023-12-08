using System.Collections.Generic;

namespace Menchul.Resources.ReferenceBooks.Models
{
    public record Subdivision
    {
        public string Code { get; set; }
        public Dictionary<string, string> Name { get; set; }

        public Dictionary<string, string> Category { get; set; }

        public string FlagUrl { get; set; }

        public string ParentCode { get; set; }
    }
}