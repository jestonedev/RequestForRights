using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.Models;

namespace RequestsForRights.Web.Infrastructure.Services
{
    public class RightService: IRightService
    {
        private readonly IRightRepository _rightRepository;

        public RightService(IRightRepository rightRepository)
        {
            if (rightRepository == null)
            {
                throw new ArgumentNullException("rightRepository");
            }
            _rightRepository = rightRepository;
        }

        public IEnumerable<ResourceUserRightModel> GetPermanentRightsOnDate(DateTime date, 
            int? idRequestUser, string department, string unit, int? idResource)
        {
            var lastStatesByRequest = from stateRow in _rightRepository.GetRequestStates()
                group stateRow.IdRequestState by stateRow.IdRequest
                into gs
                select new
                {
                    IdRequest = gs.Key,
                    IdRequestState = gs.Max()
                };
            var completedRequests = from lastStateRow in lastStatesByRequest
                join stateRow in _rightRepository.GetRequestStates()
                    on lastStateRow.IdRequestState equals stateRow.IdRequestState
                join requestRow in _rightRepository.GetRequests()
                    on stateRow.IdRequest equals requestRow.IdRequest
                where stateRow.IdRequestStateType == 4
                // Completed
                select new
                {
                    requestRow.IdRequest,
                    requestRow.IdRequestType,
                    DateFrom = stateRow.Date,
                };
            var completedUsers = (from completedRequestRow in completedRequests
                join requestUserAssocRow in _rightRepository.GetRequestUserAssocs()
                    on completedRequestRow.IdRequest equals requestUserAssocRow.IdRequest
                join requestUserRow in _rightRepository.GetRequestUsers()
                    on requestUserAssocRow.IdRequestUser equals  requestUserRow.IdRequestUser
                where (idRequestUser == null || requestUserAssocRow.IdRequestUser == idRequestUser.Value) &&
                    (department == null || requestUserRow.Department == department) &&
                    (unit == null || requestUserRow.Unit == unit)
                select new
                {
                    completedRequestRow.IdRequest,
                    completedRequestRow.IdRequestType,
                    completedRequestRow.DateFrom,
                    requestUserAssocRow.IdRequestUser,
                    requestUserAssocRow.IdRequestUserAssoc
                }).ToList();
            date = date.Date.AddDays(1).AddSeconds(-1);
            var excludedUserDates = from completedUserRow in completedUsers
                where completedUserRow.IdRequestType == 3 &&
                    completedUserRow.DateFrom <= date
                // Disconnect user
                group completedUserRow.DateFrom by completedUserRow.IdRequestUser
                into gs
                select new
                {
                    IdRequestUser = gs.Key,
                    DisconnectDate = gs.Max()
                };
            var changingUserAssocs = from completedUserRow in completedUsers
                join excludeUserDateRow in excludedUserDates
                    on completedUserRow.IdRequestUser equals excludeUserDateRow.IdRequestUser into exUser
                from exUserRow in exUser.DefaultIfEmpty()
                where new[] {1, 2}.Contains(completedUserRow.IdRequestType) &&
                      (exUserRow == null || exUserRow.DisconnectDate < completedUserRow.DateFrom) &&
                      completedUserRow.DateFrom <= date
                select completedUserRow;
            var changingUserRightsAssoc = (from userAssocRow in changingUserAssocs
                join rightAssocRow in _rightRepository.GetRequestUserRightAssocs()
                    on userAssocRow.IdRequestUserAssoc equals rightAssocRow.IdRequestUserAssoc
                where idResource == null || rightAssocRow.ResourceRight.IdResource == idResource.Value
                select new
                {
                    userAssocRow.IdRequestUser,
                    rightAssocRow.IdRequestRightGrantType,
                    rightAssocRow.IdResourceRight,
                    rightAssocRow.Descirption,
                    userAssocRow.DateFrom
                }).ToList();
            var lastRevokeRightDate = from rightAssocRow in changingUserRightsAssoc
                where rightAssocRow.IdRequestRightGrantType == 2
                group rightAssocRow.DateFrom by new
                {
                    rightAssocRow.IdRequestUser,
                    rightAssocRow.IdResourceRight
                }
                into gs
                select new
                {
                    gs.Key.IdRequestUser,
                    gs.Key.IdResourceRight,
                    RevokeDate = gs.Max()
                };
            var currentUserRights = from rightAssocRow in changingUserRightsAssoc
                join lastRevokeRightDateRow in lastRevokeRightDate
                    on new {rightAssocRow.IdRequestUser, rightAssocRow.IdResourceRight}
                    equals new {lastRevokeRightDateRow.IdRequestUser, lastRevokeRightDateRow.IdResourceRight} into
                    revDate
                from revDateRow in revDate.DefaultIfEmpty()
                where rightAssocRow.IdRequestRightGrantType == 1 &&
                      (revDateRow == null || revDateRow.RevokeDate < rightAssocRow.DateFrom)
                select rightAssocRow;
            var lastCurrentUserRights = from userRight in currentUserRights
                group new { userRight.DateFrom, userRight.Descirption } by new
                {
                    userRight.IdRequestUser,
                    userRight.IdResourceRight
                }
                into gs
                select new
                {
                    gs.Key.IdRequestUser,
                    gs.Key.IdResourceRight,
                    DateFrom = gs.Select(r => r.DateFrom).Min(),
                    Description = gs.OrderByDescending(r => r.DateFrom).Select(r => r.Descirption).FirstOrDefault()
                };
            return from userRight in lastCurrentUserRights
                   join requestUser in _rightRepository.GetRequestUsers()
                    on userRight.IdRequestUser equals requestUser.IdRequestUser
                join resourceRight in _rightRepository.GetResourceRights()
                    on userRight.IdResourceRight equals resourceRight.IdResourceRight
                select new ResourceUserRightModel
                {
                    IdRequestUser = requestUser.IdRequestUser,
                    RequestUserSnp = requestUser.Snp,
                    RequestUserDepartment = requestUser.Department,
                    RequestUserUnit = requestUser.Unit,
                    IdResourceRight = resourceRight.IdResourceRight,
                    ResourceRightName = resourceRight.Name,
                    ResourceRightDescription = resourceRight.Description,
                    IdResource = resourceRight.IdResource,
                    ResourceName = resourceRight.Resource.Name,
                    ResourceDescription = resourceRight.Resource.Description,
                    RightCategory = "Постоянное право",
                    DateFrom = userRight.DateFrom,
                    Description = userRight.Description
                };
        }

        public IEnumerable<ResourceUserRightModel> GetDelegatedRightsOnDate(DateTime date,
            int? idRequestUser, string department, string unit, int? idResource)
        {
            var lastStatesByRequest = from stateRow in _rightRepository.GetRequestStates()
                group stateRow.IdRequestState by stateRow.IdRequest
                into gs
                select new
                {
                    IdRequest = gs.Key,
                    IdRequestState = gs.Max()
                };
            var completedRequests = from lastStateRow in lastStatesByRequest
                join stateRow in _rightRepository.GetRequestStates()
                    on lastStateRow.IdRequestState equals stateRow.IdRequestState
                join requestRow in _rightRepository.GetRequests()
                    on stateRow.IdRequest equals requestRow.IdRequest
                where stateRow.IdRequestStateType == 4
                // Completed
                select new
                {
                    requestRow.IdRequest,
                    requestRow.IdRequestType,
                    DateFrom = stateRow.Date
                };
            var completedUsers = (from completedRequestRow in completedRequests
                join requestUserAssocRow in _rightRepository.GetRequestUserAssocs()
                    on completedRequestRow.IdRequest equals requestUserAssocRow.IdRequest
                select new
                {
                    completedRequestRow.IdRequest,
                    completedRequestRow.IdRequestType,
                    completedRequestRow.DateFrom,
                    requestUserAssocRow.IdRequestUser,
                    requestUserAssocRow.IdRequestUserAssoc
                }).ToList();
            date = date.Date.AddDays(1).AddSeconds(-1);
            var excludedUserDates = from completedUserRow in completedUsers
                where completedUserRow.IdRequestType == 3 &&
                      completedUserRow.DateFrom <= date
                // Disconnect user
                group completedUserRow.DateFrom by completedUserRow.IdRequestUser
                into gs
                select new
                {
                    IdRequestUser = gs.Key,
                    DisconnectDate = gs.Max()
                };
            var delegateUserAssocs = from completedUserRow in completedUsers
                join delegateExtInfoRow in _rightRepository.GetDelegationRequestUsersExtInfo()
                    on completedUserRow.IdRequestUserAssoc equals delegateExtInfoRow.IdRequestUserAssoc
                join delegateFromUserRow in _rightRepository.GetRequestUsers()
                    on completedUserRow.IdRequestUser equals delegateFromUserRow.IdRequestUser
                join delegateToUserRow in _rightRepository.GetRequestUsers()
                    on delegateExtInfoRow.IdDelegateToUser equals delegateToUserRow.IdRequestUser
                join excludeUserDateRow in excludedUserDates
                    on completedUserRow.IdRequestUser equals excludeUserDateRow.IdRequestUser into exUser
                from exUserRow in exUser.DefaultIfEmpty()
                where completedUserRow.IdRequestType == 4 &&
                      date >= delegateExtInfoRow.DelegateFromDate && 
                      date <= delegateExtInfoRow.DelegateToDate  &&
                      (exUserRow == null || 
                      exUserRow.DisconnectDate < delegateExtInfoRow.DelegateFromDate) &&
                      (idRequestUser == null || 
                      completedUserRow.IdRequestUser == idRequestUser.Value ||
                      delegateExtInfoRow.IdDelegateToUser == idRequestUser.Value) &&
                      (department == null ||
                      delegateToUserRow.Department == department ||
                      delegateFromUserRow.Department == department) &&
                      (unit == null ||
                      delegateToUserRow.Unit == unit ||
                      delegateFromUserRow.Unit == unit)
                select new
                {
                    completedUserRow.IdRequestUserAssoc,
                    IdDelegateFromUser = completedUserRow.IdRequestUser,
                    delegateExtInfoRow.IdDelegateToUser,
                    delegateExtInfoRow.DelegateFromDate,
                    delegateExtInfoRow.DelegateToDate
                };
            var delegateRights = from delegateUserAssocRow in delegateUserAssocs
                join rightAssocRow in _rightRepository.GetRequestUserRightAssocs()
                    on delegateUserAssocRow.IdRequestUserAssoc equals rightAssocRow.IdRequestUserAssoc
                where idResource == null || rightAssocRow.ResourceRight.IdResource == idResource.Value
                select new
                {
                    rightAssocRow.IdResourceRight,
                    delegateUserAssocRow.IdDelegateFromUser,
                    delegateUserAssocRow.IdDelegateToUser,
                    delegateUserAssocRow.DelegateFromDate,
                    delegateUserAssocRow.DelegateToDate
                };
            var lastDelegateRights = from delegateRight in delegateRights
                group new {delegateRight.DelegateFromDate, delegateRight.DelegateToDate} by new
                {
                    delegateRight.IdDelegateFromUser,
                    delegateRight.IdDelegateToUser,
                    delegateRight.IdResourceRight
                }
                into gs
                select new
                {
                    gs.Key.IdDelegateFromUser,
                    gs.Key.IdDelegateToUser,
                    gs.Key.IdResourceRight,
                    DelegateFromDate = gs.Select(r => r.DelegateFromDate).Min(),
                    DelegateToDate = gs.Select(r => r.DelegateToDate).Max()
                };
            return from delegateRight in lastDelegateRights
                   join delegateUserFrom in _rightRepository.GetRequestUsers()
                    on delegateRight.IdDelegateFromUser equals delegateUserFrom.IdRequestUser
                join delegateUserTo in _rightRepository.GetRequestUsers()
                    on delegateRight.IdDelegateToUser equals delegateUserTo.IdRequestUser
                join resourceRight in _rightRepository.GetResourceRights()
                    on delegateRight.IdResourceRight equals resourceRight.IdResourceRight
                select new ResourceUserRightModel
                {
                    IdRequestUser = delegateUserTo.IdRequestUser,
                    RequestUserSnp = delegateUserTo.Snp,
                    RequestUserDepartment = delegateUserTo.Department,
                    RequestUserUnit = delegateUserTo.Unit,
                    IdResourceRight = resourceRight.IdResourceRight,
                    ResourceRightName = resourceRight.Name,
                    ResourceRightDescription = resourceRight.Description,
                    IdResource = resourceRight.IdResource,
                    ResourceName = resourceRight.Resource.Name,
                    ResourceDescription = resourceRight.Resource.Description,
                    RightCategory = "Делегированное право",
                    IdDelegateFromUser = delegateUserFrom.IdRequestUser,
                    DelegateFromUserSnp = delegateUserFrom.Snp,
                    DelegateFromUserDepartment = delegateUserFrom.Department,
                    DelegateFromUserUnit = delegateUserFrom.Unit,
                    DateDelegateFrom = delegateRight.DelegateFromDate,
                    DateDelegateTo = delegateRight.DelegateToDate
                };
        }

        public IEnumerable<ResourceUserRightModel> GetRightsOnDate(DateTime date,
            int? idRequestUser, string department, string unit, int? idResource)
        {
            var permanentRights = GetPermanentRightsOnDate(date, idRequestUser, department, unit, idResource);
            var delegateRights = GetDelegatedRightsOnDate(date, idRequestUser, department, unit, idResource).ToList();
            delegateRights.ForEach(r =>
            {
                var permanentRight = permanentRights.FirstOrDefault(pr => pr.IdResourceRight == r.IdResourceRight);
                if (permanentRight != null)
                {
                    r.DateFrom = permanentRight.DateFrom;
                }
            });
            permanentRights = permanentRights.Where(pr =>
                !delegateRights.Any(
                    dr => pr.IdRequestUser == dr.IdDelegateFromUser &&
                          pr.IdResourceRight == dr.IdResourceRight));
            return permanentRights.Concat(delegateRights);
        }

        public IEnumerable<ResourceUserRightModel> GetUserRightsOnDate(DateTime date,
            int? idRequestUser)
        {
            return GetRightsOnDate(date, idRequestUser, null, null, null);
        }

        public IEnumerable<ResourceUserRightModel> GetResourceRightsOnDate(DateTime date, int? idResource)
        {
            return GetRightsOnDate(date, null, null, null, idResource);
        }

        public IEnumerable<ResourceUserRightModel> GetDepartmentRightsOnDate(DateTime date, string department, string unit)
        {
            return GetRightsOnDate(date, null, department, unit, null);
        }

        public IEnumerable<ResourceUserRightModel> GetDepartmentAndResourceRightsOnDate(DateTime date, string department, string unit, int? idResource)
        {
            return GetRightsOnDate(date, null, department, unit, idResource);
        }

        public IEnumerable<ResourceUserRightHistoryModel> GetUserRightsHistoryOnDate(DateTime from, DateTime to, int idRequestUser)
        {
            var userRights = _rightRepository.GetRequestUserRightAssocs()
                .Include(r => r.RequestUserAssoc)
                .Include(r => r.RequestUserAssoc.Request)
                .Include(r => r.RequestUserAssoc.Request.RequestStates)
                .Include(r => r.RequestUserAssoc.RequestUser)
                .Include(r => r.ResourceRight)
                .Include(r => r.RequestRightGrantType)
                .Include(r => r.ResourceRight.Resource)
                .Include(r => r.RequestUserAssoc.Request.User)
                .Include(r => r.RequestUserAssoc.Request.User.Department)
                .Where(r => r.RequestUserAssoc.IdRequestUser == idRequestUser && !r.RequestUserAssoc.Deleted
                    && !r.RequestUserAssoc.Request.Deleted && 
                    r.RequestUserAssoc.Request.RequestStates
                    .Where(rs => !r.Deleted).OrderByDescending(rs => rs.IdRequestState).FirstOrDefault().IdRequestStateType == 4 &&
                    r.RequestUserAssoc.Request.IdRequestType != 4  && r.RequestUserAssoc.Request.IdRequestType != 3 &&
                    r.RequestUserAssoc.Request.RequestStates
                    .Where(rs => !r.Deleted).OrderByDescending(rs => rs.IdRequestState).FirstOrDefault().Date >= from &&
                    r.RequestUserAssoc.Request.RequestStates
                    .Where(rs => !r.Deleted).OrderByDescending(rs => rs.IdRequestState).FirstOrDefault().Date <= to)
                    .Select(r => new ResourceUserRightHistoryModel
                    {
                        IdRequest = r.RequestUserAssoc.IdRequest,
                        IdRequestType = r.RequestUserAssoc.Request.IdRequestType,
                        RequestCompleteDate = r.RequestUserAssoc.Request.RequestStates.Where(rs => !r.Deleted)
                            .OrderByDescending(rs => rs.IdRequestState)
                            .FirstOrDefault().Date,
                        DelegationExtInfo = null,
                        AclUser = r.RequestUserAssoc.Request.User,
                        RequestUser = r.RequestUserAssoc.RequestUser,
                        GrantType = r.RequestRightGrantType,
                        Right = r.ResourceRight,
                        RequestRightDescription = r.Descirption
                    }).ToList();
            var delegations = (from delegationRow in _rightRepository.GetDelegationRequestUsersExtInfo()
                              .Include(r => r.DelegateToUser)
                    join userAssocRow in _rightRepository.GetRequestUserAssocs()
                        .Include(r => r.Request)
                        .Include(r => r.Request.RequestStates)
                        .Include(r => r.Request.User)
                        .Include(r => r.Request.User.Department)
                        .Include(r => r.RequestUser)
                    on delegationRow.IdRequestUserAssoc equals  userAssocRow.IdRequestUserAssoc
                    join userRightAssocRow in _rightRepository.GetRequestUserRightAssocs()
                        .Include(r => r.RequestRightGrantType)
                        .Include(r => r.ResourceRight)
                        .Include(r => r.ResourceRight.Resource)
                    on userAssocRow.IdRequestUserAssoc equals  userRightAssocRow.IdRequestUserAssoc
                    where delegationRow.IdDelegateToUser == idRequestUser || userAssocRow.IdRequestUser == idRequestUser &&
                        userAssocRow.Request.RequestStates.Where(rs => !rs.Deleted)
                            .OrderByDescending(rs => rs.IdRequestState)
                            .FirstOrDefault().IdRequestStateType == 4 &&
                            !delegationRow.Deleted && !userAssocRow.Deleted &&
                            !userAssocRow.Request.Deleted && !userRightAssocRow.Deleted
                    select new ResourceUserRightHistoryModel
                    {
                        IdRequest = userAssocRow.IdRequest,
                        IdRequestType = userAssocRow.Request.IdRequestType,
                        RequestCompleteDate = userAssocRow.Request.RequestStates.Where(rs => !rs.Deleted)
                            .OrderByDescending(rs => rs.IdRequestState)
                            .FirstOrDefault().Date,
                        DelegationExtInfo = delegationRow,
                        AclUser = userAssocRow.Request.User,
                        RequestUser = userAssocRow.RequestUser,
                        GrantType = userRightAssocRow.RequestRightGrantType,
                        Right = userRightAssocRow.ResourceRight,
                        RequestRightDescription = userRightAssocRow.Descirption
                    }).ToList();
            var excludeUser = (from requestRow in _rightRepository.GetRequests()
                    join userAssocRow in _rightRepository.GetRequestUserAssocs()
                    on requestRow.IdRequest equals userAssocRow.IdRequest
                    where requestRow.RequestStates.Where(rs => !rs.Deleted).OrderByDescending(r => r.IdRequestState)
                        .FirstOrDefault().IdRequestStateType == 4 && userAssocRow.IdRequestUser == idRequestUser
                        && requestRow.IdRequestType == 3
                    select new ResourceUserRightHistoryModel
                    {
                        IdRequest = requestRow.IdRequest,
                        IdRequestType = requestRow.IdRequestType,
                        RequestCompleteDate = requestRow.RequestStates.Where(rs => !rs.Deleted)
                            .OrderByDescending(rs => rs.IdRequestState)
                            .FirstOrDefault().Date,
                        AclUser = requestRow.User
                    }).ToList();

            return userRights.Union(delegations).Union(excludeUser).OrderBy(r => r.RequestCompleteDate);
        }
    }
}