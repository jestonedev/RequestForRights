using System;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using RequestsForRights.Infrastructure.Helpers;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.FilterOptions;
using RequestsForRights.Models.ModelViews;

namespace RequestsForRights.Controllers
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
            return View(_resourceService.GetResourceIndexModelView(filterOptions, 
                _resourceService.GetFilteredResources(filterOptions.Filter)));
        }

        public ActionResult GetDataTable(FilterOptions filterOptions)
        {
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return PartialView("DataTable", _resourceService.GetResourceIndexModelView(filterOptions,
                _resourceService.GetFilteredResources(filterOptions.Filter)));
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
                return View(_resourceService.GetResourceViewModelBy(resourceViewModel.Resource));
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