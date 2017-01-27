﻿using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RequestsForRights.Infrastructure.Enums;
using RequestsForRights.Infrastructure.Helpers;
using RequestsForRights.Models.FilterOptions;

namespace RequestsForRights.Infrastructure.ValueProviders
{
    public class FilterOptionsValueProvider<T>: IValueProvider
        where T: FilterOptions, new()
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
            var filterOptions = GetFilterOptions();
            return new ValueProviderResult(filterOptions,
                JsonConvert.SerializeObject(filterOptions),
                CultureInfo.InvariantCulture);
        }

        protected T GetFilterOptions()
        {
            var context = HttpContext.Current;
            var filterOptions = new T
            {
                PageSize = ValueProviderHelper.GetValue("PageSize", context, 25),
                PageIndex = ValueProviderHelper.GetValue("PageIndex", context, 0),
                Filter = ValueProviderHelper.GetValue<string>("Filter", context, null),
                SortField = ValueProviderHelper.GetValue<string>("SortField", context, null),
                SortDirection = ValueProviderHelper.GetValue("SortDirection", context, SortDirection.Asc)
            };
            return filterOptions;
        }
    }
}