using System;
using System.Collections.Generic;
using RequestsForRights.Web.Models.Models;

namespace RequestsForRights.Web.Infrastructure.Services.Interfaces
{
    public interface IRightService
    {
        IEnumerable<ResourceUserRightModel> GetPermanentRightsOnDate(DateTime date,
            int? idRequestUser, string department, int? idResource);
        IEnumerable<ResourceUserRightModel> GetDelegatedRightsOnDate(DateTime date, 
            int? idRequestUser, string department, int? idResource);
        IEnumerable<ResourceUserRightModel> GetRightsOnDate(DateTime date,
            int? idRequestUser, string department, int? idResource);
        IEnumerable<ResourceUserRightModel> GetUserRightsOnDate(DateTime date, 
            int? idRequestUser);
        IEnumerable<ResourceUserRightModel> GetResourceRightsOnDate(DateTime date, int? idResource);
        IEnumerable<ResourceUserRightModel> GetDepartmentRightsOnDate(DateTime date, string department);
        IEnumerable<ResourceUserRightModel> GetDepartmentAndResourceRightsOnDate(DateTime date, string department, int? idResource);
        IEnumerable<ResourceUserRightHistoryModel> GetUserRightsHistoryOnDate(DateTime from, DateTime to, int idRequestUser);
    }
}
