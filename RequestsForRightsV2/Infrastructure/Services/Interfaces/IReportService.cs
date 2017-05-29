using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ReportOptions;

namespace RequestsForRights.Web.Infrastructure.Services.Interfaces
{
    public interface IReportService
    {
        RequestUser FindUser(ReportUserRightsOptions options);
        IEnumerable<ResourceUserRightModel> GetUserRightsOnDate(ReportUserRightsOptions options, int idRequestUser);
        IEnumerable<ResourceUserRightHistoryModel> GetUserRightsHistoryOnDate(ReportUserRightsHistoryOptions options, int idRequestUser);
        IEnumerable<ResourceUserRightModel> GetDepartmentRightsOnDate(ReportDepartmentRightsOptions options);
        IEnumerable<ResourceUserRightModel> GetResourceRightsOnDate(ReportResourceRightsOptions options);
        IEnumerable<ResourceUserRightModel> GetDepartmentAndResourceRightsOnDate(ReportDepartmentAndResourceRightsOptions options);
        IEnumerable<Resource> GetResources();
        IEnumerable<Department> GetAllowedDepartments();
        IEnumerable<Department> GetAllDepartments();
        IEnumerable<ResourceOperatorModel> GetResourceOperatorInfo(int? idDepartment);
    }
}
