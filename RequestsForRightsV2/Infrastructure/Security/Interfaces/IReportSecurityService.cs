using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.Models;

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
        bool CanReadResourceOperatorInfo();
        bool CanVisiblieAllDepartmentsMark();
        IEnumerable<ResourceUserRightModel> FilterResourceRights(IEnumerable<ResourceUserRightModel> resourceRights);
    }
}
