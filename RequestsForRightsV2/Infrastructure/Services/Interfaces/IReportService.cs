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
        IEnumerable<ResourceUserRightModel> GetResourceRightsOnDate(ReportResourceRightsOptions options);
        IEnumerable<Resource> GetResources();
    }
}
