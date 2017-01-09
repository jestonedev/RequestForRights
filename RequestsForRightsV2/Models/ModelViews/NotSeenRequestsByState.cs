using System.Collections.Generic;
using RequestsForRights.Domain.Entities;

namespace RequestsForRightsV2.Models.ModelViews
{
    public class NotSeenRequestsByState
    {
        public RequestState RequestState { get; set; }
        public IEnumerable<Request> NotSeenRequests { get; set; }
    }
}