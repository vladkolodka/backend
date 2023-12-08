using Convey.Types;
using Menchul.Base.Constants;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menchul.MCode.Infrastructure.Security
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        private const string __rolesSection = "roles";
        private const string __scopeDivider = " ";

        private readonly string _appName;

        public ClaimsTransformation(AppOptions appOptions)
        {
            _appName = appOptions.Name;
        }

        /// <inheritdoc />
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var newClaims = new List<Claim>();

            var scopeClaim = principal.FindFirst(UserClaimTypes.Scope);

            if (!string.IsNullOrWhiteSpace(scopeClaim?.Value))
            {
                newClaims.AddRange(scopeClaim.Value.Split(__scopeDivider)
                    .Select(claimValue => new Claim(UserClaimTypes.Scopes, claimValue)));
            }

            var realmRolesClaim = principal.FindFirst(UserClaimTypes.RealmAccess);

            if (!string.IsNullOrWhiteSpace(realmRolesClaim?.Value))
            {
                var roleDocument =
                    await JsonDocument.ParseAsync(new MemoryStream(Encoding.UTF8.GetBytes(realmRolesClaim.Value)));
                var roles = roleDocument.RootElement.GetProperty(__rolesSection).EnumerateArray()
                    .Select(e => new Claim(ClaimTypes.Role, e.GetString()!));

                newClaims.AddRange(roles);
            }

            var resourceRolesClaim = principal.FindFirst(UserClaimTypes.ResourceAccess);

            if (!string.IsNullOrWhiteSpace(resourceRolesClaim?.Value))
            {
                var roleDocument = await JsonDocument.ParseAsync(new MemoryStream(
                    Encoding.UTF8.GetBytes(resourceRolesClaim.Value)));

                var roles = roleDocument.RootElement.GetProperty(_appName).GetProperty(__rolesSection).EnumerateArray()
                    .Select(e => new Claim(ClaimTypes.Role, PolicyKeeper.ClientRoleName(_appName, e.GetString())));

                newClaims.AddRange(roles);
            }

            if (!newClaims.Any())
            {
                return principal;
            }

            var identity = new ClaimsIdentity(principal.Identity, newClaims);
            return new(identity);
        }
    }
}