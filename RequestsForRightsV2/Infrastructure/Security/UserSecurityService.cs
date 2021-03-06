﻿using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using AclRole = RequestsForRights.Web.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Web.Infrastructure.Security
{
    public class UserSecurityService : SecurityService<RequestUser>, IUserSecurityService
    {
        public UserSecurityService(ISecurityRepository securityRepository) : base(securityRepository)
        {
        }

        public IQueryable<RequestUser> FilterUsers(IQueryable<RequestUser> users)
        {
            if (InRole(new []
            {
                AclRole.Administrator, AclRole.Dispatcher, 
                AclRole.Executor, AclRole.Registrar, 
                AclRole.ResourceManager
            }))
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
                                      (r.ParentDepartment != null && 
                                      department == r.ParentDepartment.IdDepartment)));
        }

        public override bool CanRead(RequestUser requestUser)
        {
            return FilterUsers(new List<RequestUser> {requestUser}.AsQueryable()).Any();
        }
    }
}