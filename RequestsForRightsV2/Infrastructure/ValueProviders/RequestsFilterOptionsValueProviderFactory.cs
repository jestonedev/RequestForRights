﻿using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using RequestsForRights.Web.Infrastructure.Helpers;
using RequestsForRights.Web.Models.FilterOptions;

namespace RequestsForRights.Web.Infrastructure.ValueProviders
{
    public class RequestsFilterOptionsValueProviderFactory: ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            try
            {
                var action = ValueProviderHelper.GetControllerActionByContext(HttpContext.Current);
                return action.GetParameters().Any(p => p.ParameterType == typeof(RequestsFilterOptions)) ?
                    new RequestsFilterOptionsValueProvider() : null;
            }
            catch (AmbiguousMatchException)
            {
                return null;
            }
        }
    }
}