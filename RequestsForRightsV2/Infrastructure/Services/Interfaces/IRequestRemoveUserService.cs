using RequestsForRights.Models.Models;
using RequestsForRights.Models.ViewModels.Request;

namespace RequestsForRights.Infrastructure.Services.Interfaces
{
    public interface IRequestRemoveUserService : 
        IRequestService<RequestUserModel, RequestRemoveUserViewModel>
    {
    }
}
