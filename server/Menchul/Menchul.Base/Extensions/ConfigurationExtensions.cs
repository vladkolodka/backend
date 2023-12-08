using Menchul.Base.Constants;
using Microsoft.Extensions.Configuration;

namespace Menchul.Base.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool DetailedErrors(this IConfiguration configuration) =>
            configuration.GetValue<bool>(ConfigurationFlags.DetailedAppInfo);

        public static bool ShowSwagger(this IConfiguration configuration) =>
            configuration.GetValue<bool>(ConfigurationFlags.ShowSwagger);
    }
}