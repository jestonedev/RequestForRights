using System.Web.Mvc;
using System.Web.Routing;

namespace RequestsForRights.Web.Infrastructure.Utilities.TransfertToRoute.Extensions
{
    public static class ControllerExtensions
    {
        [NonAction]
        public static TransferToRouteResult TransferToAction(this Controller controller, string actionName, string controllerName)
        {
            return TransferToAction(controller, actionName, controllerName, null);
        }

        [NonAction]
        public static TransferToRouteResult TransferToAction(this Controller controller, string actionName, string controllerName, object routeValues)
        {
            return new TransferToRouteResult(actionName, controllerName, routeValues);
        }

        [NonAction]
        public static TransferToRouteResult TransferToAction(this Controller controller, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            return new TransferToRouteResult(actionName, controllerName, routeValues);
        }
    }
}