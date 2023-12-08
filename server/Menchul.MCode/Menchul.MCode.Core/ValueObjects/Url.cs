using Menchul.MCode.Core.Enums;

namespace Menchul.MCode.Core.ValueObjects
{
    public record Url(string Address, UrlType Type, string Language)
    {
        public string Address { get; set; } = Address;
        public UrlType Type { get; set; } = Type;
        public string Language { get; set; } = Language;
    }
}