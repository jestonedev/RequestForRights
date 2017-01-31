using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ViewModels.Request;

namespace RequestsForRights.Infrastructure.Services
{
    public class RequestRemoveUserService : RequestService<RequestUserModel, RequestRemoveUserViewModel>, 
        IRequestRemoveUserService
    {
        public RequestRemoveUserService(
            IRequestRepository requestsRepository, 
            IRequestSecurityService<RequestUserModel> requestSecurityService) : 
            base(requestsRepository, requestSecurityService)
        {
        }
    }
}