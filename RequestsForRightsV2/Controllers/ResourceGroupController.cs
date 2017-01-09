using System;
using System.Data.Entity.Infrastructure;
using System.Net.Http;
using System.Web.Mvc;
using RequestsForRights.Domain.Entities;
using RequestsForRightsV2.Infrastructure.Services.Interfaces;
using RequestsForRightsV2.Models.FilterOptions;

namespace RequestsForRightsV2.Controllers
{
    public class ResourceGroupController : Controller
    {
        private readonly IResourceGroupService _resourceGroupService;
        
        public ResourceGroupController(IResourceGroupService resourceGroupService)
        {
            if (resourceGroupService == null)
            {
                throw new ArgumentNullException("resourceGroupService");
            }
            _resourceGroupService = resourceGroupService;
        }
        //
        // GET: /ResourceGroup/
        public ActionResult Index(FilterOptions filterOptions)
        {
            return View(_resourceGroupService.GetResourceGroupIndexModelView(filterOptions));
        }

        public PartialViewResult GetDataTable(FilterOptions filterOptions)
        {
            return PartialView("DataTable", _resourceGroupService.GetResourceGroupIndexModelView(filterOptions));
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            return View(_resourceGroupService.GetResourceGroupById(id));
        }

        [HttpPost]
        public ActionResult Update(ResourceGroup resourceGroup)
        {
            if (resourceGroup == null)
            {
                Response.StatusCode = 400;
                return Content("Не передана ссылка на категорию ресурсов");
            }
            if (!ModelState.IsValid)
            {
                return View(resourceGroup);
            }
            try
            {
                _resourceGroupService.UpdateResourceGroup(resourceGroup);
                _resourceGroupService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                Response.StatusCode = 409;
                return Content(e.Message);
            }
            return Request["returnUri"] != null ? (ActionResult)Redirect(Request["returnUri"]) : 
                RedirectToAction("Index");
        }

        public ActionResult Detail(int id)
        {
            return View(_resourceGroupService.GetResourceGroupById(id));
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                _resourceGroupService.DeleteResourceGroup(id);
                _resourceGroupService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                Response.StatusCode = 409;
                return Content(e.Message);
            }
            return RedirectToAction("GetDataTable");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();    
        }

        [HttpPost]
        public ActionResult Create(ResourceGroup resourceGroup)
        {
            if (resourceGroup == null)
            {
                Response.StatusCode = 400;
                return Content("Не передана ссылка на категорию ресурсов");
            }
            if (!ModelState.IsValid)
            {
                return View(resourceGroup);
            }
            try
            {
                _resourceGroupService.InsertResourceGroup(resourceGroup);
                _resourceGroupService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                Response.StatusCode = 409;
                return Content(e.Message);
            }
            return RedirectToAction("Index");
        }
	}
}