using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Helpers;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Infrastructure.Utilities.EmailNotify;
using RequestsForRights.Web.Infrastructure.Utilities.TransfertToRoute;
using RequestsForRights.Web.Infrastructure.Utilities.TransfertToRoute.Extensions;
using RequestsForRights.Web.Models.FilterOptions;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels.Request;
using WebGrease.Css.Extensions;

namespace RequestsForRights.Web.Controllers
{
    [HandleError]
    public class RequestController : Controller
    {

        private readonly IRequestService<RequestUserModel, 
            RequestViewModel<RequestUserModel>> _requestService;
        private readonly IRequestSecurityService<RequestUserModel> _securityService;
        private readonly IEmailBuilder _emailBuilder;
        private readonly IEmailSender _emailSender;
        private readonly Infrastructure.Logging.ILogger _logger;

        public RequestController(IRequestService<RequestUserModel, 
            RequestViewModel<RequestUserModel>> requestService,
            IRequestSecurityService<RequestUserModel> securityService,
            IEmailBuilder emailBuilder,
            IEmailSender emailSender, Infrastructure.Logging.ILogger logger)
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
            if (request == null)
            {
                return RedirectToAction("NotFoundError", "Home");
            }
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

        public ActionResult Copy(int id)
        {
            var request = _requestService.GetRequestById(id);
            if (request == null)
            {
                return RedirectToAction("NotFoundError", "Home");
            }
            return RedirectToAction("Create", new { id, request.IdRequestType });
        }

        public ActionResult Detail(int id)
        {
            var request = _requestService.GetRequestById(id);
            if (request == null)
            {
                return RedirectToAction("NotFoundError", "Home");
            }
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
            var request = _requestService.GetRequestById(id);
            if (!_securityService.CanDelete())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            try
            {
                _requestService.DeleteRequest(id);
                _requestService.SaveChanges();
                var emails = _emailBuilder.DeleteRequestEmails(request);
                _emailSender.Send(emails);
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            return RedirectToAction("GetDataTable");
        }

        [HttpGet]
        public ActionResult Create(int idRequestType, int? id)
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

        public ActionResult GetEmptyRightTemplate(int idRequestType)
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            return TransfertTo("GetEmptyRightTemplate", idRequestType);
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
            return PartialView(_requestService.GetRequestsCountByStateTypes());
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
            var request = _requestService.GetRequestById(idRequest);
            if (!_securityService.CanComment(request))
            {
                return RedirectToAction("ForbiddenError", "Home");
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

        public ActionResult SetRequestState(int idRequest, int idRequestStateType, string agreementReason)
        {
            var request = _requestService.GetRequestById(idRequest);
            if (!_securityService.CanSetRequestState(request, idRequestStateType))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            try
            {
                _requestService.SetRequestState(idRequest, idRequestStateType, agreementReason);
                _requestService.SaveChanges();
                var emails = _emailBuilder.SetRequestStateEmails(
                    _requestService.GetRequestById(idRequest, true),
                    idRequestStateType, agreementReason);
                _emailSender.Send(emails);
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            ViewData["SecurityService"] = _securityService;
            return PartialView("Request/AgreementsContent", _requestService.GetRequestViewModelBy(request));
        }

        public ActionResult AcceptCancelRequest(int idRequest)
        {
            var request = _requestService.GetRequestById(idRequest);
            if (!_securityService.CanAcceptCancelRequest(request))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            try
            {
                _requestService.AcceptCancelRequest(idRequest);
                _requestService.SaveChanges();
                var agreementReason = request.RequestAgreements.Where(r => r.IdAgreementState == 3).Select(r => r.AgreementDescription)
                    .Aggregate((v, acc) => v + "<br>" + acc);
                var emails = _emailBuilder.SetRequestStateEmails(
                    _requestService.GetRequestById(idRequest, true),
                    5, agreementReason);
                _emailSender.Send(emails);
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            ViewData["SecurityService"] = _securityService;
            return PartialView("Request/AgreementsContent", _requestService.GetRequestViewModelBy(request));
        }

        public ActionResult AddCoordinator(int idRequest, Coordinator coordinator, string sendDescription)
        {
            var request = _requestService.GetRequestById(idRequest);
            if (!_securityService.CanAddCoordinator(request))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            try
            {
                _requestService.AddCooordinator(idRequest, coordinator, sendDescription);
                _requestService.SaveChanges();
                var emails = _emailBuilder.AddCoordinatorEmails(
                    _requestService.GetRequestById(idRequest, true),
                    coordinator, sendDescription);
                _emailSender.Send(emails);
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            ViewData["SecurityService"] = _securityService;
            return PartialView("Request/AgreementsContent", _requestService.GetRequestViewModelBy(request));
        }

        public ActionResult ExcludeAgreementor(int idRequest, int idUser, int idRequestAgreementType)
        {
            var request = _requestService.GetRequestById(idRequest);
            if (!_securityService.CanExcludeAgreementor(request))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            try
            {
                _requestService.ExcludeAgreementor(idRequest, idUser, idRequestAgreementType);
                _requestService.SaveChanges();
                var emails = _emailBuilder.SetRequestStateEmails(
                    _requestService.GetRequestById(idRequest, true), 2, null);
                _emailSender.Send(emails);
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            ViewData["SecurityService"] = _securityService;
            return PartialView("Request/AgreementsContent", _requestService.GetRequestViewModelBy(request));
        }
    }
}