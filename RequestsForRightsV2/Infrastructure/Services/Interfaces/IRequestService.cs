using System.Linq;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.FilterOptions;
using RequestsForRights.Models.ModelViews;

namespace RequestsForRights.Infrastructure.Services.Interfaces
{
    public interface IRequestService
    {
        IQueryable<Request> GetNotSeenRequests();
        NotSeenRequestsViewModel GetNotSeenRequestsViewModel();
        IQueryable<Request> GetVisibleRequests(RequestsFilterOptions filterOptions,
            IQueryable<Request> filteredRequests);
        IQueryable<Request> GetFilteredRequests(RequestsFilterOptions filterOptions);
        RequestIndexModelView GetRequestIndexModelView(RequestsFilterOptions filterOptions,
            IQueryable<Request> filteredRequests);
        Request GetRequestById(int idRequest);
        Request DeleteRequest(int idRequest);
        int SaveChanges();
    }
}
