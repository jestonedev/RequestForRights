using System;
using System.Web.Mvc;
using RequestsForRights.Web.Infrastructure.Logging;

namespace RequestsForRights.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILogger logger)
        {

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            _logger = logger;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            _logger.Error(filterContext.Exception);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult ForbiddenError()
        {
            Response.StatusCode = 403;
            Response.TrySkipIisCustomErrors = true; 
            if (!Request.IsAjaxRequest()) return View("ForbiddenError");
            return Content("Доступ запрещен");
        }

        public ActionResult ConflictError(string message)
        {
            _logger.Error(message);
            Response.StatusCode = 409;
            Response.TrySkipIisCustomErrors = true; 
            if (!Request.IsAjaxRequest()) return View("ConflictError", (object)message);
            return Content(message);
        }

        public ActionResult ServerError(string message)
        {
            _logger.Error(message);
            Response.StatusCode = 409;
            Response.TrySkipIisCustomErrors = true; 
            if (!Request.IsAjaxRequest()) return View("ServerError", (object)message);
            return Content(message);
        }

        public ActionResult BadRequestError(string message)
        {
            _logger.Error(message);
            Response.StatusCode = 400;
            Response.TrySkipIisCustomErrors = true; 
            if (!Request.IsAjaxRequest()) return View("BadRequestError", (object)message);
            return Content(message);
        }

        public ActionResult NotFoundError()
        {
            return View("NotFoundError");
        }
    }
}