﻿using System;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ViewModels.Request;

namespace RequestsForRights.Infrastructure.Services
{
    public class RequestModifyPermissionsService : RequestService<RequestUserModel, 
        RequestModifyPermissionsViewModel>, 
        IRequestModifyPermissionsService
    {
        private readonly IResourceRepository _resourceRepository;

        public RequestModifyPermissionsService(
            IRequestRepository requestsRepository,
            IResourceRepository resourceRepository, 
            IRequestSecurityService<RequestUserModel> requestSecurityService) : 
            base(requestsRepository, requestSecurityService)
        {
            if (resourceRepository == null)
            {
                throw new ArgumentNullException("resourceRepository");
            }
            _resourceRepository = resourceRepository;
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
            viewModel.Resources = _resourceRepository.GetResources().OrderBy(r => r.Name).ToList();
            viewModel.ResourceRights = _resourceRepository.GetResourceRights().OrderBy(r => r.Name).ToList();
            viewModel.RequestRightGrantTypes = RequestsRepository.GetRequestRightGrantTypes()
                .Where(r => r.IdRequestRightGrantType != 3)
                .OrderBy(r => r.Name).ToList();
            return viewModel;
        }
    }
}