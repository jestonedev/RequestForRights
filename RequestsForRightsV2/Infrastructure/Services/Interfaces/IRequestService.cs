using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.FilterOptions;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels;
using RequestsForRights.Web.Models.ViewModels.Request;

namespace RequestsForRights.Web.Infrastructure.Services.Interfaces
{
    public interface IRequestService<TUserModel, out TViewModel>
        where TUserModel: RequestUserModel
        where TViewModel: RequestViewModel<TUserModel>
    {
        IEnumerable<RequestsCountByStateTypesViewModel> GetRequestsCountByStateTypes();
        bool DidNotSeenRequest(Request request);
        IQueryable<Request> GetVisibleRequests(RequestsFilterOptions filterOptions,
            IQueryable<Request> filteredRequests);
        IQueryable<Request> GetFilteredRequests(RequestsFilterOptions filterOptions);
        RequestIndexViewModel GetRequestIndexModelView(RequestsFilterOptions filterOptions,
            IQueryable<Request> filteredRequests);
        Request GetRequestById(int idRequest, bool dropCache = false);
        RequestModel<TUserModel> GetRequestModelBy(Request request);
        TViewModel GetRequestViewModelBy(Request request);
        TViewModel GetRequestViewModelBy(RequestModel<TUserModel> request);
        TViewModel GetEmptyRequestViewModel();
        IQueryable<RequestExtComment> GetRequestExtComments(int idRequest);
        IQueryable<RequestAgreement> GetRequestAgreements(int idRequest);
        IEnumerable<AclUser> GetWaitAgreementUsers(int idRequest, IList<RequestAgreement> agreements);
        int SaveChanges();
        Request DeleteRequest(int idRequest);
        Request UpdateRequest(RequestModel<TUserModel> requestModel);
        Request InsertRequest(RequestModel<TUserModel> requestModel);
        IQueryable<RequestType> GetRequestTypes();
        void UpdateUserLastSeen(int idRequest, int idUser);
        RequestExtComment AddComment(int idRequest, string comment);
        void SetRequestState(int idRequest, int idRequestStateType, string reason);
        void AddCooordinator(int idRequest, Coordinator coordinator, string sendDescription);
        void AcceptCancelRequest(int idRequest);
    }
}
