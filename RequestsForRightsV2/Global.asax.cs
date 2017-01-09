using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RequestsForRightsV2.Infrastructure.ValueProviders;

namespace RequestsForRightsV2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ValueProviderFactories.Factories.Add(new FilterOptionsValueProviderFactory());
            ValueProviderFactories.Factories.Add(new RequestsFilterOptionsValueProviderFactory());
        }
    }
}
