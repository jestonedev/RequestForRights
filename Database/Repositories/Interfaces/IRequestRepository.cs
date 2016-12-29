using System.Collections.Generic;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories.Interfaces
{
    public interface IRequestRepository
    {
        IEnumerable<Request> GetRequests();
        IEnumerable<RequestState> GetRequestStates();
        IEnumerable<RequestUserLastSeen> GetRequestsUserLastSeens(string login);
    }
}
