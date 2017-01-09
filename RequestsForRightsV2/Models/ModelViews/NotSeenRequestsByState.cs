using System.Collections.Generic;
using RequestsForRights.Domain.Entities;

namespace RequestsForRightsV2.Models.ModelViews
{
    public class NotSeenRequestsByState
    {
        public RequestStateType RequestStateType { get; set; }
        public IEnumerable<Request> NotSeenRequests { get; set; }
    }
}