using Menchul.Core.Exceptions;

namespace Menchul.MCode.Core.Exceptions
{
    public class ParentProductOwnedByAnotherCompanyException : DomainException
    {
        public ParentProductOwnedByAnotherCompanyException() : base("The parent product is owned by another company.")
        {
        }
    }
}