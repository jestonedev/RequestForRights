using System.Linq;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.FilterOptions;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels;

namespace RequestsForRights.Web.Infrastructure.Services.Interfaces
{
    public interface IResourceService
    {
        IQueryable<Resource> GetVisibleResources(FilterOptions filterOptions,
            IQueryable<Resource> filteredResources);
        IQueryable<Resource> GetFilteredResources(string filter);
        ResourceIndexViewModel GetResourceIndexModelView(FilterOptions filterOptions,
            IQueryable<Resource> filteredResources);
        Resource GetResourceBy(int id);
        ResourceViewModel GetResourceViewModelBy(int id);
        ResourceViewModel GetResourceViewModelBy(Resource resource);
        ResourceViewModel GetEmptyResourceViewModel();
        Resource DeleteResource(int idResource);
        Resource UpdateResource(Resource resource, ResourceActFilesModel files);
        Resource InsertResource(Resource resource, ResourceActFilesModel files);
        int SaveChanges();
        ActFile GetActFile(int idFile);
        Department GetDepartmentInfo(int idDepartment);
    }
}
