using System.Linq;
using RequestsForRights.Domain.Entities;
using AclRole = RequestsForRights.Web.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Web.Infrastructure.Security.Interfaces
{
    public interface ISecurityService<in T>
        where T: class
    {
        string CurrentUser { get; }     
        AclUser GetUserInfo();
        IQueryable<Department> GetUserAllowedDepartments(AclUser user = null);
        bool InRole(AclRole role);
        bool InRole(AclRole[] role);
        bool IsAnonimous();
        bool CanRead(T entity);
        bool CanUpdate(T entity);
        bool CanDelete(T entity);
        bool CanCreate(T entity);
        bool CanModify(T entity);
        bool CanRead();
        bool CanUpdate();
        bool CanDelete();
        bool CanCreate();
        bool CanModify();
        IQueryable<AclUser> GetUsersBy(AclRole role);
    }
}
