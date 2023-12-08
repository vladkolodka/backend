using Menchul.Core.Exceptions;

namespace Menchul.MCode.Core.Exceptions
{
    public class InvalidCompanyUrlException : DomainException
    {
        public InvalidCompanyUrlException(string url) : base($"Provided address [{url}] is not a valid URL.")
        {
        }
    }
}