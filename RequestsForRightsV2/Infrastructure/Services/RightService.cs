using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.Models;

namespace RequestsForRights.Infrastructure.Services
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
            int? idRequestUser, int? idResource)
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
            var completedUsers = from completedRequestRow in completedRequests
                join requestUserAssocRow in _rightRepository.GetRequestUserAssocs()
                    on completedRequestRow.IdRequest equals requestUserAssocRow.IdRequest
                where idRequestUser == null || requestUserAssocRow.IdRequestUser == idRequestUser.Value
                select new
                {
                    completedRequestRow.IdRequest,
                    completedRequestRow.IdRequestType,
                    completedRequestRow.DateFrom,
                    requestUserAssocRow.IdRequestUser,
                    requestUserAssocRow.IdRequestUserAssoc
                };
            var excludedUserDates = from completedUserRow in completedUsers
                where completedUserRow.IdRequestType == 3
                // Disconnect user
                group completedUserRow.DateFrom by completedUserRow.IdRequestUser
                into gs
                select new
                {
                    IdRequestUser = gs.Key,
                    DisconnectDate = gs.Max()
                };
            date = date.Date.AddDays(1).AddSeconds(-1);
            var changingUserAssocs = from completedUserRow in completedUsers
                join excludeUserDateRow in excludedUserDates
                    on completedUserRow.IdRequestUser equals excludeUserDateRow.IdRequestUser into exUser
                from exUserRow in exUser.DefaultIfEmpty()
                where new[] {1, 2}.Contains(completedUserRow.IdRequestType) &&
                      (exUserRow == null || exUserRow.DisconnectDate < completedUserRow.DateFrom) &&
                      completedUserRow.DateFrom <= date
                select completedUserRow;
            var changingUserRightsAssoc = from userAssocRow in changingUserAssocs
                join rightAssocRow in _rightRepository.GetRequestUserRightAssocs()
                    on userAssocRow.IdRequestUserAssoc equals rightAssocRow.IdRequestUserAssoc
                where idResource == null || rightAssocRow.ResourceRight.IdResource == idResource.Value
                select new
                {
                    userAssocRow.IdRequestUser,
                    rightAssocRow.IdRequestRightGrantType,
                    rightAssocRow.IdResourceRight,
                    userAssocRow.DateFrom
                };
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
                group userRight.DateFrom by new
                {
                    userRight.IdRequestUser,
                    userRight.IdResourceRight
                }
                into gs
                select new
                {
                    gs.Key.IdRequestUser,
                    gs.Key.IdResourceRight,
                    DateFrom = gs.Min()
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
                    IdResourceRight = resourceRight.IdResourceRight,
                    ResourceRightName = resourceRight.Name,
                    IdResource = resourceRight.IdResource,
                    ResourceName = resourceRight.Resource.Name,
                    DateFrom = userRight.DateFrom
                };
        }

        public IEnumerable<ResourceUserRightModel> GetDelegatedRightsOnDate(DateTime date,
            int? idRequestUser, int? idResource)
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
            var completedUsers = from completedRequestRow in completedRequests
                join requestUserAssocRow in _rightRepository.GetRequestUserAssocs()
                    on completedRequestRow.IdRequest equals requestUserAssocRow.IdRequest
                select new
                {
                    completedRequestRow.IdRequest,
                    completedRequestRow.IdRequestType,
                    completedRequestRow.DateFrom,
                    requestUserAssocRow.IdRequestUser,
                    requestUserAssocRow.IdRequestUserAssoc
                };
            var excludedUserDates = from completedUserRow in completedUsers
                where completedUserRow.IdRequestType == 3
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
                join excludeUserDateRow in excludedUserDates
                    on completedUserRow.IdRequestUser equals excludeUserDateRow.IdRequestUser into exUser
                from exUserRow in exUser.DefaultIfEmpty()
                where completedUserRow.IdRequestType == 4 &&
                      date >= delegateExtInfoRow.DelegateFromDate && 
                      date < delegateExtInfoRow.DelegateToDate  &&
                      (exUserRow == null || exUserRow.DisconnectDate > date ||
                       exUserRow.DisconnectDate < delegateExtInfoRow.DelegateFromDate) &&
                      (idRequestUser == null || 
                      completedUserRow.IdRequestUser == idRequestUser.Value ||
                      delegateExtInfoRow.IdDelegateToUser == idRequestUser.Value)
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
                    IdResourceRight = resourceRight.IdResourceRight,
                    ResourceRightName = resourceRight.Name,
                    IdResource = resourceRight.IdResource,
                    ResourceName = resourceRight.Resource.Name,
                    IdDelegateFromUser = delegateUserFrom.IdRequestUser,
                    DelegateFromUserSnp = delegateUserFrom.Snp,
                    DateFrom = delegateRight.DelegateFromDate,
                    DateTo = delegateRight.DelegateToDate
                };
        }

        public IEnumerable<ResourceUserRightModel> GetRightsOnDate(DateTime date,
            int? idRequestUser, int? idResource)
        {
            var permanentRights = GetPermanentRightsOnDate(date, idRequestUser, idResource);
            var delegateRights = GetDelegatedRightsOnDate(date, idRequestUser, idResource);
            permanentRights = permanentRights.Where(pr =>
                !delegateRights.Any(
                    dr => pr.IdRequestUser == dr.IdDelegateFromUser &&
                          pr.IdResourceRight == dr.IdResourceRight));
            return permanentRights.Concat(delegateRights);
        }

        public IEnumerable<ResourceUserRightModel> GetUserRightsOnDate(DateTime date,
            int? idRequestUser)
        {
            return GetRightsOnDate(date, idRequestUser, null);
        }

        public IEnumerable<ResourceUserRightModel> GetResourceRightsOnDate(DateTime date, int? idResource)
        {
            return GetRightsOnDate(date, null, idResource);
        }
    }
}