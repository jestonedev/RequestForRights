using System;
using System.Web.Mvc;
using RequestsForRights.Web.Infrastructure.Utilities.TransfertToRoute.Extensions;

namespace RequestsForRights.Web.Infrastructure.Utilities.TransfertToRoute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class TransferActionOnlyAttribute : FilterAttribute, IAuthorizationFilter
    {
        public const string IsTransferActionMarker = "IsTransferAction";
        
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (!filterContext.IsTransferAction())
            {
                throw new InvalidOperationException(
                    string.Format("The action '{0}' is accessible only by a transfer request.", 
                    filterContext.ActionDescriptor.ActionName));
            }
        }
    }
}