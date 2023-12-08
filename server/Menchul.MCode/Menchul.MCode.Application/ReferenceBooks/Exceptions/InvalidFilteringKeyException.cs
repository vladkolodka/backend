using Menchul.Application.Exceptions;

namespace Menchul.MCode.Application.ReferenceBooks.Exceptions
{
    public class InvalidFilteringKeyException : AppException
    {
        public InvalidFilteringKeyException(string key) : base($"The key [{key}] can't be used for filtering.")
        {
        }
    }
}