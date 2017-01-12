using System.Collections.Generic;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        IEnumerable<ResourceGroup> GetResourceGroups();
        IEnumerable<Department> GetOwnerDepartments();
        IEnumerable<Resource> GetResources();
        Resource DeleteResource(int idResource);
        Resource GetResourceById(int id);
        Resource UpdateResource(Resource resource);
        Resource InsertResource(Resource resource);
        int SaveChanges();
    }
}
