using Menchul.Application.Exceptions;

namespace Menchul.MCode.Application.Common.Exceptions
{
    public class InvalidCodeSignatureException : AppException
    {
        public InvalidCodeSignatureException() : base("The code signature is invalid.")
        {
        }
    }
}