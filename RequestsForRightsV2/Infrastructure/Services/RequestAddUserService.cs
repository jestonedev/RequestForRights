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
    public class RequestAddUserService: RequestService<RequestUserModel, RequestAddUserViewModel>, 
        IRequestAddUserService
    {
        private readonly IUserService _userService;
        private readonly IResourceRepository _resourceRepository;

        public RequestAddUserService(IRequestRepository requestsRepository, 
            IResourceRepository resourceRepository, 
            IUserService userService,
            IRequestSecurityService<RequestUserModel> requestSecurityService)
            : base(requestsRepository, requestSecurityService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }
            _userService = userService;
            if (resourceRepository == null)
            {
                throw new ArgumentNullException("resourceRepository");
            }
            _resourceRepository = resourceRepository;
        }

        public override RequestAddUserViewModel GetEmptyRequestViewModel()
        {
            return LoadAdditionalInfoToViewModel(base.GetEmptyRequestViewModel());
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
            viewModel.Resources = RequestSecurityService.FilterResources(_resourceRepository.GetResources())
                .OrderBy(r => r.Name).ToList();
            viewModel.ResourceRights = _resourceRepository.GetResourceRights().OrderBy(r => r.Name).ToList();
            return viewModel;
        }
    }
}