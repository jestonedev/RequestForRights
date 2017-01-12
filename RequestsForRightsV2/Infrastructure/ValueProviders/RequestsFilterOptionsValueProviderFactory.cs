using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using RequestsForRightsV2.Infrastructure.Helpers;
using RequestsForRightsV2.Models.FilterOptions;

namespace RequestsForRightsV2.Infrastructure.ValueProviders
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