using System.Web.Mvc;

namespace RequestsForRights.Web.Infrastructure.Utilities.TransfertToRoute.Extensions
{
    public static class ControllerContextExtensions
    {
        public static bool IsTransferAction(this ControllerContext context)
        {
            var routeData = context.RouteData;
            if (routeData == null)
            {
                return false;
            }

            return context.HttpContext.Request.QueryString[TransferActionOnlyAttribute.IsTransferActionMarker] != null;
        }
    }
}