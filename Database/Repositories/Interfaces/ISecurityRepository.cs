using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories.Interfaces
{
    public interface ISecurityRepository
    {
        IQueryable<AclRole> GetUserRoles(string login);
        AclUser GetUserInfo(string login);
        IQueryable<Department> GetUserAllowedDepartments(string login);
    }
}
