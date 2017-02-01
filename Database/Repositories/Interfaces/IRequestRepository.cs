using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories.Interfaces
{
    public interface IRequestRepository
    {
        IQueryable<Request> GetRequests();
        IQueryable<RequestStateType> GetRequestStateTypes();
        IQueryable<RequestRightGrantType> GetRequestRightGrantTypes();
        IQueryable<RequestType> GetRequestTypes();
        IQueryable<RequestUserLastSeen> GetRequestsUserLastSeens(string login);
        Request GetRequestById(int idRequest, bool dropCache = false);

        IQueryable<RequestExtComment> GetRequestExtComments(int idRequest);
        IQueryable<RequestAgreement> GetRequestAgreements(int idRequest);
        DelegationRequestUsersExtInfo GetDelegationRequestUserExtInfoBy(int idRequestUserAssoc);
        void UpdateUserLastSeen(int idRequest, int idUser);
        Request DeleteRequest(int idRequest);
        Request UpdateRequest(Request request, bool resetAgreements);
        Request InsertRequest(Request request);
        int SaveChanges();
        RequestExtComment AddComment(RequestExtComment requestComment);
        RequestState AddRequestState(RequestState state, bool resetAgreements);
        RequestAgreement AddAdditionalAgreement(RequestAgreement agreement);
        RequestAgreement UpdateRequestAgreement(RequestAgreement agreement);
    }
}
