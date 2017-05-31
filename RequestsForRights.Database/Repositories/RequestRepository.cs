using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RequestsForRights.CachePool;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories
{
    public class RequestRepository: IRequestRepository
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly ICachePool _cachePool;

        public RequestRepository(IDatabaseContext databaseContext, ICachePool cachePool)
        {
            if (databaseContext == null)
            {
                throw new ArgumentNullException("databaseContext");
            }
            _databaseContext = databaseContext;
            if (cachePool == null)
            {
                throw new ArgumentNullException("cachePool");
            }
            _cachePool = cachePool;
        }

        public IQueryable<Request> GetRequests()
        {
            return _databaseContext.Requests.Where(r => !r.Deleted).Include(r => r.User)
                .Include(r => r.RequestType)
                .Include(r => r.RequestStates)
                .Include(r => r.RequestUserLastSeens)
                .Include(r => r.RequestStates.Select(rs => rs.RequestStateType));
        }

        public IQueryable<RequestStateType> GetRequestStateTypes()
        {
            return _databaseContext.RequestStateTypes;
        }

        public IQueryable<RequestRightGrantType> GetRequestRightGrantTypes()
        {
            return _databaseContext.RequestRightGrantTypes;
        }

        public IQueryable<RequestType> GetRequestTypes()
        {
            return _databaseContext.RequestTypes;
        }

        public IQueryable<RequestUserLastSeen> GetRequestsUserLastSeens(string login)
        {
            return _databaseContext.RequestUserLastSeens.Include(r => r.User)
                .Where(r => r.User.Login.ToLower() == login.ToLower());
        }

        public Request GetRequestById(int id, bool dropCache = false)
        {
            var key = id.ToString();
            if (!dropCache && _cachePool.HasValue<Request>(key))
            {
                return _cachePool.GetValue<Request>(key);
            }
            var request = _databaseContext.Requests.Where(r => r.IdRequest == id).
                Include(r => r.RequestType).
                Include(r => r.RequestUserAssoc).
                Include(r => r.RequestUserAssoc.Select(ru => ru.RequestUserRightAssocs)).
                Include(r => r.RequestUserAssoc.Select(ru => ru.RequestUserRightAssocs.Select(rur => rur.ResourceRight))).
                Include(r => r.RequestUserAssoc.Select(ru => ru.RequestUser)).
                Include(r => r.RequestStates).
                Include(r => r.RequestStates.Select(rs => rs.RequestStateType)).
                Include(r => r.RequestAgreements).
                Include(r => r.RequestAgreements.Select(ra => ra.User)).
                FirstOrDefault();
            _cachePool.SetValue(key, request);
            return request;
        }

        public IQueryable<RequestExtComment> GetRequestExtComments(int idRequest)
        {
            return _databaseContext.RequestExtComments.Where(r => r.IdRequest == idRequest);
        }

        public IQueryable<RequestAgreement> GetRequestAgreements(int idRequest)
        {
            return _databaseContext.RequestAgreements.Where(r => r.IdRequest == idRequest)
                .Include(r => r.AgreementType)
                .Include(r => r.User)
                .Include(r => r.User.Department);
        }

        public DelegationRequestUsersExtInfo GetDelegationRequestUserExtInfoBy(int idRequestUserAssoc)
        {
            return _databaseContext.DelegationRequestUsersExtInfo.Find(idRequestUserAssoc);
        }

        public void UpdateUserLastSeen(int idRequest, int idUser)
        {
            var requestUserLastSeen = _databaseContext.RequestUserLastSeens.FirstOrDefault(r => r.IdRequest == idRequest && r.IdUser == idUser);
            if (requestUserLastSeen == null)
            {
                _databaseContext.RequestUserLastSeens.Add(new RequestUserLastSeen
                {
                    IdRequest = idRequest,
                    IdUser = idUser,
                    DateOfLastSeen = DateTime.Now
                });
            }
            else
            {
                requestUserLastSeen.DateOfLastSeen = DateTime.Now;
            }
        }

        public Request DeleteRequest(int idRequest)
        {
            var request = GetRequestById(idRequest);
            if (request == null) return null;
            request.Deleted = true;
            return request;
        }

        public Request UpdateRequest(Request request, bool resetAgreements)
        {
            var req = GetRequestById(request.IdRequest);
            request.IdUser = req.IdUser;
            _databaseContext.Entry(req).CurrentValues.SetValues(request);
            UpdateRequestUsers(
                req.RequestUserAssoc.Where(r => !r.Deleted), 
                request.RequestUserAssoc, req);
            if (resetAgreements)
            {
                ResetAgreements(request.IdRequest);
            }
            return req;
        }

        private void UpdateRequestUsers(IEnumerable<RequestUserAssoc> oldUsersAssoc, 
            IEnumerable<RequestUserAssoc> newUsersAssoc, Request request)
        {
            var newUsersAssocList = newUsersAssoc.ToList();
            newUsersAssocList.ForEach(r =>
            {
                var user = CreateUserIfNotExists(r.RequestUser);
                r.RequestUser = user;
                r.Request = request;
            });
            foreach (var userAssoc in oldUsersAssoc)
            {
                userAssoc.Deleted = true;
            }
            foreach (var user in newUsersAssocList)
            {
                _databaseContext.RequestUserAssocs.Add(user);
            }
        }

        private RequestUser CreateUserIfNotExists(RequestUser requestUser)
        {
            requestUser.Login = string.IsNullOrEmpty(requestUser.Login) ? null : requestUser.Login;
            requestUser.Snp = string.IsNullOrEmpty(requestUser.Snp) ? null : requestUser.Snp;
            requestUser.Department = string.IsNullOrEmpty(requestUser.Department) ? null : requestUser.Department;
            requestUser.Unit = string.IsNullOrEmpty(requestUser.Unit) ? null : requestUser.Unit;

            Func<RequestUser, bool> condition = r => !r.Deleted && requestUser.Login != null
                ? r.Login == requestUser.Login
                : r.Snp == requestUser.Snp && r.Department == requestUser.Department &&
                  r.Unit == requestUser.Unit;
            var user = _databaseContext.Users.FirstOrDefault(condition) ??
                       _databaseContext.Users.Local.FirstOrDefault(condition);
            if (user == null)
            {
                return _databaseContext.Users.Add(requestUser);
            }
            requestUser.IdRequestUser = user.IdRequestUser;
            _databaseContext.Entry(user).CurrentValues.SetValues(requestUser);
            return user;
        }

        private void ResetAgreements(int idRequest)
        {
            var agreements = _databaseContext.RequestAgreements.
                Where(r => r.IdRequest == idRequest);
            foreach (var requestAgreement in agreements)
            {
                if (requestAgreement.IdAgreementType == 2)
                {
                    // Доп. согласование
                    requestAgreement.IdAgreementState = 1;
                    requestAgreement.AgreementDescription = null;
                    requestAgreement.AgreementDate = null;
                }
                else
                {
                    // Первичное согласование
                    _databaseContext.RequestAgreements.Remove(requestAgreement);
                }
            }
        }

        public RequestState AddRequestState(RequestState state, bool resetAgreements)
        {
            var requestState = _databaseContext.RequestStates.Add(state);
            if (resetAgreements)
            {
                ResetAgreements(state.IdRequest);
            }
            return requestState;
        }

        public RequestAgreement AddAdditionalAgreement(RequestAgreement agreement)
        {
            var department = _databaseContext.Departments.FirstOrDefault(
                r => !r.Deleted && r.IdParentDepartment == null &&
                r.Name.ToLower() == agreement.User.Department.Name.ToLower());
            var idDepartment = department == null ? 24 : department.IdDepartment;
            agreement.User.Department = null;
            agreement.User.IdDepartment = idDepartment;
            agreement.User.Roles = agreement.User.Roles.Select(
                r => _databaseContext.AclRoles.Find(r.IdRole)).ToList();
            var user = _databaseContext.AclUsers.FirstOrDefault(r => !r.Deleted &&
                     (r.Login.ToLower() == agreement.User.Login.ToLower())) ??
                       _databaseContext.AclUsers.Add(agreement.User);
            foreach (var aclRole in agreement.User.Roles)
            {
                if (user.Roles.All(r => r.IdRole != aclRole.IdRole))
                {
                    user.Roles.Add(aclRole);
                }
            }
            agreement.User = user;
            agreement.IdUser = user.IdUser;
            return UpdateRequestAgreement(agreement, true);
        }

        public RequestAgreement ExcludeAgreementor(RequestAgreement agreement)
        {
            var user = _databaseContext.AclUsers.FirstOrDefault(r => !r.Deleted && (r.IdUser == agreement.IdUser));
            agreement.User = user;
            return UpdateRequestAgreement(agreement);
        }

        public RequestAgreement UpdateRequestAgreement(RequestAgreement agreement, bool updateSendInfo = false)
        {
            var reqAgreement = _databaseContext.RequestAgreements.
                FirstOrDefault(r => r.IdUser == agreement.User.IdUser &&
                                    r.IdRequest == agreement.IdRequest && 
                                    r.IdAgreementType == agreement.IdAgreementType);
            if (reqAgreement == null)
            {
                return _databaseContext.RequestAgreements.Add(agreement);
            }
            agreement.IdAgreementType = reqAgreement.IdAgreementType;
            agreement.IdRequestAgreement = reqAgreement.IdRequestAgreement;
            if (!updateSendInfo)
            {
                agreement.SendDate = reqAgreement.SendDate;
                agreement.SendDescription = reqAgreement.SendDescription;
            }
            _databaseContext.Entry(reqAgreement).CurrentValues.SetValues(agreement);
            return reqAgreement;
        }

        public void UpdateDelegationRequestUsersExtInfo(int idRequest, List<DelegationRequestUsersExtInfo> delegationRequestUsersExtInfo)
        {
            var oldExtInfo = _databaseContext.DelegationRequestUsersExtInfo.
                Where(r => !r.Deleted && r.RequestUserAssoc.IdRequest == idRequest);
            foreach (var extInfo in oldExtInfo)
            {
                extInfo.Deleted = true;
            }
            delegationRequestUsersExtInfo.ForEach(r =>
            {
                var user = CreateUserIfNotExists(r.DelegateToUser);
                r.DelegateToUser = user;
            });
            foreach (var extInfo in delegationRequestUsersExtInfo)
            {
                _databaseContext.DelegationRequestUsersExtInfo.Add(extInfo);
            }
        }

        public void InsertDelegationRequestUsersExtInfo(List<DelegationRequestUsersExtInfo> delegationRequestUsersExtInfo)
        {
            delegationRequestUsersExtInfo.ForEach(r =>
            {
                var user = CreateUserIfNotExists(r.DelegateToUser);
                r.DelegateToUser = user;
            });
            foreach (var extInfo in delegationRequestUsersExtInfo)
            {
                _databaseContext.DelegationRequestUsersExtInfo.Add(extInfo);
            }
        }

        public Request InsertRequest(Request request)
        {
            request.RequestUserAssoc.ToList().ForEach(r =>
            {
                var user = CreateUserIfNotExists(r.RequestUser);
                r.RequestUser = user;
            });
            var resRequest = _databaseContext.Requests.Add(request);
            return resRequest;
        }

        public int SaveChanges()
        {
            return _databaseContext.SaveChanges();
        }

        public RequestExtComment AddComment(RequestExtComment requestComment)
        {
            return _databaseContext.RequestExtComments.Add(requestComment);
        }
    }
}
