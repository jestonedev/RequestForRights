using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        IQueryable<ResourceGroup> GetResourceGroups();
        IQueryable<Department> GetOwnerDepartments();
        IQueryable<Resource> GetResources();
        IQueryable<ResourceRight> GetResourceRights();
        Resource DeleteResource(int idResource);
        Resource GetResourceById(int id);
        Resource UpdateResource(Resource resource);
        Resource InsertResource(Resource resource);
        int SaveChanges();
    }
}
