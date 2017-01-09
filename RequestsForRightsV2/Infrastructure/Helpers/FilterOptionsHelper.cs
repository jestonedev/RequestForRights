using System;
using System.Globalization;
using System.Web;
using RequestsForRightsV2.Infrastructure.Enums;

namespace RequestsForRightsV2.Infrastructure.Helpers
{
    public static class FilterOptionsHelper
    {
        public static T To<T>(this string value, T defaultValue)
        {
            try
            {
                var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                if (type.IsEnum)
                {
                    return (T)Enum.Parse(type, value);
                }
                return (T)Convert.ChangeType(value, type, CultureInfo.CurrentCulture);
            }
            catch (InvalidCastException)
            {
                return defaultValue;
            }
            catch (ArgumentException)
            {
                return defaultValue;
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public static T GetValue<T>(string parameterName, HttpContext context, T defaultValue)
        {
            var request = context.Request;
            var routeValues = request.RequestContext.RouteData.Values;
            if (!routeValues.ContainsKey("controller") || !routeValues.ContainsKey("action")) return defaultValue;
            var controllerName = routeValues["controller"].ToString();
            var cookieName = controllerName + "." + parameterName;
            if (context.Request[parameterName] != null)
            {
                var result = context.Request[parameterName].To(defaultValue);
                context.Response.SetCookie(new HttpCookie(cookieName, result.ToString()));
                return result;
            }
            return context.Request[cookieName] != null ?
                context.Request[cookieName].To(defaultValue) :
                defaultValue;
        }
    }
}