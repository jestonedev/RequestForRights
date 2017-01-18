﻿using System;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Ldap;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Infrastructure.Security.Interfaces;
using AclRole = RequestsForRights.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Infrastructure.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILdapRepository _ldapRepository;
        private readonly IUserSecurityService _securityRepository;

        public UserService(IUserRepository userRepository, ILdapRepository ldapRepository, IUserSecurityService securityService)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException("userRepository");
            }
            _userRepository = userRepository;
            if (ldapRepository == null)
            {
                throw new ArgumentNullException("ldapRepository");
            }
            _ldapRepository = ldapRepository;
            if (securityService == null)
            {
                throw new ArgumentNullException("securityService");
            }
            _securityRepository = securityService;
        }

        public IEnumerable<RequestUser> FindUsers(string snpPattern, int maxCount)
        {
            var dbUsers = _securityRepository.FilterUsers(
                _userRepository.FindUsers(snpPattern)).Take(maxCount);
            var ldapUsers = _ldapRepository.FindUsers(snpPattern,
                GetLdapDepartmentFilter(), maxCount);
            return ldapUsers.Select(r => new RequestUser
            {
                Login = r.Login,
                Snp = r.DisplayName,
                Post = r.Post,
                Department = r.Company,
                Unit = r.Department,
                Office = r.Office,
                Phone = r.Phone
            }).Concat(dbUsers).OrderBy(r => r.Snp).Take(10);
        }

        public IEnumerable<LdapDepartmentFilter> GetLdapDepartmentFilter()
        {
            if (_securityRepository.InRole(AclRole.Administrator))
            {
                return new List<LdapDepartmentFilter>{
                    new LdapDepartmentFilter
                    {
                        Company = "*"
                    }
                };
            }
            var allowedDepartments = _securityRepository.GetUserAllowedDepartments();
            var ldapCompanies = allowedDepartments.Where(r => r.ParentDepartment == null).
                Select(r => new LdapDepartmentFilter
                {
                    Company = r.Name
                });
            var ldapDepartments = allowedDepartments.Where(r => r.ParentDepartment != null).
                Select(r => new LdapDepartmentFilter
                {
                    Company = r.ParentDepartment.Name,
                    Department = r.Name
                });
            var ldapDepartmentFilter = ldapCompanies.Concat(ldapDepartments).ToList();
            if (ldapDepartmentFilter.Any()) return ldapDepartmentFilter;
            var userDepartment = _securityRepository.GetUserInfo().Department;
            ldapDepartmentFilter = new List<LdapDepartmentFilter>
            {
                new LdapDepartmentFilter
                {
                    Company = userDepartment.ParentDepartment == null ? userDepartment.Name : userDepartment.ParentDepartment.Name,
                    Department = userDepartment.ParentDepartment != null ? userDepartment.Name : null
                }
            };
            return ldapDepartmentFilter;
        }
    }
}