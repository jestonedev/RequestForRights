using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories
{
    public class RequestRepository: IRequestRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public RequestRepository(IDatabaseContext databaseContext)
        {
            if (databaseContext == null)
            {
                throw new ArgumentNullException("databaseContext");
            }
            _databaseContext = databaseContext;
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

        public IQueryable<RequestType> GetRequestTypes()
        {
            return _databaseContext.RequestTypes;
        }

        public IQueryable<RequestUserLastSeen> GetRequestsUserLastSeens(string login)
        {
            return _databaseContext.RequestUserLastSeens.Include(r => r.User)
                .Where(r => r.User.Login.ToLower() == login.ToLower());
        }

        public Request GetRequestById(int id)
        {
            return _databaseContext.Requests.Where(r => r.IdRequest == id).
                Include(r => r.RequestType).
                Include(r => r.RequestUserAssoc).
                Include(r => r.RequestUserAssoc.Select(ru => ru.RequestUserRightAssocs)).
                Include(r => r.RequestUserAssoc.Select(ru => ru.RequestUser)).
                Include(r => r.RequestStates).
                Include(r => r.RequestAgreements).
                FirstOrDefault();
        }

        public IQueryable<RequestExtDescription> GetRequestExtDescriptions(int idRequest)
        {
            return _databaseContext.RequestExtDescriptions.Where(r => r.IdRequest == idRequest);
        }

        public IQueryable<RequestAgreement> GetRequestAgreements(int idRequest)
        {
            return _databaseContext.RequestAgreements.Where(r => r.IdRequest == idRequest);
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
            UpdateRequestUsers(req.RequestUserAssoc.Where(r => !r.Deleted), 
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
            var newUsersList = newUsersAssoc.ToList();
            newUsersList.ForEach(r => r.IdRequest = request.IdRequest);
            var newReqUsers = newUsersList.Select(r => CreateUserIfNotExists(r.RequestUser)).ToList();
            foreach (var userAssoc in oldUsersAssoc)
            {
                if (newReqUsers.Any(r => r == userAssoc.RequestUser))
                {
                    newReqUsers.Remove(userAssoc.RequestUser);
                    continue;
                }
                userAssoc.Deleted = true;
                _databaseContext.RequestUserAssocs.Attach(userAssoc);
                _databaseContext.Entry(userAssoc).State = EntityState.Modified;
            }
            foreach (var user in newReqUsers)
            {
                _databaseContext.RequestUserAssocs.Add(
                    new RequestUserAssoc
                    {
                        Request = request,
                        RequestUser = user
                    });
            }
        }

        private RequestUser CreateUserIfNotExists(RequestUser requestUser)
        {
            var user = _databaseContext.Users.FirstOrDefault(r => !r.Deleted &&
                       requestUser.Login != null ? r.Login == requestUser.Login :
                       r.Snp == requestUser.Snp && r.Department == requestUser.Department &&
                       r.Unit == requestUser.Unit);
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
                    requestAgreement.Description = null;
                    requestAgreement.Date = null;
                }
                else
                {
                    // Первичное согласование
                    _databaseContext.RequestAgreements.Remove(requestAgreement);
                }
            }
        }

        public Request InsertRequest(Request request)
        {
            request.RequestUserAssoc.ToList().ForEach(r =>
            {
                var user = CreateUserIfNotExists(r.RequestUser);
                r.RequestUser = user;
            });
            request.RequestStates = new List<RequestState>
            {
                new RequestState
                {
                    IdRequestStateType = 1,
                    Request = request,
                    Date = DateTime.Now
                }
            };
            var resRequest = _databaseContext.Requests.Add(request);
            return resRequest;
        }

        public int SaveChanges()
        {
            return _databaseContext.SaveChanges();
        }
    }
}
