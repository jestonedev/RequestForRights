using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.Models;

namespace RequestsForRights.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IReportSecurityService _reportSecurityService;

        public ReportController(
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

        public ActionResult UserPermissions()
        {
            if (!_reportSecurityService.CanReadUserPermissions())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            return View();
        }

        public ActionResult GetUserPermissionsDataTable(DateTime date, RequestUser requestUser)
        {
            requestUser = _reportService.FindUser(requestUser);
            if (requestUser == null)
            {
                return Json(new List<ResourceUserRightModel>());
            }
            if (!_reportSecurityService.CanReadUserPermissions(requestUser))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            return PartialView("UserPermissionsDataTable", _reportService.GetUserRightsOnDate(date, requestUser.IdRequestUser));
        }

        public ActionResult ResourcePermissions()
        {
            if (!_reportSecurityService.CanReadResourcePermissions())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            return View(_reportService.GetResources());
        }

        public ActionResult GetResourcePermissionsDataTable(DateTime? date, int? idResource)
        {
            if (!_reportSecurityService.CanReadResourcePermissions())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            if (date == null || idResource == null)
            {
                return PartialView("ResourcePermissionsDataTable", null);
            }
            return PartialView("ResourcePermissionsDataTable", 
                _reportService.GetResourceRightsOnDate(date.Value, idResource.Value));
        }
    }
}