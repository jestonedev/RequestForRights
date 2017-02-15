using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace RequestsForRights.Web.Infrastructure.Helpers
{
    public static class ValueProviderHelper
    {
        private static T To<T>(this string value, T defaultValue)
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

        public static MethodInfo GetControllerActionByContext(HttpContext context)
        {
            var request = context.Request;
            var routeValues = request.RequestContext.RouteData.Values;
            if (!routeValues.ContainsKey("controller") || !routeValues.ContainsKey("action")) return null;
            var controllerName = routeValues["controller"].ToString();
            var actionName = routeValues["action"].ToString();
            var currentAssemblyTypes = Assembly.GetAssembly(typeof(ValueProviderHelper))
                .GetTypes();
            var controller = currentAssemblyTypes.
                    FirstOrDefault(t => t.Name == controllerName + "Controller" && t.BaseType == typeof(Controller));
            return controller == null ? null : controller.GetMethod(actionName);
        }
    }
}