using Menchul.Base.Constants;
using System;
using System.Security.Claims;

namespace Menchul.Base.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetCompanyId(this ClaimsPrincipal principal) =>
            principal.FindFirstValue(UserClaimTypes.CompanyId);

        public static Guid GetCompanyIdGuid(this ClaimsPrincipal principal) =>
            GetCompanyId(principal) is var companyId && string.IsNullOrEmpty(companyId)
                ? Guid.Empty
                : Guid.Parse(companyId);
    }
}