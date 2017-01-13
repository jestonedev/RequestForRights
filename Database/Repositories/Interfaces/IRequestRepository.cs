using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories.Interfaces
{
    public interface IRequestRepository
    {
        IQueryable<Request> GetRequests();
        IQueryable<RequestStateType> GetRequestStateTypes();
        IQueryable<RequestType> GetRequestTypes();
        IQueryable<RequestUserLastSeen> GetRequestsUserLastSeens(string login);

        Request DeleteRequest(int idRequest);
        int SaveChanges();
        Request GetRequestById(int idRequest);
    }
}
