using System.Web.Mvc;
using RequestsForRightsV2.Models.FilterOptions;

namespace RequestsForRightsV2.Infrastructure.ValueProviders
{
    public class FilterOptionsValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            return new FilterOptionsValueProvider<FilterOptions>();
        }
    }
}