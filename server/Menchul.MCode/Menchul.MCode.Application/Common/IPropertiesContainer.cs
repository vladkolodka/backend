using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Menchul.MCode.Application.Common
{
    public interface IPropertiesContainer
    {
        JObject Properties { get; set; }
        string GetProperties() => Properties?.ToString(Formatting.None);
    }
}