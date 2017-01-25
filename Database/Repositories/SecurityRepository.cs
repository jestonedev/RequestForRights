using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RequestsForRights.CachePool;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories
{
    public class SecurityRepository: ISecurityRepository
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly ICachePool _cachePool;

        public SecurityRepository(IDatabaseContext databaseContext, ICachePool cachePool)
        {
            if (databaseContext == null)
            {
                throw new ArgumentNullException("databaseContext");
            }
            _databaseContext = databaseContext;
            if (cachePool == null)
            {
                throw new ArgumentNullException("cachePool");
            }
            _cachePool = cachePool;
        }

        public AclUser GetUserInfo(string login)
        {
            if (_cachePool.HasValue<AclUser>(login))
            {
                return _cachePool.GetValue<AclUser>(login);
            }
            var user = _databaseContext.AclUsers.Include(r => r.AclDepartments)
                .Include(r => r.Roles)
                .FirstOrDefault(r => !r.Deleted && r.Login.ToLower() == login.ToLower());
            _cachePool.SetValue(login, user);
            return user;
        }

        public IQueryable<AclRole> GetUserRoles(string login)
        {
            var userInfo = GetUserInfo(login);
            return userInfo == null ? new List<AclRole>().AsQueryable() :
                userInfo.Roles.AsQueryable();
        }

        public IQueryable<Department> GetUserAllowedDepartments(string login)
        {
            var userInfo = GetUserInfo(login);
            return userInfo == null ? new List<Department>().AsQueryable() :
                userInfo.AclDepartments.Where(r => !r.Deleted).AsQueryable();
        }
    }
}
