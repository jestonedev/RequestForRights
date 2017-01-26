using System;
using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.Models;

namespace RequestsForRights.Infrastructure.Services.Interfaces
{
    public interface IReportService
    {
        RequestUser FindUser(RequestUser requestUser);
        IEnumerable<ResourceUserRightModel> GetUserRightsOnDate(DateTime date, int idRequestUser);
        IEnumerable<ResourceUserRightModel> GetResourceRightsOnDate(DateTime date, int idResource);
        IEnumerable<Resource> GetResources();
    }
}
