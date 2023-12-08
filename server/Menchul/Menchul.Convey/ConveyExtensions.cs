using Convey.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Menchul.Convey
{
    public static class ConveyExtensions
    {
        /// <summary>
        /// Keep in sync with <see cref="Convey.WebApi.CQRS.Extensions.UseDispatcherEndpoints"/>
        /// </summary>
        /// <param name="app"></param>
        /// <param name="builder"></param>
        /// <param name="useAuthorization"></param>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDispatcherEndpoints(this IApplicationBuilder app,
            Action<IDispatcherEndpointsBuilderAdvanced> builder, bool useAuthorization = true,
            Action<IApplicationBuilder> middleware = null)
        {
            var definitions = app.ApplicationServices.GetService<WebApiEndpointDefinitions>();
            app.UseRouting();
            if (useAuthorization)
            {
                app.UseAuthorization();
            }

            middleware?.Invoke(app);

            EndpointRoutingApplicationBuilderExtensions.UseEndpoints(app, router =>
                builder(new DispatcherEndpointsBuilderAdvanced(
                    new EndpointsBuilder(router, definitions))));

            return app;
        }
    }
}