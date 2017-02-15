using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RequestsForRights.Web.Infrastructure.Enums;
using RequestsForRights.Web.Infrastructure.Helpers;
using RequestsForRights.Web.Models.ReportOptions;

namespace RequestsForRights.Web.Infrastructure.ValueProviders
{
    public class ReportOptionsValueProvider<T> : IValueProvider
        where T: ReportOptions, new()
    {
        public bool ContainsPrefix(string prefix)
        {
            try
            {
                var action = ValueProviderHelper.GetControllerActionByContext(HttpContext.Current);
                var parameter = action.GetParameters().FirstOrDefault(p => p.Name == prefix);
                return parameter != null && parameter.ParameterType == typeof(T);
            }
            catch (AmbiguousMatchException)
            {
                return false;
            }
        }

        public ValueProviderResult GetValue(string key)
        {
            var reportOptions = GetReportOptions();
            return new ValueProviderResult(reportOptions,
                JsonConvert.SerializeObject(reportOptions),
                CultureInfo.InvariantCulture);
        }

        protected T GetReportOptions()
        {
            var context = HttpContext.Current;
            var reportOptions = new T
            {
                SortField = ValueProviderHelper.GetValue<string>("SortField", context, null),
                SortDirection = ValueProviderHelper.GetValue("SortDirection", context, SortDirection.Asc),
                ReportDisplayStyle =
                    ValueProviderHelper.GetValue("ReportDisplayStyle", context, ReportDisplayStyle.Cards)
            };
            return reportOptions;
        }
    }
}