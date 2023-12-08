using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Menchul.MCode.Api.Helpers
{
    public class BoolIsProvidedConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out object value) && value is string s)
            {
                values[routeKey] = routeKey == s;
                return true;
            }

            return false;
        }
    }
}