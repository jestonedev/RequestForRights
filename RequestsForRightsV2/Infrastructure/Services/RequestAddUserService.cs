using System;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ViewModels;

namespace RequestsForRights.Infrastructure.Services
{
    public class RequestAddUserService: RequestService<RequestUserModel>, IRequestAddUserService
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

        public override RequestViewModel<RequestUserModel> GetEmptyRequestViewModel()
        {
            return LoadDepartmentsToViewModel(base.GetEmptyRequestViewModel());
        }

        public override RequestViewModel<RequestUserModel> GetRequestViewModelBy(Request request)
        {
            return LoadDepartmentsToViewModel(base.GetRequestViewModelBy(request));
        }

        public override RequestViewModel<RequestUserModel> GetRequestViewModelBy(RequestModel<RequestUserModel> request)
        {
            return LoadDepartmentsToViewModel(base.GetRequestViewModelBy(request));
        }

        private RequestViewModel<RequestUserModel> LoadDepartmentsToViewModel(RequestViewModel<RequestUserModel> viewModel)
        {
            viewModel.Departments = _userService.GetDepartments();
            viewModel.Units = _userService.GetUnits();
            return viewModel;
        }
    }
}