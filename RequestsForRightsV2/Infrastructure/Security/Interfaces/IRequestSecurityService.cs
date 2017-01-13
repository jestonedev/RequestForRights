using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Infrastructure.Security.Interfaces
{
    public interface IRequestSecurityService : ISecurityService<Request>
    {
        IQueryable<Request> FilterRequests(IQueryable<Request> requests);
    }
}
