using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using RequestsForRights.Infrastructure.Helpers;
using RequestsForRights.Models.FilterOptions;

namespace RequestsForRights.Infrastructure.ValueProviders
{
    public class RequestsFilterOptionsValueProviderFactory: ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            try
            {
                var action = FilterOptionsHelper.GetControllerActionByContext(HttpContext.Current);
                return action.GetParameters().Any(p => p.ParameterType == typeof(RequestsFilterOptions)) ?
                    new RequestsFilterOptionsValueProvider() : null;
            }
            catch (AmbiguousMatchException)
            {
                return null;
            }
        }
    }
}