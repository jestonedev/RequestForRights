using System.Web;
using System.Web.Routing;
using WebGrease.Css.Extensions;

namespace RequestsForRights.Web.Infrastructure.Utilities.TransfertToRoute.Extensions
{
    public static class HttpRequestBaseExtensions
    {
        public static RouteValueDictionary GetRouteValueDictionary(this HttpRequestBase request)
        {
            var routedValues = new RouteValueDictionary();
            request.QueryString.AllKeys.ForEach(r => routedValues.AddWithCheck(r, request.QueryString[r]));
            request.Form.AllKeys.ForEach(r => routedValues.AddWithCheck(r, request.Form[r]));
            return routedValues;
        }
    }
}