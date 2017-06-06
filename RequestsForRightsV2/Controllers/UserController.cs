using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Domain.Enums;
using RequestsForRights.Web.Infrastructure.Logging;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.Models;

namespace RequestsForRights.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRightService _rightService;
        private readonly IUserSecurityService _userSecurityService;
        private readonly ILogger _logger;

        public UserController(
            IUserService userService, 
            IRightService rightService,
            IUserSecurityService userSecurityService, ILogger logger)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }
            _userService = userService;
            if (rightService == null)
            {
                throw new ArgumentNullException("rightService");
            }
            _rightService = rightService;
            if (userSecurityService == null)
            {
                throw new ArgumentNullException("userSecurityService");
            }
            _userSecurityService = userSecurityService;
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

        public JsonResult GetUsers(string snpPattern, UsersCategory usersCategory = UsersCategory.ActiveUsers)
        {
            return Json(_userService.FindUsers(snpPattern, usersCategory, 10), 
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAdUsers(string snpPattern)
        {
            return Json(_userService.FindAllActiveDirectoryUsers(snpPattern, 10), 
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPermanentRightsOnDate(DateTime date, RequestUser requestUser)
        {
           requestUser = _userService.FindUser(requestUser);
            if (requestUser == null)
            {
                return Json(new List<ResourceUserRightModel>(), JsonRequestBehavior.AllowGet);
            }
            if (!_userSecurityService.CanRead(requestUser))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            return Json(_rightService.GetPermanentRightsOnDate(date, requestUser.IdRequestUser, null, null, null),
                JsonRequestBehavior.AllowGet);
        }
	}
}