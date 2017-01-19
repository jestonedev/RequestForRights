using System.Linq;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.Models;

namespace RequestsForRights.Infrastructure.Security.Interfaces
{
    public interface IRequestSecurityService<T> : ISecurityService<RequestModel<T>>
        where T: RequestUserModel
    {
        IQueryable<Request> FilterRequests(IQueryable<Request> requests);
        bool CanDelete(Request request);
        bool CanUpdate(Request request);
        bool CanCreate(Request request);
        bool CanRead(Request request);
        bool CanSeeLogin();
        bool CanComment();
    }
}
