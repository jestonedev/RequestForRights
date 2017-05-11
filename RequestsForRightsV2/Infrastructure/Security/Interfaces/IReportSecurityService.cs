using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Web.Infrastructure.Security.Interfaces
{
    public interface IReportSecurityService: ISecurityService<RequestUser>
    {
        bool CanReadResourcePermissions();
        bool CanReadUserPermissions();
        bool CanReadUserPermissions(RequestUser requestUser);
        IQueryable<Resource> FilterResources(IQueryable<Resource> resources);
        IQueryable<Department> FilterDepartments(IQueryable<Department> departments);

        bool CanReadDepartmentPermissions();
        bool CanReadDepartmentAndResourcePermissions();
    }
}
