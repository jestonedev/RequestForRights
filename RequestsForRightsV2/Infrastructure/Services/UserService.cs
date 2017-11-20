using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Domain.Enums;
using RequestsForRights.Ldap;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using AclRole = RequestsForRights.Web.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Web.Infrastructure.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILdapRepository _ldapRepository;
        private readonly IUserSecurityService _securityRepository;

        public UserService(IUserRepository userRepository, 
            ILdapRepository ldapRepository, 
            IUserSecurityService securityService)
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

        public RequestUser FindUser(RequestUser requestUser)
        {
            return _userRepository.FindUser(requestUser);
        }

        public IEnumerable<RequestUser> FindUsers(string snpPattern, UsersCategory usersCategory, int maxCount)
        {
            var dbUsers = FilterUsersFields(FindDbUsers(snpPattern, usersCategory, maxCount));
            var ldapUsers = FilterUsersFields(FindActiveDirectoryUsers(snpPattern, usersCategory, maxCount));
            var maternityLeaveUsers = FilterUsersFields(_ldapRepository.FindMaternityLeaveUsers(snpPattern, GetLdapDepartmentFilter(),
                maxCount));
            switch (usersCategory)
            {
                case UsersCategory.ActiveUsers:
                case UsersCategory.All:
                    return ldapUsers.Concat(dbUsers).Concat(maternityLeaveUsers).OrderBy(r => r.Snp).Distinct().Take(10);
                case UsersCategory.BlockedUsers:
                    return ldapUsers.Concat(dbUsers).Except(maternityLeaveUsers).OrderBy(r => r.Snp).Distinct().Take(10);
                default:
                    throw new ArgumentOutOfRangeException("usersCategory", usersCategory, null);
            }
        }

        private IEnumerable<RequestUser> FindDbUsers(string snpPattern, UsersCategory usersCategory, int maxCount)
        {
            var users = _userRepository.FindUsers(snpPattern);

            switch (usersCategory)
            {
                case UsersCategory.All:
                    return users.Take(maxCount);
                case UsersCategory.ActiveUsers:
                    return users.Where(r => r.IsActive).Take(maxCount);
                case UsersCategory.BlockedUsers:
                    return users.Where(r => !r.IsActive).Take(maxCount);
                default:
                    throw new ArgumentOutOfRangeException("usersCategory");
            }
        }

        private IEnumerable<LdapUser> FindActiveDirectoryUsers(string snpPattern, UsersCategory usersCategory, int maxCount)
        {
            return _ldapRepository.FindUsers(snpPattern, usersCategory, GetLdapDepartmentFilter(), maxCount);
        }

        public IEnumerable<LdapUser> FindAllActiveDirectoryUsers(string snpPattern, int maxCount)
        {
            return _ldapRepository.FindUsers(snpPattern, UsersCategory.ActiveUsers, new List<LdapDepartmentFilter>
            {
                new LdapDepartmentFilter
                {
                    Company = "*"
                }
            }, maxCount);
        }

        public IEnumerable<Department> GetUnits()
        {
            return _securityRepository.FilterDepartments(_userRepository.GetUnits());
        }

        public IEnumerable<Department> GetDepartments()
        {
            return _securityRepository.FilterDepartments(_userRepository.GetDepartments());
        }

        private static IEnumerable<RequestUser> FilterUsersFields(IEnumerable<RequestUser> users)
        {
            return users.Select(r => new RequestUser
            {
                Login = r.Login, Snp = r.Snp, Post = r.Post, Department = r.Department, Unit = r.Unit, Office = r.Office, Phone = r.Phone
            });
        }

        private static IEnumerable<RequestUser> FilterUsersFields(IEnumerable<LdapUser> users)
        {
            return users.Select(r => new RequestUser
            {
                Login = r.Login, Snp = r.Snp, Post = r.Post, Department = r.Department, Unit = r.Unit, Office = r.Office, Phone = r.Phone
            });
        }

        public IEnumerable<LdapDepartmentFilter> GetLdapDepartmentFilter()
        {
            if (_securityRepository.InRole(new[]
            {
                AclRole.Administrator, AclRole.Dispatcher, AclRole.Executor, AclRole.Registrar, AclRole.ResourceManager,
            }))
            {
                return new List<LdapDepartmentFilter>
                {
                    new LdapDepartmentFilter
                    {
                        Company = "*"
                    }
                };
            }
            var allowedDepartments = _securityRepository.GetUserAllowedDepartments();
            var ldapCompanies = allowedDepartments.Where(r => r.ParentDepartment == null).Select(r => new LdapDepartmentFilter
            {
                Company = r.Name
            });
            var ldapDepartments = allowedDepartments.Where(r => r.ParentDepartment != null).Select(r => new LdapDepartmentFilter
            {
                Company = r.ParentDepartment.Name, Department = r.Name
            });
            var ldapDepartmentFilter = ldapCompanies.Concat(ldapDepartments).ToList();
            if (ldapDepartmentFilter.Any()) return ldapDepartmentFilter;
            var userInfo = _securityRepository.GetUserInfo();
            if (userInfo == null)
            {
                return new List<LdapDepartmentFilter>();
            }
            var userDepartment = userInfo.Department;
            ldapDepartmentFilter = new List<LdapDepartmentFilter>
            {
                new LdapDepartmentFilter
                {
                    Company = userDepartment.ParentDepartment == null ? userDepartment.Name : userDepartment.ParentDepartment.Name, Department = userDepartment.ParentDepartment != null ? userDepartment.Name : null
                }
            };
            return ldapDepartmentFilter;
        }
    }
}