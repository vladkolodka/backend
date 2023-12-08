using Menchul.Application;
using System;
using System.Security.Claims;

namespace Menchul.Infrastructure.Contexts
{
    public sealed class IdentityContext : IIdentityContext
    {
        public Guid Id { get; }
        public Guid ClientCompanyId { get; }
        public string Role { get; } = string.Empty;
        public bool IsAuthenticated { get; }
        public ClaimsPrincipal Principal { get; }

        public IdentityContext()
        {
        }

        public IdentityContext(CorrelationContext.UserContext context)
            : this(context.Id, context.ClientCompanyId, context.Role, context.IsAuthenticated, context.Principal)
        {
        }

        public IdentityContext(string id, string clientCompanyId, string role, bool isAuthenticated,
            ClaimsPrincipal principal)
        {
            Id = Guid.TryParse(clientCompanyId, out var userId) ? userId : Guid.Empty;
            ClientCompanyId = Guid.TryParse(clientCompanyId, out var clientCompanyParsedId)
                ? clientCompanyParsedId
                : Guid.Empty;
            Role = role ?? string.Empty;
            IsAuthenticated = isAuthenticated;
            Principal = principal;
        }

        internal static IIdentityContext Empty => new IdentityContext();
    }
}