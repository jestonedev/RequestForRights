using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories.Interfaces
{
    public interface IRightRepository
    {
        IQueryable<Request> GetRequests();
        IQueryable<RequestState> GetRequestStates();
        IQueryable<RequestUserAssoc> GetRequestUserAssocs();
        IQueryable<RequestUserRightAssoc> GetRequestUserRightAssocs();
        IQueryable<DelegationRequestUsersExtInfo> GetDelegationRequestUsersExtInfo();
        IQueryable<RequestUser> GetRequestUsers();
        IQueryable<ResourceRight> GetResourceRights();
    }
}
