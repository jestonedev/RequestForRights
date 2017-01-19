using System;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using RequestsForRights.Infrastructure.Helpers;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Infrastructure.Utilities.TransfertToRoute;
using RequestsForRights.Infrastructure.Utilities.TransfertToRoute.Extensions;
using RequestsForRights.Models.FilterOptions;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ModelViews;
using WebGrease.Css.Extensions;

namespace RequestsForRights.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestService<RequestUserModel> _requestService;
        private readonly IRequestSecurityService<RequestUserModel> _securityService;

        public RequestController(IRequestService<RequestUserModel> requestService,
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

        public ActionResult Index(RequestsFilterOptions filterOptions)
        {
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            ViewData["RequestService"] = _requestService;
            return View(
                _requestService.GetRequestIndexModelView(filterOptions,
                _requestService.GetFilteredRequests(filterOptions)));
        }

        public ActionResult GetDataTable(RequestsFilterOptions filterOptions)
        {
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            ViewData["RequestService"] = _requestService;
            return PartialView("DataTable",
                _requestService.GetRequestIndexModelView(filterOptions,
                _requestService.GetFilteredRequests(filterOptions)));
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var request = _requestService.GetRequestById(id);
            try
            {
                var userInfo = _securityService.GetUserInfo();
                if (userInfo == null)
                {
                    return RedirectToAction("ForbiddenError", "Home");
                }
                _requestService.UpdateUserLastSeen(id, userInfo.IdUser);
                _requestService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home", new { message = e.Message });
            }
            return TransfertTo("Update", request.IdRequestType);
        }

        [HttpPut]
        public ActionResult Update(RequestViewModel<RequestUserModel> requestViewModel)
        {
            if (requestViewModel == null || requestViewModel.RequestModel == null)
            {
                return RedirectToAction("BadRequestError", "Home",
                    new { message = "Не передана ссылка на заявку" });
            }
            return TransfertTo("Update", requestViewModel.RequestModel.IdRequestType);
        }

        public ActionResult Detail(int id)
        {
            var request = _requestService.GetRequestById(id);
            try
            {
                var userInfo = _securityService.GetUserInfo();
                if (userInfo == null)
                {
                    return RedirectToAction("ForbiddenError", "Home");
                }
                _requestService.UpdateUserLastSeen(id, userInfo.IdUser);
                _requestService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home", new { message = e.Message });
            }
            return TransfertTo("Detail", request.IdRequestType);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var request = _requestService.GetRequestModelBy(_requestService.GetRequestById(id));
            if (!_securityService.CanDelete(request))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            try
            {
                _requestService.DeleteRequest(id);
                _requestService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            return RedirectToAction("GetDataTable");
        }

        [HttpGet]
        public ActionResult Create(int idRequestType)
        {
            return TransfertTo("Create", idRequestType);
        }

        
        [HttpPost]
        public ActionResult Create(RequestViewModel<RequestUserModel> requestViewModel)
        {
            if (requestViewModel == null || requestViewModel.RequestModel == null)
            {
                return RedirectToAction("BadRequestError", "Home",
                    new { message = "Не передана ссылка на зявку" });
            }
            return TransfertTo("Create", requestViewModel.RequestModel.IdRequestType);
        }

        [HttpGet]
        public ActionResult GetEmptyUserTemplate(int idRequestType)
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            return TransfertTo("GetEmptyUserTemplate", idRequestType);
        }

        private ActionResult TransfertTo(string actionName, int idRequestType)
        {
            var controller = RequestHelper.IdRequestTypeToControllerName(idRequestType);
            if (controller == null)
            {
                return RedirectToAction("BadRequestError", "Home",
                    new { message = "Некорректный тип заявки" });
            }
            var routedValues = Request.GetRouteValueDictionary();
            RouteData.Values.ForEach(r => routedValues.AddWithCheck(r.Key, r.Value));
            return new TransferToRouteResult(actionName, controller, routedValues);
        }

        [ChildActionOnly]
        public ActionResult RequestsByStatesMenuItems()
        {
            return PartialView(_requestService.GetNotSeenRequestsViewModel());
        }

        [ChildActionOnly]
        public ActionResult RequestCreateMenuItems()
        {
            return PartialView(_requestService.GetRequestTypes());
        }

        [HttpPost]
        public ActionResult AddComment(int idRequest, string comment)
        {
            if (comment == null)
            {
                return RedirectToAction("BadRequestError", "Home",
                    new { message = "Нельзя добавить пустой комментарий" });
            }
            try
            {
                _requestService.AddComment(idRequest, comment);
                _requestService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            return PartialView("Request/ExtCommentsList", 
                _requestService.GetRequestExtComments(idRequest));
        }
    }
}