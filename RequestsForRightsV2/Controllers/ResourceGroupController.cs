using System;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using RequestsForRights.Domain.Entities;
using RequestsForRightsV2.Infrastructure.Helpers;
using RequestsForRightsV2.Infrastructure.Security.Interfaces;
using RequestsForRightsV2.Infrastructure.Services.Interfaces;
using RequestsForRightsV2.Models.FilterOptions;

namespace RequestsForRightsV2.Controllers
{
    public class ResourceGroupController : Controller
    {
        private readonly IResourceGroupService _resourceGroupService;
        private readonly IResourceGroupSecurityService _securityService;

        public ResourceGroupController(IResourceGroupService resourceGroupService,
            IResourceGroupSecurityService securityService)
        {
            if (resourceGroupService == null)
            {
                throw new ArgumentNullException("resourceGroupService");
            }
            _resourceGroupService = resourceGroupService;
            if (securityService == null)
            {
                throw new ArgumentNullException("securityService");
            }
            _securityService = securityService;
        }

        public ActionResult Index(FilterOptions filterOptions)
        {
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return View(_resourceGroupService.GetResourceGroupIndexModelView(filterOptions,
                _resourceGroupService.GetFilteredResourceGroups(filterOptions.Filter)));
        }

        public ActionResult GetDataTable(FilterOptions filterOptions)
        {
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return PartialView("DataTable", _resourceGroupService.GetResourceGroupIndexModelView(filterOptions,
                _resourceGroupService.GetFilteredResourceGroups(filterOptions.Filter)));
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            if (!_securityService.CanUpdate())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return View(_resourceGroupService.GetResourceGroupBy(id));
        }

        [HttpPut]
        public ActionResult Update(ResourceGroup resourceGroup)
        {
            if (!_securityService.CanUpdate(resourceGroup))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            if (resourceGroup == null)
            {
                return RedirectToAction("BadRequestError", "Home",
                    new { message = "Не передана ссылка на категорию ресурсов" });
            }
            if (!ModelState.IsValid)
            {
                ViewData["SecurityService"] = _securityService;
                return View(resourceGroup);
            }
            try
            {
                _resourceGroupService.UpdateResourceGroup(resourceGroup);
                _resourceGroupService.SaveChanges();
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
            return View(_resourceGroupService.GetResourceGroupBy(id));
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var resourceGroup = _resourceGroupService.GetResourceGroupBy(id);
            if (!_securityService.CanDelete(resourceGroup))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            try
            {
                _resourceGroupService.DeleteResourceGroup(id);
                _resourceGroupService.SaveChanges();
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
            return View();    
        }

        [HttpPost]
        public ActionResult Create(ResourceGroup resourceGroup)
        {
            if (!_securityService.CanCreate(resourceGroup))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            if (resourceGroup == null)
            {
                return RedirectToAction("BadRequestError", "Home",
                    new { message = "Не передана ссылка на категорию ресурсов" });
            }
            if (!ModelState.IsValid)
            {
                ViewData["SecurityService"] = _securityService;
                return View(resourceGroup);
            }
            try
            {
                _resourceGroupService.InsertResourceGroup(resourceGroup);
                _resourceGroupService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            return RedirectToAction("Index");
        }
	}
}