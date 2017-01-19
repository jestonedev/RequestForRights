using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using RequestsForRights.Infrastructure.Helpers;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Infrastructure.Utilities.TransfertToRoute;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ModelViews;

namespace RequestsForRights.Controllers
{
    public class RequestRemoveUserController : Controller
    {
        private readonly IRequestService<RequestUserModel> _requestService;
        private readonly IRequestSecurityService<RequestUserModel> _securityService;

        public RequestRemoveUserController(IRequestService<RequestUserModel> requestService,
            IRequestSecurityService<RequestUserModel> securityService)
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
        }

        [TransferActionOnly]
        public ActionResult Detail(int id)
        {
            var request = _requestService.GetRequestModelBy(_requestService.GetRequestById(id));
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
            var request = _requestService.GetRequestModelBy(_requestService.GetRequestById(id));
            if (!_securityService.CanUpdate(request))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return View(_requestService.GetRequestViewModelBy(request));
        }

        [TransferActionOnly]
        [HttpPut]
        public ActionResult Update(RequestViewModel<RequestUserModel> requestViewModel)
        {
            if (requestViewModel == null || requestViewModel.RequestModel == null)
            {
                return RedirectToAction("BadRequestError", "Home",
                    new { message = "Не передана ссылка на ресурс" });
            }
            var request = _requestService.GetRequestModelBy(_requestService.GetRequestById(
                requestViewModel.RequestModel.IdRequest));
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
        public ActionResult Create(RequestViewModel<RequestUserModel> requestViewModel)
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
                _requestService.InsertRequest(requestViewModel.RequestModel);
                _requestService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            return RedirectToAction("Index", "Request");
        }

        private void Validate(RequestModel<RequestUserModel> request)
        {
            if (request.Users == null || !request.Users.Any())
            {
                ModelState.AddModelError(string.Empty,
                    "Необходимо указать по меньшей мере одно пользователя");
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
	}
}