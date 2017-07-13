using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels.Request;

namespace RequestsForRights.Web.Infrastructure.Services
{
    public class RequestRemoveUserService : RequestService<RequestUserModel, RequestRemoveUserViewModel>, 
        IRequestRemoveUserService
    {
        public RequestRemoveUserService(
            IRequestRepository requestsRepository, 
            IResourceRepository resourceRepository,
            IRequestSecurityService<RequestUserModel> requestSecurityService) : 
            base(requestsRepository, resourceRepository, requestSecurityService)
        {
        }
    }
}