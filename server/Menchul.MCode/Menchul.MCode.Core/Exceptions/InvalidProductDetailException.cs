namespace Menchul.MCode.Core.Exceptions
{
    public class InvalidProductDetailException : InvalidDetailException
    {
        public InvalidProductDetailException(string detailName, object value = null)
            : base("product", detailName, value)
        {
        }
    }
}