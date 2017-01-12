using System.Collections.Generic;
using RequestsForRightsV2.Models.ModelViews;

namespace RequestsForRightsV2.Infrastructure.Services.Interfaces
{
    public interface IRequestService
    {
        IEnumerable<NotSeenRequestsByState> GetNotSeenRequestsByStates();
    }
}
