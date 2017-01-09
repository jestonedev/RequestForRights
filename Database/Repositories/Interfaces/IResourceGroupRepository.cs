using System.Collections.Generic;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories.Interfaces
{
    public interface IResourceGroupRepository
    {
        IEnumerable<ResourceGroup> GetResourceGroups();
        ResourceGroup DeleteResourceGroup(int idResourceGroup);
        int SaveChanges();
    }
}
