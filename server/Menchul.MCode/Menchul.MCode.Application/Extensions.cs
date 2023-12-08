using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Menchul.Core.Entities;
using Menchul.MCode.Application.Services;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.MCode.Core.Entities;
using Menchul.MCode.Core.Policies;
using Microsoft.Extensions.DependencyInjection;

namespace Menchul.MCode.Application
{
    public static class Extensions
    {
        public static IConveyBuilder AddApplication(this IConveyBuilder builder)
        {
            builder.Services
                .AddSingleton<IClientCompanyOwnerPolicy, ClientCompanyOwnerPolicy>()
                .AddSingleton<IEventMapper, EventMapper>()
                .AddSingleton<ICodeGenerator, CodeGenerator>()
                .AddSingleton<IReferenceBookCache, ReferenceBookCache>()
                .AddTransient(provider => new Id12BytesCrypto(provider.GetService<IIdProvider>()!.NextCrypto12ByteId()))
                .AddTransient(provider => new Id12Bytes(provider.GetService<IIdProvider>()!.Next12ByteId()))
                .AddTransient(provider => new IdGuid(provider.GetService<IIdProvider>()!.NextGuid()));
            // TODO
            // .AddSingleton<IStoryTextFactory, StoryTextFactory>()

            return builder
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddInMemoryCommandDispatcher()
                .AddInMemoryEventDispatcher();
            ;
        }
    }
}