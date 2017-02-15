using System.Web.Mvc;

namespace RequestsForRights.Web.Controllers
{
    public class HomeController : Controller
    {
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
            if (!Request.IsAjaxRequest()) return View("ForbiddenError");
            return Content("Доступ запрещен");
        }

        public ActionResult ConflictError(string message)
        {
            Response.StatusCode = 409;
            if (!Request.IsAjaxRequest()) return View("ConflictError", (object)message);
            return Content(message);
        }

        public ActionResult ServerError(string message)
        {
            Response.StatusCode = 409;
            if (!Request.IsAjaxRequest()) return View("ServerError", (object)message);
            return Content(message);
        }

        public ActionResult BadRequestError(string message)
        {
            Response.StatusCode = 400;
            if (!Request.IsAjaxRequest()) return View("BadRequestError", (object)message);
            return Content(message);
        }

        public ActionResult NotFoundError()
        {
            return View("NotFoundError");
        }

        public ActionResult UnknownServerError()
        {
            Response.StatusCode = 500;
            return View("UnknownServerError");
        }
    }
}