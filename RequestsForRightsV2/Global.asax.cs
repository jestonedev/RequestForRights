using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RequestsForRights.Web.Infrastructure.Logging;
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
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "ErrorMessagesResource";
            DefaultModelBinder.ResourceClassKey = "ErrorMessagesResource";
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var logger = new NLogLogger();
            logger.Error(exception);

            var urlHelper = new UrlHelper(Request.RequestContext);
            var routeValues = new RouteValueDictionary
            {
                {"message", "Непредвиденная ошибка на стороне сервреа"}
            };

            var destinationUrl = urlHelper.Action("ServerError", "Home", routeValues);
            if (string.IsNullOrEmpty(destinationUrl))
            {
                throw new InvalidOperationException("NoRoutesMatched");
            }
            Server.TransferRequest(destinationUrl, true);
            Server.ClearError();
        }
    }
}
