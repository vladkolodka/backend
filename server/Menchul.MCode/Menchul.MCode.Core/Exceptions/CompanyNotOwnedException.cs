using System;

namespace Menchul.MCode.Core.Exceptions
{
    public class CompanyNotOwnedException : NotOwnedException
    {
        public CompanyNotOwnedException(Guid companyId) : base($"The company [{companyId}] is not owned.")
        {
        }
    }
}