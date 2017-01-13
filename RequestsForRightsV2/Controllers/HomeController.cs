using System.Web.Mvc;

namespace RequestsForRights.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Help()
        {
            return RedirectPermanent("http://rqrights/App_Data/Manual.html");
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

        public ActionResult BadRequestError(string message)
        {
            Response.StatusCode = 400;
            if (!Request.IsAjaxRequest()) return View("BadRequestError", (object)message);
            return Content(message);
        }
    }
}