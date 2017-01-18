using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Security.Interfaces;
using AclRole = RequestsForRights.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Infrastructure.Security
{
    public class UserSecurityService : SecurityService<RequestUser>, IUserSecurityService
    {
        public UserSecurityService(ISecurityRepository securityRepository) : base(securityRepository)
        {
        }

        public IQueryable<RequestUser> FilterUsers(IQueryable<RequestUser> users)
        {
            if (InRole(AclRole.Administrator))
            {
                return users;
            }
            var allowedDepartments = GetUserAllowedDepartments().Where(r => r.ParentDepartment == null).
                Select(r => r.Name).ToList();
            var allowedUnits = GetUserAllowedDepartments().Where(r => r.ParentDepartment != null).
                Select(r => r.ParentDepartment.Name + "@" + r.Name).ToList();
            if (!allowedDepartments.Any() && !allowedUnits.Any())
            {
                var userInfo = GetUserInfo();
                var department = userInfo.Department.ParentDepartment == null ? userInfo.Department.Name :
                    userInfo.Department.ParentDepartment.Name; 
                allowedDepartments.Add(department);
            }
            return users.Where(r => allowedDepartments.Any(ad => ad == r.Department) ||
                allowedUnits.Any(au => au == r.Department + "@" + r.Unit));
        }
    }
}