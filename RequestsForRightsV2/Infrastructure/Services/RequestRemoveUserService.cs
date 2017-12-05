using System;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels.Request;

namespace RequestsForRights.Web.Infrastructure.Services
{
    public class RequestRemoveUserService : RequestService<RequestUserModel, RequestRemoveUserViewModel>, 
        IRequestRemoveUserService
    {
        private readonly IRightService _rightService;

        public RequestRemoveUserService(
            IRequestRepository requestsRepository, 
            IResourceRepository resourceRepository,
            IRightService rightService,
            IRequestSecurityService<RequestUserModel> requestSecurityService) : 
            base(requestsRepository, resourceRepository, requestSecurityService)
        {
            if (rightService == null)
            {
                throw new ArgumentNullException("rightService");
            }
            _rightService = rightService;
        }

        public override RequestModel<RequestUserModel> GetRequestModelBy(Request request)
        {
            DateTime? completeDate = null;
            if (request.IdCurrentRequestStateType == 4)
            {
                completeDate = request.CurrentRequestStateDate;
            }
            var date = DateTime.Now;
            return new RequestModel<RequestUserModel>
            {
                IdRequest = request.IdRequest,
                Description = request.Description,
                OwnerSnp = request.User.Snp,
                OwnerDepartment = request.User.Department.Name,
                RequestStateName = request.CurrentRequestStateType.Name,
                RequestDate = request.RequestStates.First(r => !r.Deleted).Date,
                CompleteDate = completeDate,
                IdRequestType = request.IdRequestType,
                Users = request.RequestUserAssoc.Where(ru => !ru.Deleted).Select(ru =>
                {
                    var userModel = FillRequestUserModel(ru);
                    userModel.Rights = _rightService.GetUserRightsOnDate(date, ru.IdRequestUser)
                        .Select(r => new RequestUserRightModel
                        {
                            IdResource = r.IdResource,
                            IdResourceRight = r.IdResourceRight,
                            IdRequestRightGrantType = r.IdDelegateFromUser == null ? 1 : 3,
                            Description = r.Description,
                            ResourceDescription = r.ResourceDescription,
                            ResourceName = r.ResourceName,
                            ResourceRightDescription = r.ResourceRightDescription,
                            ResourceRightName = r.ResourceRightName,
                            RequestRightGrantTypeName = r.IdDelegateFromUser == null ? "Выдать право" : "Делегировать право"
                        }).ToList();
                    return userModel;
                }).ToList()
            };
        }
    }
}