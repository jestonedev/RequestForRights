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
            var res = GetResourceById(resource.IdResource);
            res.Name = resource.Name;
            res.Description = resource.Description;
            res.IdResourceGroup = resource.IdResourceGroup;
            res.IdDepartment = resource.IdDepartment;
            // TODO: save resource rights
            var oldRights = res.ResourceRights.ToList();
            var newRights = resource.ResourceRights.ToList();
            foreach (var right in oldRights)
            {
                if (!newRights.Contains(right))
                {
                    res.ResourceRights.Remove(right);
                }
            }
            return res;
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
