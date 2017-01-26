using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.Models;

namespace RequestsForRights.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRightService _rightService;
        private readonly IUserSecurityService _userSecurityService;

        public UserController(
            IUserService userService, 
            IRightService rightService,
            IUserSecurityService userSecurityService)
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
        }

        public JsonResult GetUsers(string snpPattern)
        {
            return Json(_userService.FindUsers(snpPattern, 10), 
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
                return Json(new List<ResourceUserRightModel>());
            }
            if (!_userSecurityService.CanRead(requestUser))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            return Json(_rightService.GetPermanentRightsOnDate(date, requestUser.IdRequestUser, null),
                JsonRequestBehavior.AllowGet);
        }
	}
}