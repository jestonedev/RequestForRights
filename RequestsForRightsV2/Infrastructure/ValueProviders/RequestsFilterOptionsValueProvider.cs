using System.Globalization;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
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
            filterOptions.RequestCategory = FilterOptionsHelper.GetRequestCategoryValue("RequestCategory", context);
            filterOptions.IdRequestState = FilterOptionsHelper.GetIntValue("IdRequestState", context);
            filterOptions.IdRequestType = FilterOptionsHelper.GetIntValue("IdRequestType", context);
            filterOptions.DateOfFillingFrom = FilterOptionsHelper.GetDateTimeValue("DateOfFillingFrom", context);
            filterOptions.DateOfFillingTo = FilterOptionsHelper.GetDateTimeValue("DateOfFillingTo", context);
            return new ValueProviderResult(filterOptions,
                JsonConvert.SerializeObject(filterOptions), 
                CultureInfo.InvariantCulture);
        }
    }
}