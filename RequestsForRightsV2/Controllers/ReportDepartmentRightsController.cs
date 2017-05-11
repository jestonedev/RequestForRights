using System;
using System.Web.Mvc;
using RequestsForRights.Web.Infrastructure.Logging;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.ReportOptions;
using RequestsForRights.Web.Models.ViewModels;

namespace RequestsForRights.Web.Controllers
{
    public class ReportDepartmentRightsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IReportSecurityService _reportSecurityService;
        private readonly ILogger _logger;

        public ReportDepartmentRightsController(
            IReportService reportService, 
            IReportSecurityService reportSecurityService,
            ILogger logger
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

        public ActionResult Index(ReportDepartmentRightsOptions options)
        {
            if (!_reportSecurityService.CanReadDepartmentPermissions())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            return View(new ReportDepartmentRightsViewModel
            {
                Departments = _reportService.GetAllowedDepartments(),
                Options = options
            });
        }

        public ActionResult GetDataTable(ReportDepartmentRightsOptions options)
        {
            if (options.Date == null || options.Department == null)
            {
                return PartialView("DataTable", null);
            }
            if (!_reportSecurityService.CanReadDepartmentPermissions())
            {
                return PartialView("DataTable", null);
            }
            if (options.SortField == null)
            {
                options.SortField = "RequestUserSnp";
            }
            return PartialView("DataTable", new ReportDepartmentRightsViewModel
            {
                Options = options,
                Rights = _reportService.GetDepartmentRightsOnDate(options)
            });
        }
    }
}