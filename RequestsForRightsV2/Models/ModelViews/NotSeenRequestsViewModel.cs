using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Models.ModelViews
{
    public class NotSeenRequestsViewModel
    {
        public IQueryable<RequestStateType> RequestStateTypes { get; set; }
        public IQueryable<Request> NotSeenRequests { get; set; }
    }
}