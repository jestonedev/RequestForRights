using System;
using System.Web.Mvc;
using RequestsForRights.Web.Infrastructure.Logging;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.ReportOptions;
using RequestsForRights.Web.Models.ViewModels;

namespace RequestsForRights.Web.Controllers
{
    public class ReportUserRightsHistoryController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IReportSecurityService _reportSecurityService;
        private readonly ILogger _logger;

        public ReportUserRightsHistoryController(
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

        public ActionResult Index(ReportUserRightsHistoryOptions options)
        {
            if (!_reportSecurityService.CanReadUserPermissions())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            return View(new ReportUserRightsHistoryViewModel
            {
                Options = options
            });
        }

        public ActionResult GetDataTable(ReportUserRightsHistoryOptions options)
        {
            var requestUser = _reportService.FindUser(
                new ReportUserRightsOptions
                {
                    Login = options.Login,
                    Department = options.Department,
                    Unit = options.Unit,
                    Snp = options.Snp
                });
            if (requestUser == null || options.DateFrom == null || options.DateTo == null)
            {
                return PartialView("DataTable", null);
            }
            if (!_reportSecurityService.CanReadUserPermissions(requestUser))
            {
                return PartialView("DataTable", null);
            }
            return PartialView("DataTable",
                new ReportUserRightsHistoryViewModel
                {
                    Options = options,
                    Rights = _reportService.GetUserRightsHistoryOnDate(options, requestUser.IdRequestUser),
                    RequestUser = requestUser
                });
        }
	}
}