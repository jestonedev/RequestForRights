using System;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using RequestsForRights.Infrastructure.Helpers;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.FilterOptions;

namespace RequestsForRights.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestService _requestService;
        private readonly IRequestSecurityService _securityService;

        public RequestController(IRequestService requestService,
            IRequestSecurityService securityService)
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

        //
        // GET: /ResourceGroup/
        public ActionResult Index(RequestsFilterOptions filterOptions)
        {
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
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
            return PartialView("DataTable",
                _requestService.GetRequestIndexModelView(filterOptions,
                _requestService.GetFilteredRequests(filterOptions)));
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            if (!_securityService.CanUpdate())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return Content(""/*_requestService.GetRequestViewModelBy(id)*/);
        }

        /*[HttpPut]
        public ActionResult Update(ResourceViewModel resourceViewModel)
        {
            if (resourceViewModel == null || resourceViewModel.Resource == null)
            {
                return RedirectToAction("BadRequestError", "Home",
                    new { message = "Не передана ссылка на ресурс" });
            }
            if (!_securityService.CanUpdate(resourceViewModel.Resource))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            if (resourceViewModel.Resource.ResourceRights == null ||
                resourceViewModel.Resource.ResourceRights.Count < 1)
            {
                ModelState.AddModelError(string.Empty, 
                    "Необходимо задать по меньшей мере одно право для ресурса");
            }
            if (!ModelState.IsValid)
            {
                ViewData["SecurityService"] = _securityService;
                return System.Web.UI.WebControls.View(_resourceService.GetResourceViewModelBy(resourceViewModel.Resource));
            }
            try
            {
                _resourceService.UpdateResource(resourceViewModel.Resource);
                _resourceService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home", 
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            return Request["returnUri"] != null ? (ActionResult)Redirect(Request["returnUri"]) :
                RedirectToAction("Index");
        }*/

        public ActionResult Detail(int id)
        {
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return Content("")/*View(_requestService.GetRequestBy(id))*/;
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var request = _requestService.GetRequestById(id);
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
        public ActionResult Create()
        {
            if (!_securityService.CanCreate())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return Content("") /*View(_requestService.GetEmptyResourceViewModel())*/;
        }
        /*
        [HttpPost]
        public ActionResult Create(ResourceViewModel resourceViewModel)
        {
            if (resourceViewModel == null || resourceViewModel.Resource == null)
            {
                return RedirectToAction("BadRequestError", "Home",
                    new { message = "Не передана ссылка на ресурс" });
            }
            if (!_securityService.CanCreate(resourceViewModel.Resource))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            if (resourceViewModel.Resource.ResourceRights == null || 
                resourceViewModel.Resource.ResourceRights.Count < 1)
            {
                ModelState.AddModelError(string.Empty, 
                    "Необходимо задать по меньшей мере одно право для ресурса");
            }
            if (!ModelState.IsValid)
            {
                ViewData["SecurityService"] = _securityService;
                return System.Web.UI.WebControls.View(_resourceService.GetResourceViewModelBy(resourceViewModel.Resource));
            }
            try
            {
                _resourceService.InsertResource(resourceViewModel.Resource);
                _resourceService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            return RedirectToAction("Index");
        }*/

        [ChildActionOnly]
        public ActionResult RequestsByStatesMenuItems()
        {
            return PartialView(_requestService.GetNotSeenRequestsViewModel());
        }
    }
}