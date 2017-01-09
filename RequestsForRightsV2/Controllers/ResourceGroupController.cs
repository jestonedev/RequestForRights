using System;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using RequestsForRightsV2.Infrastructure.Services.Interfaces;
using RequestsForRightsV2.Models.FilterOptions;
using RequestsForRightsV2.Models.ModelViews;

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
            return PartialView(_resourceGroupService.GetResourceGroupIndexModelView(filterOptions));
        }

        [HttpGet]
        public ActionResult Update(int idResourceGroup)
        {
            return View();
        }

        public ActionResult Detail(int idResourceGroup)
        {
            return View();
        }

        [HttpDelete]
        public ActionResult Delete(int idResourceGroup)
        {
            try
            {
                _resourceGroupService.DeleteResourceGroup(idResourceGroup);
                _resourceGroupService.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return new HttpStatusCodeResult(409, "Не удалось удалить категорию ресурса, т.к. она имеет зависимые ресурсы");
            }
            return RedirectToAction("GetDataTable");
        }
	}
}