using Convey.CQRS.Commands;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Menchul.Convey
{
    public interface IDispatcherEndpointsBuilderAdvanced : IDispatcherEndpointsBuilder
    {
        IDispatcherEndpointsBuilderAdvanced Post<TCommand, TResponse>(string path,
            Func<TCommand, HttpContext, Task> beforeDispatch = null,
            Func<ICommandInfo<TCommand, TResponse>, HttpContext, Task> afterDispatch = null,
            Action<IEndpointConventionBuilder> endpoint = null, bool auth = false, string roles = null,
            params string[] policies) where TCommand : class, ICommand;

        // TODO Put
    }
}