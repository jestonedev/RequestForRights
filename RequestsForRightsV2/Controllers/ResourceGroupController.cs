using System;
using System.Web.Mvc;
using RequestsForRights.Database;

namespace RequestsForRightsV2.Controllers
{
    public class ResourceGroupController : Controller
    {
        private readonly IDatabaseContext _context;

        public ResourceGroupController(IDatabaseContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            _context = context;
        }

        //
        // GET: /ResourceGroup/
        public ActionResult Index()
        {
            return View(_context.ResourceGroups);
        }
	}
}