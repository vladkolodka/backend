using Menchul.Core.Exceptions;

namespace Menchul.MCode.Core.Exceptions
{
    /// <summary>
    /// Used for 404 HTTP status code
    /// </summary>
    public abstract class NotFoundException : DomainException
    {
        protected NotFoundException(string message) : base(message)
        {
        }
    }
}