using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RequestsForRights.Web.Infrastructure.Helpers;
using RequestsForRights.Web.Models.ReportOptions;

namespace RequestsForRights.Web.Infrastructure.ValueProviders
{
    public class ReportResourceRightsOptionsValueProvider : 
        ReportOptionsValueProvider<ReportResourceRightsOptions>, IValueProvider
    {
        public new bool ContainsPrefix(string prefix)
        {
            try
            {
                var action = ValueProviderHelper.GetControllerActionByContext(HttpContext.Current);
                var parameter = action.GetParameters().FirstOrDefault(p => p.Name == prefix);
                return parameter != null && parameter.ParameterType == typeof(ReportResourceRightsOptions);
            }
            catch (AmbiguousMatchException)
            {
                return false;
            }
        }

        public new ValueProviderResult GetValue(string key)
        {
            var context = HttpContext.Current;
            var options = GetReportOptions();
            options.IdResource = ValueProviderHelper.GetValue<int?>("IdResource", context, null);
            options.Date = ValueProviderHelper.GetValue("Date", context, DateTime.Now.Date);
            return new ValueProviderResult(options,
                JsonConvert.SerializeObject(options),
                CultureInfo.InvariantCulture);
        }
    }
}