using Menchul.Resources.ReferenceBooks.Models;

namespace Menchul.Resources
{
    public class ResourceUtils
    {
        public static byte[] GetReferenceBooks<TModelType>() where TModelType : IReferenceBookModel
        {
            var key = ResourceKeys.ReferenceBooks[typeof(TModelType)];

            object obj = ReferenceBooksResource.ResourceManager.GetObject(key, ReferenceBooksResource.Culture);
            return ((byte[])(obj));
        }
    }
}