using Menchul.MCode.Core.Entities;
using System;

namespace Menchul.MCode.Core.Policies
{
    public interface IClientCompanyOwnerPolicy
    {
        /*
         * компания владеет:
         * - product
         * - product unit
         *
         *
         */

        public bool IsOwnedBy(IClientCompanyOwned owned, Guid companyId);
    }
}