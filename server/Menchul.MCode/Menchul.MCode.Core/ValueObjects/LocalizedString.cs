using Menchul.Core.ResourceProcessing;
using Menchul.Core.ResourceProcessing.Entities;
using System.Collections.Generic;

namespace Menchul.MCode.Core.ValueObjects
{
    public record LocalizedString : IAppResource
    {
        public string TranslationId { get; set; }
        public IDictionary<string, string> Translations { get; set; }
        public void Accept(IResourceVisitor visitor)
        {
            visitor.Visit(this);
        }

        public LocalizedString()
        {
        }

        public LocalizedString(IDictionary<string, string> translations)
        {
            Translations = translations;
        }
    }
}