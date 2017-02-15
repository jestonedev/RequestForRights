using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Web.Infrastructure.Security.Interfaces
{
    public interface IUserSecurityService: ISecurityService<RequestUser>
    {
        IQueryable<RequestUser> FilterUsers(IQueryable<RequestUser> users);
        IQueryable<Department> FilterDepartments(IQueryable<Department> departments);
    }
}
