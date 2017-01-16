using System.Linq;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.FilterOptions;
using RequestsForRights.Models.ModelViews;

namespace RequestsForRights.Infrastructure.Services.Interfaces
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
        Resource UpdateResource(Resource resource);
        Resource InsertResource(Resource resource);
        int SaveChanges();
    }
}
