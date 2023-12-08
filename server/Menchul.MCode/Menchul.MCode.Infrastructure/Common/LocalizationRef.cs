using Menchul.MCode.Core.ValueObjects;
using Menchul.MCode.Infrastructure.Mongo.Documents;
using Menchul.Mongo.Common;
using MongoDB.Bson;

namespace Menchul.MCode.Infrastructure.Common
{
    internal class LocalizationRef : DocumentRef<LocalizationDocument, ObjectId>
    {
        public static implicit operator LocalizedString(LocalizationRef @ref) => @ref?.Document?.ToValueObject();

        public static implicit operator LocalizationRef(LocalizedString s) =>
            s == null ? null : new() {RefId = new ObjectId(s.TranslationId)};
    }
}