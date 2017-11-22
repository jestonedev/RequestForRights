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
            var toDate = date;
            var fromDate = date.Date.AddDays(1);
            return (from rightAssoc in _rightRepository.GetRequestUserRightAssocs()
                join userAssoc in _rightRepository.GetRequestUserAssocs()
                    on rightAssoc.IdRequestUserAssoc equals userAssoc.IdRequestUserAssoc
                join requestUser in _rightRepository.GetRequestUsers()
                    on userAssoc.IdRequestUser equals requestUser.IdRequestUser
                join resourceRight in _rightRepository.GetResourceRights()
                    on rightAssoc.IdResourceRight equals resourceRight.IdResourceRight
                    where rightAssoc.IdRequestRightGrantType == 1 && rightAssoc.GrantedFrom < fromDate &&
                      (rightAssoc.GrantedTo == null || rightAssoc.GrantedTo > toDate) &&
                      (idRequestUser == null || userAssoc.IdRequestUser == idRequestUser.Value) &&
                      (department == null || requestUser.Department == department) &&
                      (unit == null || requestUser.Unit == unit) &&
                      (idResource == null || rightAssoc.ResourceRight.IdResource == idResource.Value)
                group new {rightAssoc.GrantedFrom, rightAssoc.Descirption} by
                    new
                    {
                        requestUser.IdRequestUser,
                        RequestUserSnp = requestUser.Snp,
                        RequestUserDepartment = requestUser.Department,
                        RequestUserUnit = requestUser.Unit,
                        resourceRight.IdResourceRight,
                        ResourceRightName = resourceRight.Name,
                        ResourceRightDescription = resourceRight.Description,
                        resourceRight.IdResource,
                        ResourceName = resourceRight.Resource.Name,
                        ResourceDescription = resourceRight.Resource.Description,
                    }
                into gs
                select new ResourceUserRightModel
                {
                    IdRequestUser = gs.Key.IdRequestUser,
                    RequestUserSnp = gs.Key.RequestUserSnp,
                    RequestUserDepartment = gs.Key.RequestUserDepartment,
                    RequestUserUnit = gs.Key.RequestUserUnit,
                    IdResourceRight = gs.Key.IdResourceRight,
                    ResourceRightName = gs.Key.ResourceRightName,
                    ResourceRightDescription = gs.Key.ResourceRightDescription,
                    IdResource = gs.Key.IdResource,
                    ResourceName = gs.Key.ResourceName,
                    ResourceDescription = gs.Key.ResourceDescription,
                    RightCategory = "Постоянное право",
                    DateFrom = gs.Select(r => r.GrantedFrom).Min(),
                    Description = gs.OrderByDescending(r => r.GrantedFrom).Select(r => r.Descirption).FirstOrDefault()
                }).ToList();
        }

        public IEnumerable<ResourceUserRightModel> GetDelegatedRightsOnDate(DateTime date,
            int? idRequestUser, string department, string unit, int? idResource)
        {
            var toDate = date;
            var fromDate = date.Date.AddDays(1);
            return (from rightAssoc in _rightRepository.GetRequestUserRightAssocs()
                join userAssoc in _rightRepository.GetRequestUserAssocs()
                    on rightAssoc.IdRequestUserAssoc equals userAssoc.IdRequestUserAssoc
                join requestUser in _rightRepository.GetRequestUsers()
                    on userAssoc.IdRequestUser equals requestUser.IdRequestUser
                join resourceRight in _rightRepository.GetResourceRights()
                    on rightAssoc.IdResourceRight equals resourceRight.IdResourceRight
                join delegation in _rightRepository.GetDelegationRequestUsersExtInfo()
                    on userAssoc.IdRequestUserAssoc equals delegation.IdRequestUserAssoc
                join delegationUser in _rightRepository.GetRequestUsers()
                    on delegation.IdDelegateToUser equals delegationUser.IdRequestUser
                where rightAssoc.IdRequestRightGrantType == 3 && rightAssoc.GrantedFrom < fromDate &&
                      (rightAssoc.GrantedTo == null || rightAssoc.GrantedTo > toDate) &&
                      (idRequestUser == null || requestUser.IdRequestUser == idRequestUser.Value ||
                       delegation.IdDelegateToUser == idRequestUser.Value) &&
                      (department == null || requestUser.Department == department ||
                       delegationUser.Department == department) &&
                      (unit == null || requestUser.Unit == unit || delegationUser.Unit == unit) &&
                      (idResource == null || rightAssoc.ResourceRight.IdResource == idResource.Value)
                group new {rightAssoc.GrantedFrom, rightAssoc.GrantedTo} by
                    new
                    {
                        IdRequestFromUser = requestUser.IdRequestUser,
                        RequestUserFromSnp = requestUser.Snp,
                        RequestUserFromDepartment = requestUser.Department,
                        RequestUserFromUnit = requestUser.Unit,

                        delegation.IdDelegateToUser,
                        DelegateToUserSnp = delegationUser.Snp,
                        DelegateToUserDepartment = delegationUser.Department,
                        DelegateToUserUnit = delegationUser.Unit,

                        resourceRight.IdResourceRight,
                        ResourceRightName = resourceRight.Name,
                        ResourceRightDescription = resourceRight.Description,
                        resourceRight.IdResource,
                        ResourceName = resourceRight.Resource.Name,
                        ResourceDescription = resourceRight.Resource.Description,
                    }
                into gs
                select new ResourceUserRightModel
                {
                    IdRequestUser = gs.Key.IdDelegateToUser,
                    RequestUserSnp = gs.Key.DelegateToUserSnp,
                    RequestUserDepartment = gs.Key.DelegateToUserDepartment,
                    RequestUserUnit = gs.Key.DelegateToUserUnit,
                    IdResourceRight = gs.Key.IdResourceRight,
                    ResourceRightName = gs.Key.ResourceRightName,
                    ResourceRightDescription = gs.Key.ResourceRightDescription,
                    IdResource = gs.Key.IdResource,
                    ResourceName = gs.Key.ResourceName,
                    ResourceDescription = gs.Key.ResourceDescription,
                    RightCategory = "Делегированное право",
                    IdDelegateFromUser = gs.Key.IdRequestFromUser,
                    DelegateFromUserSnp = gs.Key.RequestUserFromSnp,
                    DelegateFromUserDepartment = gs.Key.RequestUserFromDepartment,
                    DelegateFromUserUnit = gs.Key.RequestUserFromUnit,
                    DateDelegateFrom = gs.Select(r => r.GrantedFrom).Min(),
                    DateDelegateTo = gs.Select(r => r.GrantedTo).Max()
                }).ToList();
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
                    r.RequestUserAssoc.Request.IdCurrentRequestStateType == 4 &&
                    r.RequestUserAssoc.Request.IdRequestType != 4  && r.RequestUserAssoc.Request.IdRequestType != 3 &&
                    r.RequestUserAssoc.Request.CurrentRequestStateDate >= from &&
                    r.RequestUserAssoc.Request.CurrentRequestStateDate <= to)
                    .Select(r => new ResourceUserRightHistoryModel
                    {
                        IdRequest = r.RequestUserAssoc.IdRequest,
                        IdRequestType = r.RequestUserAssoc.Request.IdRequestType,
                        RequestCompleteDate = r.RequestUserAssoc.Request.CurrentRequestStateDate.Value,
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
                        userAssocRow.Request.IdCurrentRequestStateType == 4 &&
                            !delegationRow.Deleted && !userAssocRow.Deleted &&
                            !userAssocRow.Request.Deleted && !userRightAssocRow.Deleted
                    select new ResourceUserRightHistoryModel
                    {
                        IdRequest = userAssocRow.IdRequest,
                        IdRequestType = userAssocRow.Request.IdRequestType,
                        RequestCompleteDate = userAssocRow.Request.CurrentRequestStateDate.Value,
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
                    where requestRow.IdCurrentRequestStateType == 4 && userAssocRow.IdRequestUser == idRequestUser
                        && requestRow.IdRequestType == 3
                    select new ResourceUserRightHistoryModel
                    {
                        IdRequest = requestRow.IdRequest,
                        IdRequestType = requestRow.IdRequestType,
                        RequestCompleteDate = requestRow.CurrentRequestStateDate.Value,
                        AclUser = requestRow.User
                    }).ToList();

            return userRights.Union(delegations).Union(excludeUser).OrderBy(r => r.RequestCompleteDate);
        }
    }
}