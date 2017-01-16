using System;
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

        public Request DeleteRequest(int idRequest)
        {
            var request = GetRequestById(idRequest);
            if (request == null) return null;
            request.Deleted = true;
            return request;
        }

        public Request GetRequestById(int id)
        {
            return _databaseContext.Requests.Find(id);
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

        public int SaveChanges()
        {
            return _databaseContext.SaveChanges();
        }
    }
}
