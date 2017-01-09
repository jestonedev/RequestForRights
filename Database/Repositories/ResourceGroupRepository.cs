using System;
using System.Collections.Generic;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using System.Linq;

namespace RequestsForRights.Database.Repositories
{
    public class ResourceGroupRepository : IResourceGroupRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public ResourceGroupRepository(IDatabaseContext databaseContext)
        {
            if (databaseContext == null)
            {
                throw new ArgumentNullException("databaseContext");
            }
            _databaseContext = databaseContext;
        }

        public IEnumerable<ResourceGroup> GetResourceGroups()
        {
            return _databaseContext.ResourceGroups.Where(r => !r.Deleted);
        }

        public ResourceGroup DeleteResourceGroup(int idResourceGroup)
        {
            var resourceGroup = _databaseContext.ResourceGroups.FirstOrDefault(
                r => r.IdResourceGroup == idResourceGroup);
            return _databaseContext.ResourceGroups.Remove(resourceGroup);
        }

        public int SaveChanges()
        {
            return _databaseContext.SaveChanges();
        }
    }
}
