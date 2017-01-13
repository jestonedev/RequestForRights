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
        private readonly Dictionary<string, AclUser> _usersCache = new Dictionary<string, AclUser>();

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
            if (_usersCache.ContainsKey(login))
            {
                return _usersCache[login];
            }
            return _usersCache[login] = 
                _databaseContext.AclUsers.Include(r => r.AclDepartments)
                .Include(r => r.Roles)
                .FirstOrDefault(r => r.Login.ToLower() == login.ToLower());
        }

        public IQueryable<AclRole> GetUserRoles(string login)
        {
            return GetUserInfo(login).Roles.AsQueryable();
        }

        public IQueryable<Department> GetUserAllowedDepartments(string login)
        {
            return GetUserInfo(login).AclDepartments.AsQueryable();
        }
    }
}
