using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using RequestsForRights.Infrastructure.Helpers;
using RequestsForRights.Models.FilterOptions;

namespace RequestsForRights.Infrastructure.ValueProviders
{
    public class FilterOptionsValueProviderFactory<T> : ValueProviderFactory
        where T : FilterOptions, new()
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            try
            {
                var action = FilterOptionsHelper.GetControllerActionByContext(HttpContext.Current);
                return action.GetParameters().Any(p => p.ParameterType == typeof(T)) ? 
                    new FilterOptionsValueProvider<FilterOptions>() : null;
            }
            catch (AmbiguousMatchException)
            {
                return null;
            }
        }
    }
}