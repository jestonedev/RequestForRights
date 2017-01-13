using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RequestsForRights.Infrastructure.ValueProviders;
using RequestsForRights.Models.FilterOptions;

namespace RequestsForRights
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ValueProviderFactories.Factories.Add(new FilterOptionsValueProviderFactory<FilterOptions>());
            ValueProviderFactories.Factories.Add(new RequestsFilterOptionsValueProviderFactory());
        }
    }
}
