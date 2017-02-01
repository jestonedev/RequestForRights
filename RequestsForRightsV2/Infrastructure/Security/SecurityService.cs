using System;
using System.Linq;
using System.Web;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Security.Interfaces;
using AclRole = RequestsForRights.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Infrastructure.Security
{
    public class SecurityService<T>: ISecurityService<T>
        where T: class
    {
        private readonly ISecurityRepository _securityRepository;

        public SecurityService(ISecurityRepository securityRepository)
        {
            if (securityRepository == null)
            {
                throw new ArgumentNullException("securityRepository");
            }
            _securityRepository = securityRepository;
        }

        public string CurrentUser
        {
            get
            {
                return HttpContext.Current != null
                       && HttpContext.Current.User != null
                       && HttpContext.Current.User.Identity != null
                    ? HttpContext.Current.User.Identity.Name
                    : null;
            }
        }

        public IQueryable<Department> GetUserAllowedDepartments(AclUser user = null)
        {
            if (user == null)
            {
                user = GetUserInfo();
            }
            if (user == null)
            {
                throw new ApplicationException("Неизвестный пользователь");
            }
            var allowedDepartments = _securityRepository.GetUserAllowedDepartments(user.Login);
            if (allowedDepartments.Any()) return allowedDepartments;
            allowedDepartments = allowedDepartments.ToList().Concat(new[]
            {
                user.Department
            }).AsQueryable();
            return allowedDepartments;
        }

        public IQueryable<AclUser> GetUsersBy(AclRole role)
        {
            return _securityRepository.GetUsersBy((int) role);
        }

        public AclUser GetUserInfo()
        {
            return _securityRepository.GetUserInfo(CurrentUser);
        }

        public bool InRole(AclRole role)
        {
            return GetUserRoles().Any(r => r.IdRole == (int)role);
        }

        public bool InRole(AclRole[] roles)
        {
            return GetUserRoles().Any(r => roles.Any(role => r.IdRole == (int)role));
        }

        public virtual bool IsAnonimous()
        {
            return !GetUserRoles().Any();
        }

        private IQueryable<Domain.Entities.AclRole> GetUserRoles()
        {
            return _securityRepository.GetUserRoles(CurrentUser);
        }

        public virtual bool CanRead(T entity)
        {
            return CanRead();
        }

        public virtual bool CanUpdate(T entity)
        {
            return CanUpdate();
        }

        public virtual bool CanDelete(T entity)
        {
            return CanDelete();
        }

        public virtual bool CanCreate(T entity)
        {
            return CanCreate();
        }

        public virtual bool CanModify(T entity)
        {
            return CanCreate(entity) && CanDelete(entity) && CanUpdate(entity);
        }

        public virtual bool CanRead()
        {
            return false;
        }

        public virtual bool CanUpdate()
        {
            return false;
        }

        public virtual bool CanDelete()
        {
            return false;
        }

        public virtual bool CanCreate()
        {
            return false;
        }

        public virtual bool CanModify()
        {
            return CanCreate() && CanDelete() && CanUpdate();
        }
    }
}