using System.Collections.Generic;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories.Interfaces
{
    public interface IResourceGroupRepository
    {
        IEnumerable<ResourceGroup> GetResourceGroups();
        ResourceGroup DeleteResourceGroup(int idResourceGroup);
        int SaveChanges();

        ResourceGroup GetResourceGroupById(int id);

        ResourceGroup UpdateResourceGroup(ResourceGroup resourceGroup);

        ResourceGroup InsertResourceGroup(ResourceGroup resourceGroup);
    }
}
