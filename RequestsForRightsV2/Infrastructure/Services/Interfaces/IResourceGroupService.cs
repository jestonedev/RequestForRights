using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRightsV2.Models.FilterOptions;
using RequestsForRightsV2.Models.ModelViews;

namespace RequestsForRightsV2.Infrastructure.Services.Interfaces
{
    public interface IResourceGroupService
    {
        IEnumerable<ResourceGroup> GetVisibleResourceGroups(FilterOptions filterOptions, 
            IEnumerable<ResourceGroup> filteredResourceGroups);
        IEnumerable<ResourceGroup> GetFilteredResourceGroups(string filter);
        ResourceGroupIndexModelView GetResourceGroupIndexModelView(FilterOptions filterOptions,
            IEnumerable<ResourceGroup> filteredResourceGroups);
        ResourceGroup DeleteResourceGroup(int idResourceGroup);
        ResourceGroup GetResourceGroupBy(int id);
        ResourceGroup UpdateResourceGroup(ResourceGroup resourceGroup);
        ResourceGroup InsertResourceGroup(ResourceGroup resourceGroup);
        int SaveChanges();
    }
}
