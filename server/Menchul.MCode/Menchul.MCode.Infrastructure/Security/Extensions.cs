using Convey;
using Convey.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Menchul.MCode.Infrastructure.Security
{
    public static class Extensions
    {
        public static IConveyBuilder AddApplicationSecurity(this IConveyBuilder builder, string appName)
        {
            builder.Services.Configure<AuthorizationOptions>(options => new PolicyKeeper(appName).RegisterPolicies(options));

            builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformation>();

            return builder.AddJwt();
        }
    }
}