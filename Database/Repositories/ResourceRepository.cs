using System;
using System.Collections.Generic;
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
            return _databaseContext.Resources.Where(r => !r.Deleted);
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
            var res = GetResourceById(resource.IdResourceGroup);
            res.Name = resource.Name;
            res.Description = resource.Description;
            res.IdResourceGroup = resource.IdResourceGroup;
            res.IdDepartment = resource.IdDepartment;
            return res;
        }

        public Resource InsertResource(Resource resource)
        {
            return _databaseContext.Resources.Add(resource);
        }
    }
}
