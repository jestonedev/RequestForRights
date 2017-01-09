using System;
using System.Globalization;
using System.Web;
using RequestsForRightsV2.Infrastructure.Enums;

namespace RequestsForRightsV2.Infrastructure.Helpers
{
    public static class FilterOptionsHelper
    {
        public static int? GetIntValue(string parameterName, HttpContext context)
        {
            var request = context.Request;
            var routeValues = request.RequestContext.RouteData.Values;
            if (!routeValues.ContainsKey("controller") || !routeValues.ContainsKey("action")) return null;
            var controllerName = routeValues["controller"].ToString();
            var cookieName = controllerName + "." + parameterName;
            int result;
            if (int.TryParse(context.Request[parameterName], out result))
            {
                context.Response.SetCookie(new HttpCookie(cookieName, result.ToString()));
                return result;
            }
            if (int.TryParse(context.Request[cookieName], out result))
            {
                return result;
            }
            return null;
        }

        public static DateTime? GetDateTimeValue(string parameterName, HttpContext context)
        {
            var request = context.Request;
            var routeValues = request.RequestContext.RouteData.Values;
            if (!routeValues.ContainsKey("controller") || !routeValues.ContainsKey("action")) return null;
            var controllerName = routeValues["controller"].ToString();
            var cookieName = controllerName + "." + parameterName;
            DateTime result;
            if (DateTime.TryParse(context.Request[parameterName], out result))
            {
                context.Response.SetCookie(new HttpCookie(cookieName, result.ToString(CultureInfo.InvariantCulture)));
                return result;
            }
            if (DateTime.TryParse(context.Request[cookieName], out result))
            {
                return result;
            }
            return null;   
        }

        public static string GetStringValue(string parameterName, HttpContext context)
        {
            var request = context.Request;
            var routeValues = request.RequestContext.RouteData.Values;
            if (!routeValues.ContainsKey("controller") || !routeValues.ContainsKey("action")) return null;
            var controllerName = routeValues["controller"].ToString();
            var cookieName = controllerName + "." + parameterName;
            if (context.Request[parameterName] == null) return context.Request[cookieName];
            context.Response.SetCookie(new HttpCookie(cookieName, context.Request[parameterName]));
            return context.Request[parameterName];
        }

        public static RequestCategory GetRequestCategoryValue(string parameterName, HttpContext context)
        {
            var request = context.Request;
            var routeValues = request.RequestContext.RouteData.Values;
            if (!routeValues.ContainsKey("controller") || !routeValues.ContainsKey("action")) 
                return RequestCategory.AllRequests;
            var controllerName = routeValues["controller"].ToString();
            var cookieName = controllerName + "." + parameterName;
            RequestCategory result;
            if (Enum.TryParse(context.Request[parameterName], out result))
            {
                context.Response.SetCookie(new HttpCookie(cookieName, result.ToString()));
                return result;
            }
            return Enum.TryParse(context.Request[cookieName], out result) ? 
                result : RequestCategory.AllRequests;
        }

        public static SortDirection GetSortDirectionValue(string parameterName, HttpContext context)
        {
            var request = context.Request;
            var routeValues = request.RequestContext.RouteData.Values;
            if (!routeValues.ContainsKey("controller") || !routeValues.ContainsKey("action"))
                return SortDirection.Desc;
            var controllerName = routeValues["controller"].ToString();
            var cookieName = controllerName + "." + parameterName;
            SortDirection result;
            if (Enum.TryParse(context.Request[parameterName], out result))
            {
                context.Response.SetCookie(new HttpCookie(cookieName, result.ToString()));
                return result;
            }
            return Enum.TryParse(context.Request[cookieName], out result) ?
                result : SortDirection.Desc;
        }
    }
}