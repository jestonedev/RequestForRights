using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Helpers;
using RequestsForRights.Web.Infrastructure.Logging;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.ViewModels;

namespace RequestsForRights.Web.Controllers
{
    public class ReportResourceOperatorController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IReportSecurityService _reportSecurityService;
        private readonly ILogger _logger;

        public ReportResourceOperatorController(
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

        public ActionResult Index(int? idDepartment)
        {
            if (!_reportSecurityService.CanReadResourceOperatorInfo())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            var departments = _reportService.GetAllowedDepartments();
            if (_reportSecurityService.CanVisiblieAllDepartmentsMark())
            {
                departments = new List<Department>
                {
                    new Department
                    {
                        Name = "Все организации",
                        IdDepartment = 0
                    }
                }.Concat(departments);
            }
            return View(new ReportResourceOperatorViewModel
            {
                Departments = departments,
                IdDepartment = ValueProviderHelper.GetValue("IdDepartment", System.Web.HttpContext.Current, idDepartment)
            });
        }

        public ActionResult GetDataTable(int? idDepartment)
        {
            if (!_reportSecurityService.CanReadResourceOperatorInfo())
            {
                return PartialView("DataTable", null);
            }
            idDepartment = ValueProviderHelper.GetValue("IdDepartment", System.Web.HttpContext.Current, idDepartment);
            if (idDepartment == null)
            {
                return PartialView("DataTable", null);
            }
            return PartialView("DataTable", new ReportResourceOperatorViewModel
            {
                IdDepartment = idDepartment,
                ResourceOperators = _reportService.GetResourceOperatorInfo(idDepartment).ToList()
            });
        }
    }
}