using System.Web.Mvc;

namespace RequestsForRights.Controllers
{
    public class ReportController : Controller
    {
        public ActionResult UserPermissions()
        {
            return View();
        }

        public ActionResult ResourcePermissions()
        {
            return View();
        }
    }
}