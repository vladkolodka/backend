using Convey;
using Convey.Logging;
using Convey.WebApi;
using Menchul.MCode.Api.Helpers;
using Menchul.MCode.Application;
using Menchul.MCode.Application.Common.Configuration;
using Menchul.MCode.Infrastructure;
using Menchul.MCode.Infrastructure.Common.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Menchul.MCode.Api
{
    public static class Program
    {
        private static IConfiguration _configuration;
        private static readonly QrGenerationOptions QROptions = new();

        public static async Task Main(string[] args)
        {
            DrawServiceName();

            await CreateHostBuilder(args)
                .Build()
                .RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(c => c.AddCommandLine(args)
                    .AddJsonFile("appsettings.secrets.json", optional: true))
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .ConfigureServices((context, services) =>
                    {
                        _configuration = context.Configuration;

                        using var serviceProvider = services.BuildServiceProvider();
                        var configuration = serviceProvider.GetService<IConfiguration>();

                        var qrOptionsSection = configuration!.GetSection(QrGenerationOptions.SectionName);
                        qrOptionsSection.Bind(QROptions);

                        services.Configure<QrGenerationOptions>(qrOptionsSection);

                        services.Configure<MenchulCertificateOptions>(
                            configuration!.GetSection(MenchulCertificateOptions.SectionName));

                        services.Configure<ForwardedHeadersOptions>(options =>
                        {
                            options.ForwardedHeaders =
                                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                        });

                        services.AddRouting(options =>
                            options.ConstraintMap.Add(Endpoints.Constraints.BoolIsProvided,
                                typeof(BoolIsProvidedConstraint)));

                        services
                            .AddCors()
                            .AddConvey()
                            .AddWebApi(builder => builder.ConfigureMvcOptions(),
                                JsonSettingsFactory.ConstructSerializer())
                            .AddApplication()
                            .AddInfrastructure()
                            .Build();
                    })
                    .Configure(app => app
                        .UseCors(b =>
                            b.WithOrigins(QROptions.BaseUrl).AllowAnyHeader().AllowAnyMethod().AllowCredentials())
                        .UseCors(b =>
                            b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())
                        .UseForwardedHeaders()
                        .UseInfrastructure()
                        .UseApplicationEndpoints(_configuration)
                    ))
                .UseLogging((hostBuilderContext, loggerConfiguration) =>
                {
                    var advancedFileOptions = hostBuilderContext.Configuration
                        .GetOptions<FileOptionsAdvanced>(FileOptionsAdvanced.ConfigPath);

                    if (advancedFileOptions.Enabled)
                    {
                        AddCustomFileLogging(loggerConfiguration, advancedFileOptions);
                    }
                });

        private static void AddCustomFileLogging(LoggerConfiguration loggerConfiguration,
            FileOptionsAdvanced fileOptions)
        {
            var path = string.IsNullOrWhiteSpace(fileOptions.Path) ? "logs/logs.txt" : fileOptions.Path;
            if (!Enum.TryParse<RollingInterval>(fileOptions.Interval, true, out var interval))
            {
                interval = RollingInterval.Day;
            }

            if (fileOptions.Structured)
            {
                loggerConfiguration.WriteTo.File(new CompactJsonFormatter(), path, rollingInterval: interval);
            }
            else
            {
                loggerConfiguration.WriteTo.File(path, rollingInterval: interval, outputTemplate: fileOptions.Format);
            }
        }

        private static void DrawServiceName()
        {
            const string logo = @"
███╗   ███╗███████╗███╗   ██╗ ██████╗██╗  ██╗██╗   ██╗██╗         ███╗   ███╗ ██████╗ ██████╗ ██████╗ ███████╗              888b    | 888~~  Y88b    / ~~~888~~~
████╗ ████║██╔════╝████╗  ██║██╔════╝██║  ██║██║   ██║██║         ████╗ ████║██╔════╝██╔═══██╗██╔══██╗██╔════╝    Y88b    / |Y88b   | 888___  Y88b  /     888
██╔████╔██║█████╗  ██╔██╗ ██║██║     ███████║██║   ██║██║         ██╔████╔██║██║     ██║   ██║██║  ██║█████╗       Y88b  /  | Y88b  | 888      Y88b/      888
██║╚██╔╝██║██╔══╝  ██║╚██╗██║██║     ██╔══██║██║   ██║██║         ██║╚██╔╝██║██║     ██║   ██║██║  ██║██╔══╝        Y88b/   |  Y88b | 888      /Y88b      888
██║ ╚═╝ ██║███████╗██║ ╚████║╚██████╗██║  ██║╚██████╔╝███████╗    ██║ ╚═╝ ██║╚██████╗╚██████╔╝██████╔╝███████╗       Y8/    |   Y88b| 888     /  Y88b     888
╚═╝     ╚═╝╚══════╝╚═╝  ╚═══╝ ╚═════╝╚═╝  ╚═╝ ╚═════╝ ╚══════╝    ╚═╝     ╚═╝ ╚═════╝ ╚═════╝ ╚═════╝ ╚══════╝        Y     |    Y888 888___ /    Y88b    888   ";
            Debug.WriteLine(logo);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(logo);
            Console.ResetColor();
        }
    }
}