using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace RequestsForRights.Web.Infrastructure.Utilities.TransfertToRoute
{
    public class TransferToRouteResult : ActionResult
    {
        public TransferToRouteResult(string actionName, string controllerName, object routeValues)
            : this(actionName, controllerName, routeValues == null ? null : new RouteValueDictionary(routeValues))
        {
        }

        public TransferToRouteResult(string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            ActionName = actionName;
            ControllerName = controllerName;
            RouteValues = routeValues == null ? new RouteValueDictionary() : new RouteValueDictionary(routeValues);
        }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public RouteValueDictionary RouteValues { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            RouteValues.Add(TransferActionOnlyAttribute.IsTransferActionMarker, true);

            var urlHelper = new UrlHelper(context.RequestContext);
            var destinationUrl = urlHelper.Action(ActionName, ControllerName, RouteValues);
            if (string.IsNullOrEmpty(destinationUrl))
            {
                throw new InvalidOperationException("NoRoutesMatched");
            }

            context.HttpContext.Server.TransferRequest(destinationUrl, true);
        }
    }
}