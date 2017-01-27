using System;
using System.Web.Mvc;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.ReportOptions;
using RequestsForRights.Models.ViewModels;

namespace RequestsForRights.Controllers
{
    public class ReportUserRightsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IReportSecurityService _reportSecurityService;

        public ReportUserRightsController(
            IReportService reportService, 
            IReportSecurityService reportSecurityService
        )
        {
            if (reportSecurityService == null)
            {
                throw new ArgumentNullException("reportSecurityService");
            }
            _reportSecurityService = reportSecurityService;
            if (reportService == null)
            {
                throw new ArgumentNullException("reportService");
            }
            _reportService = reportService;
        }

        public ActionResult Index(ReportUserRightsOptions options)
        {
            if (!_reportSecurityService.CanReadUserPermissions())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            return View(new ReportUserRightsViewModel
            {
                Options = options
            });
        }

        public ActionResult GetDataTable(ReportUserRightsOptions options)
        {
            var requestUser = _reportService.FindUser(options);
            if (requestUser == null || options.Date == null)
            {
                return PartialView("DataTable", null);
            }
            if (!_reportSecurityService.CanReadUserPermissions(requestUser))
            {
                return PartialView("DataTable", null);
            }
            return PartialView("DataTable",
                new ReportUserRightsViewModel
                {
                    Options = options,
                    Rights = _reportService.GetUserRightsOnDate(options, requestUser.IdRequestUser),
                    RequestUser = requestUser
                });
        }
	}
}