﻿using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using RequestsForRights.Web.Infrastructure.Helpers;
using RequestsForRights.Web.Infrastructure.Logging;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Infrastructure.Utilities.EmailNotify;
using RequestsForRights.Web.Infrastructure.Utilities.TransfertToRoute;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels.Request;
using RequestsForRights.Domain.Enums;

namespace RequestsForRights.Web.Controllers
{
    public class RequestAddUserController : Controller
    {
        private readonly IRequestAddUserService _requestService;
        private readonly IRequestSecurityService<RequestUserModel> _securityService;
        private readonly IUserService _userService;
        private readonly IEmailBuilder _emailBuilder;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public RequestAddUserController(IRequestAddUserService requestService,
            IRequestSecurityService<RequestUserModel> securityService,
            IUserService userService,
            IEmailBuilder emailBuilder, IEmailSender emailSender,
            ILogger logger)
        {
            if (requestService == null)
            {
                throw new ArgumentNullException("requestService");
            }
            _requestService = requestService;
            if (securityService == null)
            {
                throw new ArgumentNullException("securityService");
            }
            _securityService = securityService;
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }
            _userService = userService;
            if (emailBuilder == null)
            {
                throw new ArgumentNullException("emailBuilder");
            }
            _emailBuilder = emailBuilder;
            if (emailSender == null)
            {
                throw new ArgumentNullException("emailSender");
            }
            _emailSender = emailSender;
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

        [TransferActionOnly]
        public ActionResult Detail(int id)
        {
            var request = _requestService.GetRequestById(id);
            if (!_securityService.CanRead(request))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            ViewData["UserService"] = _userService;
            return View(_requestService.GetRequestViewModelBy(request));
        }

        [TransferActionOnly]
        [HttpGet]
        public ActionResult Update(int id)
        {
            var request = _requestService.GetRequestById(id);
            if (!_securityService.CanUpdate(request))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return View(_requestService.GetRequestViewModelBy(request));
        }

        [TransferActionOnly]
        [HttpPut]
        public ActionResult Update(RequestAddUserViewModel requestViewModel)
        {
            if (requestViewModel == null || requestViewModel.RequestModel == null)
            {
                return RedirectToAction("BadRequestError", "Home",
                    new { message = "Не передана ссылка на ресурс" });
            }
            var request = _requestService.GetRequestById(requestViewModel.RequestModel.IdRequest);
            if (!_securityService.CanUpdate(request))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            Validate(requestViewModel.RequestModel);
            if (!ModelState.IsValid)
            {
                ViewData["SecurityService"] = _securityService;
                return View(_requestService.GetRequestViewModelBy(requestViewModel.RequestModel));
            }
            try
            {
                _requestService.UpdateRequest(requestViewModel.RequestModel);
                _requestService.UpdateRequestState(request);
                _requestService.SaveChanges();
                var emails = _emailBuilder.UpdateRequestEmails(
                    _requestService.GetRequestById(request.IdRequest, true));
                _emailSender.Send(emails);
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            return Request["returnUri"] != null ? (ActionResult)Redirect(Request["returnUri"]) :
                RedirectToAction("Index", "Request");
        }

        [TransferActionOnly]
        [HttpGet]
        public ActionResult Create(int? id)
        {
            if (!_securityService.CanCreate())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            if (id != null)
            {
                var request = _requestService.GetRequestById(id.Value);
                return View(_requestService.GetRequestViewModelBy(request));
            }
            return View(_requestService.GetEmptyRequestViewModel());
        }
        
        [TransferActionOnly]
        [HttpPost]
        public ActionResult Create(RequestAddUserViewModel requestViewModel)
        {
            if (requestViewModel == null || requestViewModel.RequestModel == null)
            {
                return RedirectToAction("BadRequestError", "Home",
                    new { message = "Не передана ссылка на ресурс" });
            }
            if (!_securityService.CanCreate(requestViewModel.RequestModel))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            Validate(requestViewModel.RequestModel);
            if (!ModelState.IsValid)
            {
                ViewData["SecurityService"] = _securityService;
                return View(_requestService.GetRequestViewModelBy(requestViewModel.RequestModel));
            }
            try
            {
                var request = _requestService.InsertRequest(requestViewModel.RequestModel);
                _requestService.SaveChanges();
                var emails = _emailBuilder.CreateRequestEmails(
                    _requestService.GetRequestById(request.IdRequest, true));
                _emailSender.Send(emails);
                return RedirectToAction("Detail", "Request", new { id = request.IdRequest });
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
        }

        [HttpGet]
        public ActionResult GetEmptyUserTemplate()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["UserIndex"] = 0;
            ViewData["SecurityService"] = _securityService;
            return PartialView("UserEditor", _requestService.GetEmptyRequestViewModel());
        }

        [HttpGet]
        public ActionResult GetEmptyRightTemplate()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            var viewModel = _requestService.GetEmptyRequestViewModel();
            ViewData["UserIndex"] = 0;
            if (viewModel.RequestModel.Users.Any())
            {
                ViewData["RightIndex"] = viewModel.RequestModel.Users[0].Rights.Count - 1;
            }
            ViewData["SecurityService"] = _securityService;
            return PartialView("RightEditor", viewModel);
        }

        private void Validate(RequestModel<RequestUserModel> request)
        {
            if (request.Users == null || !request.Users.Any())
            {
                ModelState.AddModelError(string.Empty,
                    @"Необходимо указать по меньшей мере одного сотрудника");
            }
            if (request.Users != null && request.Users.Any(r => r.Rights == null || !r.Rights.Any()))
            {
                ModelState.AddModelError(string.Empty,
                    @"Необходимо указать по меньшей мере по одному праву каждому сотруднику");
            }
            if (request.Users != null && request.Users.Any(
                r => r.Rights != null && r.Rights.Any(right => right.IdRequestRightGrantType != 1)))
            {
                ModelState.AddModelError(string.Empty, @"Некорректно задан тип одного из прав доступа");
            }
        }

        public ActionResult GetDuplicateUserNotification(string requesterSnp,
            string requesterDepartment,
            string requestUserSnp,
            string requestUserDepartment,
            string requestUserUnit)
        {
            if (!_securityService.CanSendTransferUserNotification())
            {
                return Content("");
            }
            var users = _userService.FindUsers(requestUserSnp, UsersCategory.ActiveUsers, 5).Where(u => u.Department != requestUserDepartment).ToList();
            if (!users.Any())
            {
                return Content("");
            }
            ViewData["requesterSnp"] = requesterSnp;
            ViewData["requesterDepartment"] = requesterDepartment;
            ViewData["requestUserSnp"] = requestUserSnp;
            ViewData["requestUserDepartment"] = requestUserDepartment;
            ViewData["requestUserUnit"] = requestUserUnit;
            return PartialView(users);
        }

        public ActionResult SendTransferUserNotification(string requesterSnp,
            string requesterDepartment,
            string transferUserSnp,
            string transferToDepartment,
            string transferToUnit,
            string transferFromDepartment,
            string transferFromUnit)
        {
            if (!_securityService.CanSendTransferUserNotification())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            var emails = _emailBuilder.CreateSendTransferUserEmails(
                requesterSnp,
                requesterDepartment,
                transferUserSnp,
                transferToDepartment,
                transferToUnit,
                transferFromDepartment,
                transferFromUnit);
            _emailSender.Send(emails);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        } 
    }
}