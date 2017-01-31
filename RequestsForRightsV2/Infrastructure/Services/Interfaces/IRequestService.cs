using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.FilterOptions;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ViewModels;
using RequestsForRights.Models.ViewModels.Request;

namespace RequestsForRights.Infrastructure.Services.Interfaces
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
        Request GetRequestById(int idRequest);
        RequestModel<TUserModel> GetRequestModelBy(Request request);
        TViewModel GetRequestViewModelBy(Request request);
        TViewModel GetRequestViewModelBy(RequestModel<TUserModel> request);
        TViewModel GetEmptyRequestViewModel();
        IQueryable<RequestExtComment> GetRequestExtComments(int idRequest);
        IQueryable<RequestAgreement> GetRequestAgreements(int idRequest);
        int SaveChanges();
        Request DeleteRequest(int idRequest);
        Request UpdateRequest(RequestModel<TUserModel> requestModel);
        Request InsertRequest(RequestModel<TUserModel> requestModel);
        IQueryable<RequestType> GetRequestTypes();
        void UpdateUserLastSeen(int idRequest, int idUser);
        RequestExtComment AddComment(int idRequest, string comment);
        void SetRequestState(int idRequest, int idRequestStateType, string reason);
        void AddCooordinator(int idRequest, Coordinator coordinator);
    }
}
