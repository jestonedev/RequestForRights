﻿using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using RequestsForRights.Web.Infrastructure.Helpers;
using RequestsForRights.Web.Models.ReportOptions;

namespace RequestsForRights.Web.Infrastructure.ValueProviders
{
    public class ReportDepartmentAndResourceRightsOptionsValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            try
            {
                var action = ValueProviderHelper.GetControllerActionByContext(HttpContext.Current);
                return action.GetParameters().Any(p => p.ParameterType == typeof(ReportDepartmentAndResourceRightsOptions)) ?
                    new ReportDepartmentAndResourceRightsOptionsValueProvider() : null;
            }
            catch (AmbiguousMatchException)
            {
                return null;
            }
        }
    }
}