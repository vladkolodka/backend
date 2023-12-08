using Menchul.Core.Exceptions;

namespace Menchul.MCode.Core.Exceptions
{
    public class PackageContentsInvalidException : DomainException
    {
        public PackageContentsInvalidException() : base(
            "The package contains invalid references to either package or a product unit.")
        {
        }
    }
}