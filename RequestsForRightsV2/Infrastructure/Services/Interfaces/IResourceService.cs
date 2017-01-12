using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRightsV2.Models.FilterOptions;
using RequestsForRightsV2.Models.ModelViews;

namespace RequestsForRightsV2.Infrastructure.Services.Interfaces
{
    public interface IResourceService
    {
        IEnumerable<Resource> GetVisibleResources(FilterOptions filterOptions);
        ResourceIndexModelView GetResourceIndexModelView(FilterOptions filterOptions);
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
