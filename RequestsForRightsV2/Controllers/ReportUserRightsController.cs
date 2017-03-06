using System;
using System.Web.Mvc;
using RequestsForRights.Web.Infrastructure.Logging;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.ReportOptions;
using RequestsForRights.Web.Models.ViewModels;

namespace RequestsForRights.Web.Controllers
{
    public class ReportUserRightsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IReportSecurityService _reportSecurityService;
        private readonly ILogger _logger;

        public ReportUserRightsController(
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
            if (options.SortField == null)
            {
                options.SortField = "ResourceName";
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