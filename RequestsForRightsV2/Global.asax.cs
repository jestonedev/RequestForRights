using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RequestsForRights.Web.Infrastructure.ValueProviders;
using RequestsForRights.Web.Models.FilterOptions;

namespace RequestsForRights.Web
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
            ValueProviderFactories.Factories.Add(new ReportUserRightsOptionsValueProviderFactory());
            ValueProviderFactories.Factories.Add(new ReportResourceRightsOptionsValueProviderFactory());
            ValueProviderFactories.Factories.Add(new ReportDepartmentRightsOptionsValueProviderFactory());
            ValueProviderFactories.Factories.Add(new ReportDepartmentAndResourceRightsOptionsValueProviderFactory());
            ValueProviderFactories.Factories.Add(new ReportUserRightsHistoryOptionsValueProviderFactory());
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "ErrorMessagesResource";
            DefaultModelBinder.ResourceClassKey = "ErrorMessagesResource";
        }
    }
}
