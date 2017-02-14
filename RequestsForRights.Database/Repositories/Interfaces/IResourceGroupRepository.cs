using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories.Interfaces
{
    public interface IResourceGroupRepository
    {
        IQueryable<ResourceGroup> GetResourceGroups();
        ResourceGroup DeleteResourceGroup(int idResourceGroup);

        ResourceGroup GetResourceGroupById(int id);

        ResourceGroup UpdateResourceGroup(ResourceGroup resourceGroup);

        ResourceGroup InsertResourceGroup(ResourceGroup resourceGroup);
        int SaveChanges();
    }
}
