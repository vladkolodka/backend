using Menchul.Application.Exceptions;

namespace Menchul.MCode.Application.Products.Exceptions
{
    public class EanAlreadyTakenException : AppException
    {
        public EanAlreadyTakenException(long ean) : base($"The specified EAN [{ean}] is already taken by another product.")
        {
        }
    }
}