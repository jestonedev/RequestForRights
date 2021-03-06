﻿using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels.Request;

namespace RequestsForRights.Web.Infrastructure.Services
{
    public class RequestModifyPermissionsService : RequestService<RequestUserModel, 
        RequestModifyPermissionsViewModel>, 
        IRequestModifyPermissionsService
    {

        public RequestModifyPermissionsService(
            IRequestRepository requestsRepository,
            IResourceRepository resourceRepository, 
            IRequestSecurityService<RequestUserModel> requestSecurityService) : 
            base(requestsRepository, resourceRepository, requestSecurityService)
        {
        }
        public override RequestModifyPermissionsViewModel GetEmptyRequestViewModel()
        {
            return LoadAdditionalInfoToViewModel(base.GetEmptyRequestViewModel());
        }

        public override RequestModifyPermissionsViewModel GetRequestViewModelBy(Request request)
        {
            return LoadAdditionalInfoToViewModel(base.GetRequestViewModelBy(request));
        }

        public override RequestModifyPermissionsViewModel GetRequestViewModelBy(
            RequestModel<RequestUserModel> request)
        {
            return LoadAdditionalInfoToViewModel(base.GetRequestViewModelBy(request));
        }

        private RequestModifyPermissionsViewModel LoadAdditionalInfoToViewModel(
            RequestModifyPermissionsViewModel viewModel)
        {
            viewModel.Resources =
                RequestSecurityService.FilterResources(ResourceRepository.GetResources())
                    .OrderBy(r => r.Name)
                    .ToList();
            viewModel.ResourceRights = ResourceRepository.GetResourceRights().OrderBy(r => r.Name).ToList();
            viewModel.RequestRightGrantTypes = RequestsRepository.GetRequestRightGrantTypes()
                .Where(r => r.IdRequestRightGrantType != 3)
                .OrderBy(r => r.Name).ToList();
            return viewModel;
        }
    }
}