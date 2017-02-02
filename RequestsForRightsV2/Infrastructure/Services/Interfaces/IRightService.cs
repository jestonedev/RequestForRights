using System;
using System.Collections.Generic;
using RequestsForRights.Models.Models;

namespace RequestsForRights.Infrastructure.Services.Interfaces
{
    public interface IRightService
    {
        IEnumerable<ResourceUserRightModel> GetPermanentRightsOnDate(DateTime date,
            int? idRequestUser, int? idResource);
        IEnumerable<ResourceUserRightModel> GetDelegatedRightsOnDate(DateTime date, 
            int? idRequestUser, int? idResource);
        IEnumerable<ResourceUserRightModel> GetRightsOnDate(DateTime date,
            int? idRequestUser, int? idResource);
        IEnumerable<ResourceUserRightModel> GetUserRightsOnDate(DateTime date,
            int? idRequestUser);
        IEnumerable<ResourceUserRightModel> GetResourceRightsOnDate(DateTime date, int? idResource);
    }
}
