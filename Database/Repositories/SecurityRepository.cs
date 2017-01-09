using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories
{
    public class SecurityRepository: ISecurityRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public SecurityRepository(IDatabaseContext databaseContext)
        {
            if (databaseContext == null)
            {
                throw new ArgumentNullException("databaseContext");
            }
            _databaseContext = databaseContext;
        }

        public AclUser GetUserInfo(string login)
        {
            return _databaseContext.AclUsers.FirstOrDefault(r => r.Login.ToLower() == login.ToLower());
        }

        public IEnumerable<AclRole> GetUserRoles(string login)
        {
            var user = GetUserInfo(login);
            return user != null ? user.Roles : new List<AclRole>();
        }

        public IEnumerable<Department> GetUserAllowedDepartments(string login)
        {
            var user = GetUserInfo(login);
            return user != null ? user.AclDepartments : new List<Department>();
        }
    }
}
