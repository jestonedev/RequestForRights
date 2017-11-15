using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RequestsForRights.Domain.Enums;
using RequestsForRights.Web.Infrastructure.Helpers;
using RequestsForRights.Web.Models.ReportOptions;

namespace RequestsForRights.Web.Infrastructure.ValueProviders
{
    public class ReportUserRightsHistoryOptionsValueProvider : 
        ReportOptionsValueProvider<ReportUserRightsHistoryOptions>, IValueProvider
    {
        public new bool ContainsPrefix(string prefix)
        {
            try
            {
                var action = ValueProviderHelper.GetControllerActionByContext(HttpContext.Current);
                var parameter = action.GetParameters().FirstOrDefault(p => p.Name == prefix);
                return parameter != null && parameter.ParameterType == typeof(ReportUserRightsHistoryOptions);
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
            options.Login = ValueProviderHelper.GetValue<string>("Login", context, null);
            options.Snp = ValueProviderHelper.GetValue<string>("Snp", context, null);
            options.Department = ValueProviderHelper.GetValue<string>("Department", context, null);
            options.Unit = ValueProviderHelper.GetValue<string>("Unit", context, null);
            options.DateFrom = ValueProviderHelper.GetValue("DateFrom", context, DateTime.Now.Date);
            options.DateTo = ValueProviderHelper.GetValue("DateTo", context, DateTime.Now.Date);
            options.UsersCategory = ValueProviderHelper.GetValue("UsersCategory", context, UsersCategory.ActiveUsers);
            return new ValueProviderResult(options,
                JsonConvert.SerializeObject(options),
                CultureInfo.InvariantCulture);
        }
    }
}