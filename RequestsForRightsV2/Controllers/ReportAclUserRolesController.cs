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
    public class ReportAclUserRolesController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IReportSecurityService _reportSecurityService;
        private readonly ILogger _logger;

        public ReportAclUserRolesController(
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

        public ActionResult Index(int? idDepartment, int? idRole)
        {
            if (!_reportSecurityService.CanReadAclUserRights())
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
            var roles = _reportService.GetAclRoles();
            roles = new List<AclRole>
            {
                new AclRole
                {
                    Name = "Все роли",
                    IdRole = 0
                }
            }.Concat(roles);
            return View("Index", new ReportAclUserRolesViewModel
            {
                IdDepartment = ValueProviderHelper.GetValue("IdDepartment", System.Web.HttpContext.Current, idDepartment),
                IdRole = ValueProviderHelper.GetValue("IdRole", System.Web.HttpContext.Current, idRole),
                Departments = departments,
                Roles = roles
            });
        }

        public ActionResult GetDataTable(int? idDepartment, int? idRole)
        {
            if (!_reportSecurityService.CanReadAclUserRights())
            {
                return PartialView("DataTable");
            }
            idDepartment = ValueProviderHelper.GetValue("IdDepartment", System.Web.HttpContext.Current, idDepartment);
            if (idDepartment == null)
            {
                return PartialView("DataTable", null);
            }
            idRole = ValueProviderHelper.GetValue("IdRole", System.Web.HttpContext.Current, idRole);
            if (idRole == null)
            {
                return PartialView("DataTable", null);
            }
            return View("DataTable", new ReportAclUserRolesViewModel
            {
                IdDepartment = idDepartment,
                IdRole = idRole,
                UserRoles = _reportService.GetAclUserRoles(idDepartment, idRole)
            });
        }
    }
}