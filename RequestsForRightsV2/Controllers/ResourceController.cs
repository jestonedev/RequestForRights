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
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return View(_resourceService.GetResourceById(id));
        }

        [HttpPut]
        public ActionResult Update(Resource resource)
        {
            if (!_securityService.CanUpdate(resource))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            if (resource == null)
            {
                Response.StatusCode = 400;
                return Content("Не передана ссылка на ресурс");
            }
            if (!ModelState.IsValid)
            {
                return View(resource);
            }
            try
            {
                _resourceService.UpdateResource(resource);
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
            return View(_resourceService.GetResourceById(id));
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var resourceGroup = _resourceService.GetResourceById(id);
            if (!_securityService.CanDelete(resourceGroup))
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
            return View();
        }

        [HttpPost]
        public ActionResult Create(Resource resource)
        {
            if (!_securityService.CanCreate(resource))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            if (resource == null)
            {
                Response.StatusCode = 400;
                return Content("Не передана ссылка на ресурс");
            }
            if (!ModelState.IsValid)
            {
                return View(resource);
            }
            try
            {
                _resourceService.InsertResource(resource);
                _resourceService.SaveChanges();
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