using Convey.CQRS.Commands;
using Convey.WebApi;
using Convey.WebApi.CQRS.Builders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Menchul.Convey
{
    public class DispatcherEndpointsBuilderAdvanced : DispatcherEndpointsBuilder, IDispatcherEndpointsBuilderAdvanced
    {
        private readonly IEndpointsBuilder _builder;

        public DispatcherEndpointsBuilderAdvanced(IEndpointsBuilder builder) : base(builder)
        {
            _builder = builder;
        }

        public IDispatcherEndpointsBuilderAdvanced Post<TCommand, TResponse>(string path, Func<TCommand, HttpContext, Task> beforeDispatch = null,
            Func<ICommandInfo<TCommand, TResponse>, HttpContext, Task> afterDispatch = null, Action<IEndpointConventionBuilder> endpoint = null, bool auth = false, string roles = null,
            params string[] policies) where TCommand : class, ICommand
        {
            _builder.Post<TCommand>(path, (cmd, ctx) => BuildCommandContext(cmd, ctx, beforeDispatch, afterDispatch),
                endpoint, auth, roles, policies);

            return this;
        }

        private static async Task BuildCommandContext<TCommand, TResponse>(TCommand command, HttpContext context,
            Func<TCommand, HttpContext, Task> beforeDispatch = null,
            Func<ICommandInfo<TCommand, TResponse>, HttpContext, Task> afterDispatch = null)
            where TCommand : class, ICommand
        {
            if (beforeDispatch is {})
            {
                await beforeDispatch(command, context);
            }

            var dispatcher = context.RequestServices.GetRequiredService<ICommandDispatcher>();

            var commandInfo = new CommandInfo<TCommand, TResponse>(command);

            await dispatcher.SendAsync<ICommandInfo<TCommand, TResponse>>(commandInfo);

            if (afterDispatch is null)
            {
                context.Response.StatusCode = 200;

                await SendResponse();

                return;
            }

            await afterDispatch(commandInfo, context);

            await SendResponse();

            async Task SendResponse()
            {
                if (commandInfo.Result != null)
                {
                    await context.Response.WriteJsonAsync(commandInfo.Result);
                }
            }
        }
    }
}