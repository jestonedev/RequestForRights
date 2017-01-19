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
        Request GetRequestById(int idRequest);

        IQueryable<RequestExtComment> GetRequestExtComments(int idRequest);
        IQueryable<RequestAgreement> GetRequestAgreements(int idRequest);
        DelegationRequestUsersExtInfo GetDelegationRequestUserExtInfoBy(int idRequestUserAssoc);
        void UpdateUserLastSeen(int idRequest, int idUser);
        Request DeleteRequest(int idRequest);
        Request UpdateRequest(Request request, bool resetAgreements);
        Request InsertRequest(Request request);
        int SaveChanges();
        RequestExtComment AddComment(RequestExtComment requestComment);
    }
}
