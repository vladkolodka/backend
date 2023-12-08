using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Menchul.Base.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder MapAppInfoEndpoint(this IApplicationBuilder appBuilder, string route = "",
            bool detailedErrors = false, bool showAssembly = false, Func<HttpContext, string> appNameProvider = null)
        {
            appBuilder.UseEndpoints(builder => builder.MapGet(route, async context =>
                    {
                        var result = new StringBuilder();

                        if (appNameProvider?.Invoke(context) is var appName && !string.IsNullOrEmpty(appName))
                        {
                            result.AppendLine(appName);
                        }

                        if (showAssembly)
                        {
                            if (result.Length != 0)
                            {
                                result.AppendLine();
                            }

                            var entryAssemblyName = Assembly.GetEntryAssembly()!.GetName();

                            result.AppendLine(entryAssemblyName.Name);
                            result.AppendLine(entryAssemblyName.Version?.ToString());
                        }

                        if (detailedErrors)
                        {
                            if (result.Length != 0)
                            {
                                result.AppendLine();
                            }

                            var loadedAssemblies = AppDomain.CurrentDomain.LoadAssemblies();

                            result.AppendLine("Assemblies: " + loadedAssemblies.Length + Environment.NewLine);

                            int nameMaxLength = loadedAssemblies.Max(x => x.Name.Length);
                            int versionMaxLength = loadedAssemblies.Max(x => x.Version.ToString().Length);

                            foreach (AssemblyName assembly in loadedAssemblies)
                            {
                                result.AppendLine(
                                    $"{assembly.Name?.PadRight(nameMaxLength)}   {assembly.Version?.ToString().PadRight(versionMaxLength)}   {assembly.CodeBase}");
                            }
                        }

                        await context.Response.WriteAsync(result.ToString());
                        await context.Response.CompleteAsync();
                    }
                )
            );

            return appBuilder;
        }
    }
}