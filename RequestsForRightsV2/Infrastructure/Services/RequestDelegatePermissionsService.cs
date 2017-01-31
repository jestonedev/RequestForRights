using System;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ViewModels.Request;

namespace RequestsForRights.Infrastructure.Services
{
    public class RequestDelegatePermissionsService: RequestService<RequestDelegatePermissionsUserModel,
        RequestDelegatePermissionsViewModel>, IRequestDelegatePermissionsService
    {
        private readonly IResourceRepository _resourceRepository;

        public RequestDelegatePermissionsService(IRequestRepository requestsRepository,
            IResourceRepository resourceRepository,
            IRequestSecurityService<RequestDelegatePermissionsUserModel> requestSecurityService) :
            base(requestsRepository, requestSecurityService)
        {
            if (resourceRepository == null)
            {
                throw new ArgumentNullException("resourceRepository");
            }
            _resourceRepository = resourceRepository;
        }

        public override RequestModel<RequestDelegatePermissionsUserModel> GetRequestModelBy(Request request)
        {
            return new RequestModel<RequestDelegatePermissionsUserModel>
            {
                IdRequest = request.IdRequest,
                Description = request.Description,
                IdRequestType = request.IdRequestType,
                Users = request.RequestUserAssoc.Where(ru => !ru.Deleted).Select(ru =>
                {
                    var delegationExtInfo =
                        RequestsRepository.GetDelegationRequestUserExtInfoBy(ru.IdRequestUserAssoc);
                    if (delegationExtInfo == null)
                    {
                        throw new ApplicationException("Отсутствует дополнительная информация о делегировании");
                    }
                    var userModel = FillRequestUserModel(ru);
                    userModel.DelegationRequestUsersExtInfoModel = new DelegationRequestUsersExtInfoModel
                    {
                        LoginDelegateTo = delegationExtInfo.DelegateToUser.Login,
                        PostDelegateTo = delegationExtInfo.DelegateToUser.Post,
                        SnpDelegateTo = delegationExtInfo.DelegateToUser.Snp,
                        PhoneDelegateTo = delegationExtInfo.DelegateToUser.Phone,
                        DepartmentDelegateTo = delegationExtInfo.DelegateToUser.Department,
                        UnitDelegateTo = delegationExtInfo.DelegateToUser.Unit,
                        OfficeDelegateTo = delegationExtInfo.DelegateToUser.Office,
                        DelegateFromDate = delegationExtInfo.DelegateFromDate,
                        DelegateToDate = delegationExtInfo.DelegateToDate
                    };
                    return userModel;
                }).ToList()
            };
        }

        public override RequestDelegatePermissionsViewModel GetEmptyRequestViewModel()
        {
            return LoadAdditionalInfoToViewModel(base.GetEmptyRequestViewModel());
        }

        public override RequestDelegatePermissionsViewModel GetRequestViewModelBy(Request request)
        {
            return LoadAdditionalInfoToViewModel(base.GetRequestViewModelBy(request));
        }

        public override RequestDelegatePermissionsViewModel GetRequestViewModelBy(
            RequestModel<RequestDelegatePermissionsUserModel> request)
        {
            return LoadAdditionalInfoToViewModel(base.GetRequestViewModelBy(request));
        }

        private RequestDelegatePermissionsViewModel LoadAdditionalInfoToViewModel(
            RequestDelegatePermissionsViewModel viewModel)
        {
            viewModel.Resources = _resourceRepository.GetResources().OrderBy(r => r.Name).ToList();
            viewModel.ResourceRights = _resourceRepository.GetResourceRights().OrderBy(r => r.Name).ToList();
            return viewModel;
        }

        public override Request UpdateRequest(RequestModel<RequestDelegatePermissionsUserModel> requestModel)
        {
            return null;
        }

        public override Request InsertRequest(RequestModel<RequestDelegatePermissionsUserModel> requestModel)
        {
            return null;
        }
    }
}