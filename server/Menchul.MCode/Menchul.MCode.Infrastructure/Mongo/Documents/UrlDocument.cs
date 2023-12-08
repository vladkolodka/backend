using Menchul.MCode.Core.Enums;
using Menchul.MCode.Core.ValueObjects;

namespace Menchul.MCode.Infrastructure.Mongo.Documents
{
    internal class UrlDocument
    {
        public string Address { get; set; }
        public UrlType Type { get; set; }
        public string Language { get; set; }

        public UrlDocument(Url url)
        {
            Address = url.Address;
            Language = url.Language;
            Type = url.Type;
        }

        public Url ToValueObject()
        {
            return new(Address, Type, Language);
        }
    }
}