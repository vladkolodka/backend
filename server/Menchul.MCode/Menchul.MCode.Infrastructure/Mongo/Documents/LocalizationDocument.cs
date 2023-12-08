using Menchul.MCode.Core.ValueObjects;
using Menchul.MCode.Infrastructure.Services.Interfaces;
using Menchul.Mongo.Common;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace Menchul.MCode.Infrastructure.Mongo.Documents
{
    internal class LocalizationDocument : IDocumentRoot<ObjectId>
    {
        public ObjectId Id { get; set; }

        public List<TranslationDocument> Translations { get; set; }

        public class TranslationDocument
        {
            public string Language { get; set; }
            public string Text { get; set; }
        }

        public LocalizationDocument(LocalizedString localizedString, IIdProviderFull idProvider)
        {
            if (string.IsNullOrEmpty(localizedString.TranslationId))
            {
                Id = idProvider.NextObjectId();
                localizedString.TranslationId = Id.ToString();
            }
            else
            {
                Id = new ObjectId(localizedString.TranslationId);
            }

            Translations = localizedString.Translations.Select(pair => new TranslationDocument
            {
                Language = pair.Key.ToLower(),
                Text = pair.Value
            }).ToList();
        }

        public LocalizedString ToValueObject() => new(
            Translations.ToDictionary(t => t.Language, t => t.Text)
        ) {TranslationId = Id.ToString()};
    }
}