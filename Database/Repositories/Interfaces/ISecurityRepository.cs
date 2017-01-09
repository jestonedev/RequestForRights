using System.Collections.Generic;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories.Interfaces
{
    public interface ISecurityRepository
    {
        IEnumerable<AclRole> GetUserRoles(string login);
        IEnumerable<Department> GetUserAllowedDepartments(string login);
    }
}
