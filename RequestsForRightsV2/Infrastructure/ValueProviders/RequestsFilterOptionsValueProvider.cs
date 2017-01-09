using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RequestsForRightsV2.Infrastructure.Enums;
using RequestsForRightsV2.Infrastructure.Helpers;
using RequestsForRightsV2.Models.FilterOptions;

namespace RequestsForRightsV2.Infrastructure.ValueProviders
{
    public class RequestsFilterOptionsValueProvider : FilterOptionsValueProvider<RequestsFilterOptions>
    {
        public new ValueProviderResult GetValue(string key)
        {
            var context = HttpContext.Current;
            var filterOptions = GetFilterOptions();
            filterOptions.RequestCategory = FilterOptionsHelper.GetValue("RequestCategory", context, RequestCategory.AllRequests);
            filterOptions.IdRequestState = FilterOptionsHelper.GetValue<int?>("IdRequestState", context, null);
            filterOptions.IdRequestType = FilterOptionsHelper.GetValue<int?>("IdRequestType", context, null);
            filterOptions.DateOfFillingFrom = FilterOptionsHelper.GetValue<DateTime?>("DateOfFillingFrom", context, null);
            filterOptions.DateOfFillingTo = FilterOptionsHelper.GetValue<DateTime?>("DateOfFillingTo", context, null);
            return new ValueProviderResult(filterOptions,
                JsonConvert.SerializeObject(filterOptions), 
                CultureInfo.InvariantCulture);
        }
    }
}