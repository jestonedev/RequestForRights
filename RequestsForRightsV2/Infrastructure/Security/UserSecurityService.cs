using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Security.Interfaces;
using AclRole = RequestsForRights.Infrastructure.Enums.AclRole;
using System;

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
                if (userInfo != null)
                {
                    var department = userInfo.Department.ParentDepartment == null
                        ? userInfo.Department.Name
                        : userInfo.Department.ParentDepartment.Name;
                    allowedDepartments.Add(department);
                }
            }
            return users.Where(r => allowedDepartments.Any(ad => ad == r.Department) ||
                allowedUnits.Any(au => au == r.Department + "@" + r.Unit));
        }

        public IQueryable<Department> FilterDepartments(IQueryable<Department> departments)
        {
            if (InRole(AclRole.Administrator))
            {
                return departments;
            }
            var allowedDepartments = GetUserAllowedDepartments().Select(r => r.IdDepartment).ToList();
            var userInfo = GetUserInfo();
            if (userInfo == null)
            {
                throw new ApplicationException("Неизвестный пользователь");
            }
            if (!allowedDepartments.Any())
            {
                allowedDepartments.Add(userInfo.IdDepartment);
            }
            return departments.Where(r =>
                allowedDepartments.
                    Any(department => department == r.IdDepartment ||
                                      (r.ParentDepartment != null && department == r.ParentDepartment.IdDepartment)));
        }
    }
}