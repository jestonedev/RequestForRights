using System.Linq;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.Models;

namespace RequestsForRights.Web.Infrastructure.Security.Interfaces
{
    public interface IRequestSecurityService<T> : ISecurityService<RequestModel<T>>
        where T: RequestUserModel
    {
        IQueryable<Request> FilterRequests(IQueryable<Request> requests);
        IQueryable<Resource> FilterResources(IQueryable<Resource> resources);
        bool CanDelete(Request request);
        bool CanUpdate(Request request);
        bool CanCreate(Request request);
        bool CanRead(Request request);
        bool CanSeeLogin();
        bool CanComment();
        bool CanComment(RequestModel<T> request);
        bool CanComment(Request request);
        bool CanAddCoordinator();
        bool CanAddCoordinator(RequestModel<T> request);
        bool CanAddCoordinator(Request request);
        bool CanSetRequestState(RequestModel<T> request, int idRequestStateType);
        bool CanSetRequestState(Request request, int idRequestStateType);
        bool CanSetRequestStateGlobal(Request request, int idRequestStateType);
        bool CanAgreement(RequestModel<T> request);
        bool CanVisibleUser(RequestModel<T> request, T user);
        bool CanVisibleRight(RequestModel<T> request, RequestUserRightModel right);
    }
}
