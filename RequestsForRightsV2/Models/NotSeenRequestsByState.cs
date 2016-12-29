using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRightsV2.Models
{
    public class NotSeenRequestsByState
    {
        public RequestState RequestState { get; set; }
        public IEnumerable<Request> NotSeenRequests { get; set; }
    }
}