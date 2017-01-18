using System;
using System.Web.Mvc;
using RequestsForRights.Infrastructure.Services.Interfaces;

namespace RequestsForRights.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }
            _userService = userService;
        }

        public JsonResult GetUsers(string snpPattern)
        {
            return Json(_userService.FindUsers(snpPattern, 10), JsonRequestBehavior.AllowGet);
        }
	}
}