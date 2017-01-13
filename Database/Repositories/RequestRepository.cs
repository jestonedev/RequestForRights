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
            return _databaseContext.Requests.FirstOrDefault(r => r.IdRequest == id);
        }

        public int SaveChanges()
        {
            return _databaseContext.SaveChanges();
        }
    }
}
