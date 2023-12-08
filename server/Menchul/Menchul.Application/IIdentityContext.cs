using System;
using System.Security.Claims;

namespace Menchul.Application
{
    public interface IIdentityContext
    {
        Guid Id { get; }
        Guid ClientCompanyId { get; }
        string Role { get; }
        bool IsAuthenticated { get; }
        public ClaimsPrincipal Principal { get; }
    }
}