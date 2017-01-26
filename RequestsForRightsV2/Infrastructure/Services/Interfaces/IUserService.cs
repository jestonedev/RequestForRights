using RequestsForRights.Domain.Entities;
using System.Collections.Generic;
using RequestsForRights.Ldap;

namespace RequestsForRights.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        RequestUser FindUser(RequestUser requestUser);
        IEnumerable<RequestUser> FindUsers(string snpPattern, int maxCount);
        IEnumerable<Department> GetUnits();
        IEnumerable<Department> GetDepartments();
        IEnumerable<LdapUser> FindAllActiveDirectoryUsers(string snpPattern, int maxCount);
    }
}