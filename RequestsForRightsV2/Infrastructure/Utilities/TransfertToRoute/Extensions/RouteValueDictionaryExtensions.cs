using System.Web.Routing;

namespace RequestsForRights.Web.Infrastructure.Utilities.TransfertToRoute.Extensions
{
    public static class RouteValueDictionaryExtensions
    {
        public static void AddWithCheck(this RouteValueDictionary dictionary, string key, object value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }
    }
}