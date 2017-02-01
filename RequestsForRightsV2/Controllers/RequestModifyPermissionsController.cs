using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using RequestsForRights.Infrastructure.Helpers;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Infrastructure.Utilities.EmailNotify;
using RequestsForRights.Infrastructure.Utilities.TransfertToRoute;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ViewModels.Request;

namespace RequestsForRights.Controllers
{
    public class RequestModifyPermissionsController : Controller
    {
        private readonly IRequestModifyPermissionsService _requestService;
        private readonly IRequestSecurityService<RequestUserModel> _securityService;
        private readonly IEmailBuilder _emailBuilder;
        private readonly IEmailSender _emailSender;

        public RequestModifyPermissionsController(IRequestModifyPermissionsService requestService,
            IRequestSecurityService<RequestUserModel> securityService,
            IEmailBuilder emailBuilder, IEmailSender emailSender)
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
        public ActionResult Update(RequestModifyPermissionsViewModel requestViewModel)
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
        public ActionResult Create()
        {
            if (!_securityService.CanCreate())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return View(_requestService.GetEmptyRequestViewModel());
        }

        [TransferActionOnly]
        [HttpPost]
        public ActionResult Create(RequestModifyPermissionsViewModel requestViewModel)
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
            ViewData["UserIndex"] = 0;
            ViewData["RightIndex"] = 0;
            ViewData["SecurityService"] = _securityService;
            return PartialView("RightEditor", _requestService.GetEmptyRequestViewModel());
        }

        private void Validate(RequestModel<RequestUserModel> request)
        {
            if (request.Users == null || !request.Users.Any())
            {
                ModelState.AddModelError(string.Empty,
                    "Необходимо указать по меньшей мере одного сотрудника");
            }
            if (request.Users != null && request.Users.Any(r => r.Rights == null || !r.Rights.Any()))
            {
                ModelState.AddModelError(string.Empty,
                    "Необходимо указать по меньшей мере по одному праву каждому сотруднику");
            }
            if (request.Users != null && request.Users.Any(
                r => r.Rights != null &&
                     !r.Rights.All(right => new[] {1, 2}.Contains(right.IdRequestRightGrantType))))
            {
                ModelState.AddModelError(string.Empty, "Некорректно задан тип одного из прав доступа");
            }
        }
	}
}