using System;
using System.Linq;
using System.Web;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRightsV2.Infrastructure.Enums;
using RequestsForRightsV2.Infrastructure.Security.Interfaces;

namespace RequestsForRightsV2.Infrastructure.Security
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

        public bool InRole(AclRole role)
        {
            return _securityRepository.GetUserRoles(CurrentUser).Any(r => r.IdRole == (int) role);
        }

        public bool InRole(AclRole[] roles)
        {
            return _securityRepository.GetUserRoles(CurrentUser).
                Any(r => roles.Any(role => r.IdRole == (int)role));
        }

        public virtual bool IsAnonimous()
        {
            return !_securityRepository.GetUserRoles(CurrentUser).Any();
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