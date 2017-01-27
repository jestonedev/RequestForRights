using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RequestsForRights.Infrastructure.Enums;
using RequestsForRights.Infrastructure.Helpers;
using RequestsForRights.Models.FilterOptions;

namespace RequestsForRights.Infrastructure.ValueProviders
{
    public class RequestsFilterOptionsValueProvider : FilterOptionsValueProvider<RequestsFilterOptions>, 
        IValueProvider
    {
        public new ValueProviderResult GetValue(string key)
        {
            var context = HttpContext.Current;
            var filterOptions = GetFilterOptions();
            filterOptions.RequestCategory = ValueProviderHelper.GetValue("RequestCategory", context, RequestCategory.AllRequests);
            filterOptions.IdRequestStateType = ValueProviderHelper.GetValue<int?>("IdRequestStateType", context, null);
            filterOptions.IdRequestType = ValueProviderHelper.GetValue<int?>("IdRequestType", context, null);
            filterOptions.DateOfFillingFrom = ValueProviderHelper.GetValue<DateTime?>("DateOfFillingFrom", context, null);
            filterOptions.DateOfFillingTo = ValueProviderHelper.GetValue<DateTime?>("DateOfFillingTo", context, null);
            return new ValueProviderResult(filterOptions,
                JsonConvert.SerializeObject(filterOptions), 
                CultureInfo.InvariantCulture);
        }
    }
}