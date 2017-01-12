﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
            var resourceGroup = GetResourceGroupById(idResourceGroup);
            if (resourceGroup == null) return null;
            if (resourceGroup.Resources.Any(r => !r.Deleted))
            {
                throw new DbUpdateException("Не удалось удалить категорию ресурсов, т.к. она имеет зависимые ресурсы");
            }
            resourceGroup.Deleted = true;
            return resourceGroup;
        }

        public ResourceGroup UpdateResourceGroup(ResourceGroup resourceGroup)
        {
            var resGroup = _databaseContext.ResourceGroups.Attach(resourceGroup);
            _databaseContext.Entry(resGroup).State = EntityState.Modified;
            return resGroup;
        }

        public ResourceGroup InsertResourceGroup(ResourceGroup resourceGroup)
        {
            return _databaseContext.ResourceGroups.Add(resourceGroup);
        }

        public int SaveChanges()
        {
            return _databaseContext.SaveChanges();
        }

        public ResourceGroup GetResourceGroupById(int id)
        {
            return _databaseContext.ResourceGroups.FirstOrDefault(r => r.IdResourceGroup == id);
        }
    }
}
