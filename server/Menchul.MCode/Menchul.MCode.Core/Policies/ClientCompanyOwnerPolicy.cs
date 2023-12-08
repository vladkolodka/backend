using Menchul.MCode.Core.Entities;
using System;

namespace Menchul.MCode.Core.Policies
{
    public class ClientCompanyOwnerPolicy : IClientCompanyOwnerPolicy
    {
        public bool IsOwnedBy(IClientCompanyOwned owned, Guid companyId) => owned.GetCompanyId() == companyId;
    }
}