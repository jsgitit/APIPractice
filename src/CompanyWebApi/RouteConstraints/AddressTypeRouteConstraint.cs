using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace CompanyWebApi.RouteConstraints
{
    public class AddressTypeRouteConstraint : IRouteConstraint
    {
        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out var value) && value is string stringValue)
            {
                return Enum.TryParse(typeof(AddressType), stringValue, true, out _);
            }

            return false;
        }
    }
}
