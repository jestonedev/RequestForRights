using System;
using System.Web.Mvc;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.ReportOptions;
using RequestsForRights.Models.ViewModels;

namespace RequestsForRights.Controllers
{
    public class ReportResourceRightsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IReportSecurityService _reportSecurityService;

        public ReportResourceRightsController(
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

        public ActionResult Index(ReportResourceRightsOptions options)
        {
            if (!_reportSecurityService.CanReadResourcePermissions())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            if (options.SortField == null)
            {
                options.SortField = "RequestUserSnp";
            }
            return View(new ReportResourceRightsViewModel
            {
                Resources = _reportService.GetResources(),
                Options = options
            });
        }

        public ActionResult GetDataTable(ReportResourceRightsOptions options)
        {
            if (options.Date == null || options.IdResource == null)
            {
                return PartialView("DataTable", null);
            }
            if (!_reportSecurityService.CanReadResourcePermissions())
            {
                return PartialView("DataTable", null);
            }
            if (options.SortField == null)
            {
                options.SortField = "RequestUserSnp";
            }
            return PartialView("DataTable", new ReportResourceRightsViewModel
            {
                Options = options,
                Rights = _reportService.GetResourceRightsOnDate(options)
            });
        }
    }
}