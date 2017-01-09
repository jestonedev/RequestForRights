using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RequestsForRightsV2.Infrastructure.Enums;
using RequestsForRightsV2.Infrastructure.Helpers;
using RequestsForRightsV2.Models.FilterOptions;

namespace RequestsForRightsV2.Infrastructure.ValueProviders
{
    public class FilterOptionsValueProvider<T>: IValueProvider
        where T: FilterOptions, new()
    {
        public bool ContainsPrefix(string prefix)
        {
            var request = HttpContext.Current.Request;
            var routeValues = request.RequestContext.RouteData.Values;
            if (!routeValues.ContainsKey("controller") || !routeValues.ContainsKey("action")) return false;
            var controllerName = routeValues["controller"].ToString();
            var actionName = routeValues["action"].ToString();
            var currentAssemblyTypes = Assembly.GetAssembly(GetType())
                .GetTypes();
            var controller = currentAssemblyTypes.
                FirstOrDefault(t => t.Name == controllerName + "Controller" && t.BaseType == typeof(Controller));
            if (controller == null) return false;
            try
            {
                var action = controller.GetMethod(actionName);
                var parameter = action.GetParameters().FirstOrDefault(p => p.Name == prefix);
                return parameter != null && parameter.ParameterType == typeof(T);
            }
            catch (AmbiguousMatchException)
            {
                return false;
            }
        }

        public ValueProviderResult GetValue(string key)
        {
            var filterOptions = GetFilterOptions();
            return new ValueProviderResult(filterOptions,
                JsonConvert.SerializeObject(filterOptions),
                CultureInfo.InvariantCulture);
        }

        protected T GetFilterOptions()
        {
            var context = HttpContext.Current;
            var filterOptions = new T
            {
                PageSize = FilterOptionsHelper.GetValue("PageSize", context, 25),
                PageIndex = FilterOptionsHelper.GetValue("PageIndex", context, 0),
                Filter = FilterOptionsHelper.GetValue<string>("Filter", context, null),
                SortField = FilterOptionsHelper.GetValue<string>("SortField", context, null),
                SortDirection = FilterOptionsHelper.GetValue("SortDirection", context, SortDirection.Asc)
            };
            return filterOptions;
        }
    }
}