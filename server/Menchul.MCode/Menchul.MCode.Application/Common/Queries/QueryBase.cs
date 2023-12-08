using System.Collections.Generic;
using System.Linq;

namespace Menchul.MCode.Application.Common.Queries
{
    public class QueryBase
    {
        public const string UiModeValue = "UI";

        public string Lang { get; set; }
        public bool UseUiMode { get; set; }

        public List<string> GetLanguages()
        {
            const string separator = ",";
            return string.IsNullOrWhiteSpace(Lang) ? null : Lang.Split(separator).ToList();
        }
    }
}