using System.Web.Mvc;

namespace RequestsForRightsV2.Infrastructure.ValueProviders
{
    public class RequestsFilterOptionsValueProviderFactory: ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            return new RequestsFilterOptionsValueProvider();
        }
    }
}