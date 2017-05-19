using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Domain.Enums;
using RequestsForRights.Ldap;
using RequestsForRights.Web.Infrastructure.Enums;

namespace RequestsForRights.Web.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        RequestUser FindUser(RequestUser requestUser);
        IEnumerable<RequestUser> FindUsers(string snpPattern, UsersCategory usersCategory, int maxCount);
        IEnumerable<Department> GetUnits();
        IEnumerable<Department> GetDepartments();
        IEnumerable<LdapUser> FindAllActiveDirectoryUsers(string snpPattern, int maxCount);
    }
}