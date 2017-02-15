using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
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

        public IEnumerable<RequestUser> FindUsers(string snpPattern, int maxCount)
        {
            var dbUsers = FilterUsersFields(FindDbUsers(snpPattern, maxCount));
            var ldapUsers = FilterUsersFields(FindActiveDirectoryUsers(snpPattern, maxCount));
            var result = ldapUsers.Concat(dbUsers).OrderBy(r => r.Snp).Distinct().Take(10);
            return result;
        }

        private IEnumerable<RequestUser> FindDbUsers(string snpPattern, int maxCount)
        {
            var users = _userRepository.FindUsers(snpPattern);

            var lastStatesByRequest = from stateRow in _userRepository.GetRequestStates()
                                      group stateRow.IdRequestState by stateRow.IdRequest
                                          into gs
                                          select new
                                          {
                                              IdRequest = gs.Key,
                                              IdRequestState = gs.Max()
                                          };
            var completedRequests = from lastStateRow in lastStatesByRequest
                join stateRow in _userRepository.GetRequestStates()
                    on lastStateRow.IdRequestState equals stateRow.IdRequestState
                join requestRow in _userRepository.GetRequests()
                    on stateRow.IdRequest equals requestRow.IdRequest
                join userAssocRow in _userRepository.GetRequestUserAssocs()
                    on requestRow.IdRequest equals userAssocRow.IdRequest
                where stateRow.IdRequestStateType == 4
                select new
                {
                    requestRow.IdRequest,
                    requestRow.IdRequestType,
                    userAssocRow.IdRequestUser,
                    stateRow.Date
                };

            var lastRequestsDateForUsers = from row in completedRequests
                group row.Date by row.IdRequestUser
                into gs
                select new
                {
                    Date = gs.Max(),
                    IdRequestUser = gs.Key
                };

            completedRequests = from requestRow in completedRequests
                join lrRow in lastRequestsDateForUsers
                    on new {requestRow.IdRequestUser, requestRow.Date} equals
                    new {lrRow.IdRequestUser, lrRow.Date}
                where requestRow.IdRequestType == 3
                select new
                {
                    requestRow.IdRequest,
                    requestRow.IdRequestType,
                    requestRow.IdRequestUser,
                    requestRow.Date
                };

            var excludeUsers = from request in completedRequests
                join userAssoc in _userRepository.GetRequestUserAssocs()
                    on request.IdRequest equals userAssoc.IdRequest
                select userAssoc.RequestUser;

            users = users.Except(excludeUsers);

            return _securityRepository.FilterUsers(users).Take(maxCount);
        }

        private IEnumerable<LdapUser> FindActiveDirectoryUsers(string snpPattern, int maxCount)
        {
            return _ldapRepository.FindUsers(snpPattern,
                GetLdapDepartmentFilter(), maxCount);
        }

        public IEnumerable<LdapUser> FindAllActiveDirectoryUsers(string snpPattern, int maxCount)
        {
            return _ldapRepository.FindUsers(snpPattern,
                new List<LdapDepartmentFilter>
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
                Login = r.Login,
                Snp = r.Snp,
                Post = r.Post,
                Department = r.Department,
                Unit = r.Unit,
                Office = r.Office,
                Phone = r.Phone
            });
        }

        private static IEnumerable<RequestUser> FilterUsersFields(IEnumerable<LdapUser> users)
        {
            return users.Select(r => new RequestUser
            {
                Login = r.Login,
                Snp = r.Snp,
                Post = r.Post,
                Department = r.Department,
                Unit = r.Unit,
                Office = r.Office,
                Phone = r.Phone
            });
        }

        public IEnumerable<LdapDepartmentFilter> GetLdapDepartmentFilter()
        {
            if (_securityRepository.InRole(new[]
            {
                AclRole.Administrator, AclRole.Dispatcher, 
                AclRole.Executor, AclRole.Registrar, 
                AclRole.ResourceManager, 
            }))
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
                    Company = userDepartment.ParentDepartment == null ? userDepartment.Name : userDepartment.ParentDepartment.Name,
                    Department = userDepartment.ParentDepartment != null ? userDepartment.Name : null
                }
            };
            return ldapDepartmentFilter;
        }
    }
}