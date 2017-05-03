using System;
using System.Web.Mvc;
using RequestsForRights.Web.Infrastructure.Logging;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.ReportOptions;
using RequestsForRights.Web.Models.ViewModels;

namespace RequestsForRights.Web.Controllers
{
    public class ReportDepartmentAndResourceRightsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IReportSecurityService _reportSecurityService;
        private readonly ILogger _logger;

        public ReportDepartmentAndResourceRightsController(
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

        public ActionResult Index(ReportDepartmentAndResourceRightsOptions options)
        {
            if (!_reportSecurityService.CanReadResourcePermissions())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            return View(new ReportDepartmentAndResourceRightsViewModel
            {
                Resources = _reportService.GetResources(),
                Departments = _reportService.GetAllDepartments(),
                Options = options
            });
        }

        public ActionResult GetDataTable(ReportDepartmentAndResourceRightsOptions options)
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
            return PartialView("DataTable", new ReportDepartmentAndResourceRightsViewModel
            {
                Options = options,
                Rights = _reportService.GetDepartmentAndResourceRightsOnDate(options)
            });
        }
    }
}