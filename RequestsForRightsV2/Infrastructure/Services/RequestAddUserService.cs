﻿using System;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels.Request;
using WebGrease.Css.Extensions;

namespace RequestsForRights.Web.Infrastructure.Services
{
    public class RequestAddUserService: RequestService<RequestUserModel, RequestAddUserViewModel>, 
        IRequestAddUserService
    {
        private readonly IUserService _userService;

        public RequestAddUserService(IRequestRepository requestsRepository, 
            IResourceRepository resourceRepository, 
            IUserService userService,
            IRequestSecurityService<RequestUserModel> requestSecurityService)
            : base(requestsRepository, resourceRepository, requestSecurityService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }
            _userService = userService;
        }

        public override RequestAddUserViewModel GetEmptyRequestViewModel()
        {
            var emtpyRequest = LoadAdditionalInfoToViewModel(base.GetEmptyRequestViewModel());
            emtpyRequest.RequestModel.Users.ForEach(u =>
            {
                var createAccountRight = emtpyRequest.ResourceRights.FirstOrDefault(r => r.Resource.Name == "Учетная запись пользователя");
                if (createAccountRight != null)
                {
                    u.Rights.Insert(0, new RequestUserRightModel
                    {
                        IdResource = createAccountRight.IdResource,
                        IdResourceRight = createAccountRight.IdResourceRight,
                        IdRequestRightGrantType = 1
                    });
                }
            });
            return emtpyRequest;
        }

        public override RequestAddUserViewModel GetRequestViewModelBy(Request request)
        {
            return LoadAdditionalInfoToViewModel(base.GetRequestViewModelBy(request));
        }

        public override RequestAddUserViewModel GetRequestViewModelBy(
            RequestModel<RequestUserModel> request)
        {
            return LoadAdditionalInfoToViewModel(base.GetRequestViewModelBy(request));
        }

        private RequestAddUserViewModel LoadAdditionalInfoToViewModel(
            RequestAddUserViewModel viewModel)
        {
            var departments = _userService.GetDepartments().ToList();
            var units = _userService.GetUnits().ToList();
            viewModel.Units = departments.Select(r => new Department
            {
                IdParentDepartment = r.IdDepartment,
                ParentDepartment = r
            }).Concat(units).OrderBy(r => r.Name).ToList();
            viewModel.Departments = departments
                .Concat(units.Select(r => r.ParentDepartment))
                .Distinct()
                .OrderBy(r => r.Name).ToList();
            viewModel.Resources = RequestSecurityService.FilterResources(ResourceRepository.GetResources())
                .OrderBy(r => r.Name).ToList();
            viewModel.ResourceRights = ResourceRepository.GetResourceRights().OrderBy(r => r.Name).ToList();
            return viewModel;
        }
    }
}