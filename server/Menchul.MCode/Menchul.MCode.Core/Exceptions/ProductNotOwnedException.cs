namespace Menchul.MCode.Core.Exceptions
{
    public class ProductNotOwnedException : NotOwnedException
    {
        public ProductNotOwnedException(string productId) : base($"The product [{productId}] is not owned.")
        {
        }
    }
}