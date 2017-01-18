using System.Linq;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.FilterOptions;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ModelViews;

namespace RequestsForRights.Infrastructure.Services.Interfaces
{
    public interface IRequestService<T>
        where T: RequestUserModel
    {
        IQueryable<Request> GetNotSeenRequests();
        bool DidNotSeenRequest(Request request);
        NotSeenRequestsViewModel GetNotSeenRequestsViewModel();
        IQueryable<Request> GetVisibleRequests(RequestsFilterOptions filterOptions,
            IQueryable<Request> filteredRequests);
        IQueryable<Request> GetFilteredRequests(RequestsFilterOptions filterOptions);
        RequestIndexViewModel GetRequestIndexModelView(RequestsFilterOptions filterOptions,
            IQueryable<Request> filteredRequests);
        Request GetRequestById(int idRequest);
        RequestModel<T> GetRequestModelBy(Request request);
        RequestViewModel<T> GetRequestViewModelBy(Request request);
        RequestViewModel<T> GetRequestViewModelBy(RequestModel<T> request);
        RequestViewModel<T> GetEmptyRequestViewModel();
        int SaveChanges();
        Request DeleteRequest(int idRequest);
        Request UpdateRequest(RequestModel<T> requestModel);
        Request InsertRequest(RequestModel<T> requestModel);
        IQueryable<RequestType> GetRequestTypes();

        void UpdateUserLastSeen(int idRequest, int idUser);
    }
}
