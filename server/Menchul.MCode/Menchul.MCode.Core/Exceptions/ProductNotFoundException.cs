namespace Menchul.MCode.Core.Exceptions
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(string productId) : base($"The product [{productId}] wasn't found.")
        {
        }

        public ProductNotFoundException() : base($"The product wasn't found.")
        {
        }
    }
}