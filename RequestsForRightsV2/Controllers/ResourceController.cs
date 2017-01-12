using System;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using RequestsForRightsV2.Infrastructure.Helpers;
using RequestsForRightsV2.Infrastructure.Security.Interfaces;
using RequestsForRightsV2.Infrastructure.Services.Interfaces;
using RequestsForRightsV2.Models.FilterOptions;
using RequestsForRightsV2.Models.ModelViews;

namespace RequestsForRightsV2.Controllers
{
    public class ResourceController : Controller
    {
        private readonly IResourceService _resourceService;
        private readonly IResourceSecurityService _securityService;

        public ResourceController(IResourceService resourceService,
            IResourceSecurityService securityService)
        {
            if (resourceService == null)
            {
                throw new ArgumentNullException("resourceService");
            }
            _resourceService = resourceService;
            if (securityService == null)
            {
                throw new ArgumentNullException("securityService");
            }
            _securityService = securityService;
        }

        //
        // GET: /ResourceGroup/
        public ActionResult Index(FilterOptions filterOptions)
        {
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return View(_resourceService.GetResourceIndexModelView(filterOptions));
        }

        public ActionResult GetDataTable(FilterOptions filterOptions)
        {
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return PartialView("DataTable", _resourceService.GetResourceIndexModelView(filterOptions));
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            if (!_securityService.CanUpdate())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return View(_resourceService.GetResourceViewModelBy(id));
        }

        [HttpPut]
        public ActionResult Update(ResourceViewModel resourceViewModel)
        {
            if (resourceViewModel == null || resourceViewModel.Resource == null)
            {
                // TODO: редирект на ошибку 400
                Response.StatusCode = 400;
                return Content("Не передана ссылка на ресурс");
            }
            if (!_securityService.CanUpdate(resourceViewModel.Resource))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View(_resourceService.GetResourceViewModelBy(resourceViewModel.Resource));
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
        }

        public ActionResult Detail(int id)
        {
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return View(_resourceService.GetResourceBy(id));
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var resource = _resourceService.GetResourceViewModelBy(id);
            if (!_securityService.CanDelete(resource.Resource))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            try
            {
                _resourceService.DeleteResource(id);
                _resourceService.SaveChanges();
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
            return View(_resourceService.GetEmptyResourceViewModel());
        }

        [HttpPost]
        public ActionResult Create(ResourceViewModel model)
        {
            if (model == null || model.Resource == null)
            {
                // TODO: редирект на ошибку 400
                Response.StatusCode = 400;
                return Content("Не передана ссылка на ресурс");
            }
            if (!_securityService.CanCreate(model.Resource))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View(_resourceService.GetResourceViewModelBy(model.Resource));
            }
            try
            {
                _resourceService.InsertResource(model.Resource);
                _resourceService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetEmptyRightTemplate()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["RightIndex"] = 0;
            return PartialView("RightEditor", _resourceService.GetEmptyResourceViewModel());
        }
    }
}