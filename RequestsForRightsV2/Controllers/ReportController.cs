using System.Web.Mvc;

namespace RequestsForRightsV2.Controllers
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