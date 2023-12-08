using Menchul.MCode.Core.ValueObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Menchul.MCode.Application.Products.Dto
{
    public class MetadataDocumentDto
    {
        public MetadataDocumentDto()
        {
        }

        public MetadataDocumentDto(MetadataDocument metadataDocument)
        {
            Version = metadataDocument.Version;
            Descriptors = JArray.Parse(metadataDocument.Descriptors);
        }

        public static MetadataDocumentDto NewOrNull(MetadataDocument document)
        {
            if (document == null)
            {
                return null;
            }

            return new MetadataDocumentDto(document);
        }

        public string Version { get; set; }

        public JArray Descriptors { get; set; }

        public MetadataDocument ToValueObject()
        {
            return new MetadataDocument(Version, Descriptors.ToString(Formatting.None));
        }
    }
}