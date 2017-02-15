using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels.Request;

namespace RequestsForRights.Web.Infrastructure.Services.Interfaces
{
    public interface IRequestAddUserService : IRequestService<RequestUserModel, RequestAddUserViewModel>
    {
    }
}
