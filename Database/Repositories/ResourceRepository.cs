using System;
using System.Collections.Generic;
using System.Data.Entity;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using System.Linq;

namespace RequestsForRights.Database.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public ResourceRepository(IDatabaseContext databaseContext)
        {
            if (databaseContext == null)
            {
                throw new ArgumentNullException("databaseContext");
            }
            _databaseContext = databaseContext;
        }

        public IEnumerable<Resource> GetResources()
        {
            return _databaseContext.Resources.Include(r => r.ResourceGroup).Where(r => !r.Deleted);
        }

        public Resource DeleteResource(int idResource)
        {
            var resource = GetResourceById(idResource);
            if (resource == null) return null;
            resource.Deleted = true;
            return resource;
        }

        public int SaveChanges()
        {
            return _databaseContext.SaveChanges();
        }

        public Resource GetResourceById(int id)
        {
            return _databaseContext.Resources.FirstOrDefault(r => r.IdResource == id);
        }

        public Resource UpdateResource(Resource resource)
        {
            var res = _databaseContext.Resources.Find(resource.IdResource);
            _databaseContext.Entry(res).CurrentValues.SetValues(resource);
            UpdateResoureceRights(res.ResourceRights, resource.ResourceRights, resource.IdResource);
            return res;
        }

        private void UpdateResoureceRights(IEnumerable<ResourceRight> oldRights, IEnumerable<ResourceRight> newRights, int idResource)
        {
            var newRightsList = newRights.ToList();
            newRightsList.ForEach(r => r.IdResource = idResource);
            foreach (var resourceRight in oldRights)
            {
                if (newRightsList.Any(r => r.IdResourceRight == resourceRight.IdResourceRight)) continue;
                resourceRight.Deleted = true;
                _databaseContext.ResourceRights.Attach(resourceRight);
                _databaseContext.Entry(resourceRight).State = EntityState.Modified;
            }

            foreach (var resourceRight in newRightsList)
            {
                if (resourceRight.IdResourceRight == default(int))
                {
                    _databaseContext.ResourceRights.Add(resourceRight);
                }
                else
                {
                    var resRight = _databaseContext.ResourceRights.Find(resourceRight.IdResourceRight);
                    _databaseContext.Entry(resRight).CurrentValues.SetValues(resourceRight);
                }
            }
        }

        public Resource InsertResource(Resource resource)
        {
            return _databaseContext.Resources.Add(resource);
        }


        public IEnumerable<ResourceGroup> GetResourceGroups()
        {
            return _databaseContext.ResourceGroups.Where(r => !r.Deleted);
        }

        public IEnumerable<Department> GetOwnerDepartments()
        {
            return _databaseContext.Departments.Where(r => r.IdParentDepartment == null).Where(r => !r.Deleted);
        }
    }
}
