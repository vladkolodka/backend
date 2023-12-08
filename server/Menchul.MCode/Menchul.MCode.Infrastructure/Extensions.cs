using Convey;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Convey.HTTP;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.Swagger;
using FluentValidation.AspNetCore;
using Menchul.Certificate;
using Menchul.Core.Services;
using Menchul.Infrastructure;
using Menchul.Infrastructure.Contexts;
using Menchul.Infrastructure.Decorators;
using Menchul.Infrastructure.Logging;
using Menchul.MCode.Application.Companies.Commands.CreateClientCompany;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Infrastructure.Exceptions;
using Menchul.MCode.Infrastructure.Initializers;
using Menchul.MCode.Infrastructure.Logging;
using Menchul.MCode.Infrastructure.Mongo;
using Menchul.MCode.Infrastructure.Mongo.Repositories;
using Menchul.MCode.Infrastructure.Security;
using Menchul.MCode.Infrastructure.Services;
using Menchul.MCode.Infrastructure.Services.Interfaces;
using Menchul.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Menchul.MCode.Infrastructure
{
    public static class Extensions
    {
        public static void ConfigureMvcOptions(this IMvcCoreBuilder builder)
        {
            builder.AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<CreateClientCompanyCommandValidator>());
        }

        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            var appOptions = builder.Build().GetService<AppOptions>();
            var appName = appOptions?.Name;

            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ApplicationException("App name is not configured");
            }

            builder.Services
                .AddMemoryCache()
                .AddCommonInfrastructure()
                .AddSingleton<IClock, UtcClock>()
                .AddSingleton<ICertificateManager, CertificateManager>()
                .AddSingleton<ICertificateLocator, X509StoreCertificateLocator>()
                .AddSingleton<IAggregateIdMutator, AggregateIdMutator>()
                .AddSingleton<IAggregateIdParser, AggregateIdParser>()
                .AddSingleton<IIdProviderFull, IdProviderFull>()
                .AddSingleton<IIdProvider, IdProvider>()
                .AddSingleton<IUrlManager, UrlManager>()
                .AddSingleton<IQRTools, QRTools>()
                .AddSingleton<IImageCreator, ImageCreator>()
                .AddSingleton<IHashManager, HashManager>()
                .AddScoped<ILocalizationService, LocalizationService>()
                .AddSingleton<IRelationManager, RelationManager>()
                .AddRepositories()
                .AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().CreateLocal())
                .AddScoped<LogContextMiddleware>()
                .AddSingleton<CertificateInitializer>()
                .AddSingleton<ICorrelationIdFactory, CorrelationIdFactory>();
            // .AddScoped<IMessageBroker, MessageBroker>()

            builder.AddInitializer<CertificateInitializer>();

            builder
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddJwt()
                .AddApplicationSecurity(appName)
                .AddJaeger()
                .AddQueryHandlers()
                .AddCQRSLogging(typeof(CreateClientCompanyCommand).Assembly)
                .AddInMemoryQueryDispatcher()
                .AddMongoDb()
                .AddWebApiSwaggerDocs()
                .AddSecurity();

            // validation for commands and queries
            builder.Services.TryDecorate(typeof(IQueryHandler<,>), typeof(ValidationQueryHandlerDecorator<,>));
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));

            // TODO correlation
            // builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            // builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            // TODO auth
            app
                .UseMiddleware<LogContextMiddleware>()
                .UseAuthentication()
                // .UseMiddleware<RequestTypeMetricsMiddleware>()
                .UseErrorHandler()
                // .UseSwaggerDocs()
                // .UseJaeger()
                .UseConvey();
            // .UsePublicContracts<ContractAttribute>()
            // .UsePrometheus()
            // .UseCertificateAuthentication()
            // .UseRabbitMq()
            // .SubscribeEvent<UserCreated>()
            // .SubscribeEvent<UserLocked>()
            // .SubscribeEvent<UserUnlocked>()
            // .SubscribeCommand<SendStory>()
            // .SubscribeCommand<RateStory>();

            return app;
        }

        public static string GetAppName(this HttpContext httpContext)
        {
            var appOptions = httpContext.RequestServices.GetService<AppOptions>();
            return appOptions?.Name ?? appOptions?.Service ?? "MCode";
        }
    }
}