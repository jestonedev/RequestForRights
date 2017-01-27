using System;
using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ReportOptions;

namespace RequestsForRights.Infrastructure.Services.Interfaces
{
    public interface IReportService
    {
        RequestUser FindUser(ReportUserRightsOptions options);
        IEnumerable<ResourceUserRightModel> GetUserRightsOnDate(ReportUserRightsOptions options, int idRequestUser);
        IEnumerable<ResourceUserRightModel> GetResourceRightsOnDate(ReportResourceRightsOptions options);
        IEnumerable<Resource> GetResources();
    }
}
