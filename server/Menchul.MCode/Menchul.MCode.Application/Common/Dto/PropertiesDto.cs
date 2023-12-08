using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Menchul.MCode.Application.Common.Dto
{
    public class PropertiesDto
    {
        [JsonIgnore]
        public string PropertiesRaw { get; set; }

        // ReSharper disable once UnusedMember.Global
        public JObject Properties => string.IsNullOrWhiteSpace(PropertiesRaw) || !PropertiesRaw.StartsWith("{")
            ? null
            : JObject.Parse(PropertiesRaw);
    }
}