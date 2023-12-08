using Menchul.Core.Exceptions;

namespace Menchul.MCode.Core.Exceptions
{
    /// <summary>
    /// Used for 403 HTTP status code
    /// </summary>
    public class NotOwnedException: DomainException
    {
        public NotOwnedException(string message) : base(message)
        {
        }
    }
}