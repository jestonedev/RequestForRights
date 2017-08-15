using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels.Request;
using AclRole = RequestsForRights.Web.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Web.Infrastructure.Services
{
    public class RequestDelegatePermissionsService: RequestService<RequestDelegatePermissionsUserModel,
        RequestDelegatePermissionsViewModel>, IRequestDelegatePermissionsService
    {

        public RequestDelegatePermissionsService(IRequestRepository requestsRepository,
            IResourceRepository resourceRepository,
            IRequestSecurityService<RequestDelegatePermissionsUserModel> requestSecurityService) :
            base(requestsRepository, resourceRepository, requestSecurityService)
        {
        }

        public override RequestModel<RequestDelegatePermissionsUserModel> GetRequestModelBy(Request request)
        {
            DateTime? completeDate = null;
            if (request.IdCurrentRequestStateType == 4)
            {
                completeDate = request.CurrentRequestStateDate;
            }
            return new RequestModel<RequestDelegatePermissionsUserModel>
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
            var emptyViewModel = base.GetEmptyRequestViewModel();
            emptyViewModel.RequestModel.Users.ToList()
                .ForEach(r =>
                {
                    r.DelegationRequestUsersExtInfoModel = new DelegationRequestUsersExtInfoModel
                    {
                        DelegateFromDate = DateTime.Now.Date,
                        DelegateToDate = DateTime.Now.Date
                    };
                });
            return LoadAdditionalInfoToViewModel(emptyViewModel);
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
            viewModel.Resources = RequestSecurityService.FilterResources(ResourceRepository.GetResources())
                .OrderBy(r => r.Name).ToList();
            viewModel.ResourceRights = ResourceRepository.GetResourceRights().OrderBy(r => r.Name).ToList();
            return viewModel;
        }

        public override Request UpdateRequest(RequestModel<RequestDelegatePermissionsUserModel> requestModel)
        {
            var delegationRequestUsersExtInfo = new List<DelegationRequestUsersExtInfo>();
            var request = ConvertToRequest(requestModel, (rua, user) =>
            {
                delegationRequestUsersExtInfo.Add(GetDelegationRequestUsersExtInfo(rua, user));
            });
            var updatedRequest = RequestsRepository.UpdateRequest(request,
                !RequestSecurityService.InRole(AclRole.Administrator));
            RequestsRepository.UpdateDelegationRequestUsersExtInfo(request.IdRequest, delegationRequestUsersExtInfo);
            return updatedRequest;
        }

        public override Request InsertRequest(RequestModel<RequestDelegatePermissionsUserModel> requestModel)
        {
            var delegationRequestUsersExtInfo = new List<DelegationRequestUsersExtInfo>();
            var request = ConvertToRequest(requestModel, (rua, user) =>
            {
                delegationRequestUsersExtInfo.Add(GetDelegationRequestUsersExtInfo(rua, user));
            });
            request.User = RequestSecurityService.GetUserInfo();
            var idRequestStateType =
                RequestSecurityService.CanSetRequestStateGlobal(request, 1) ? 1 : 2;
            request.RequestStates = new List<RequestState>
            {
                new RequestState
                {
                    IdRequestStateType = idRequestStateType,
                    Request = request,
                    Date = DateTime.Now
                }
            };
            var insertedRequest = RequestsRepository.InsertRequest(request);
            RequestsRepository.InsertDelegationRequestUsersExtInfo(delegationRequestUsersExtInfo);
            return insertedRequest;
        }

        private DelegationRequestUsersExtInfo GetDelegationRequestUsersExtInfo(RequestUserAssoc requestUserAssoc,
            RequestDelegatePermissionsUserModel user)
        {
            var delegationToUser = new RequestUser
            {
                Snp = user.DelegationRequestUsersExtInfoModel.SnpDelegateTo,
                Login = user.DelegationRequestUsersExtInfoModel.LoginDelegateTo,
                Post = user.DelegationRequestUsersExtInfoModel.PostDelegateTo,
                Department = user.DelegationRequestUsersExtInfoModel.DepartmentDelegateTo,
                Unit = user.DelegationRequestUsersExtInfoModel.UnitDelegateTo,
                Office = user.DelegationRequestUsersExtInfoModel.OfficeDelegateTo,
                Phone = user.DelegationRequestUsersExtInfoModel.PhoneDelegateTo
            };
            return new DelegationRequestUsersExtInfo
            {
                RequestUserAssoc = requestUserAssoc,
                DelegateFromDate = user.DelegationRequestUsersExtInfoModel.DelegateFromDate.Date,
                DelegateToDate = user.DelegationRequestUsersExtInfoModel.DelegateToDate.Date.AddDays(1).AddSeconds(-1),
                DelegateToUser = delegationToUser
            };
        }

        protected override IList<RequestDelegatePermissionsUserModel> ClearUsersDuplicates(IEnumerable<RequestDelegatePermissionsUserModel> users)
        {
            return (from user in users
                    group user by new { user.Login, user.Snp, user.Department, user.Unit, user.Description, 
                        user.DelegationRequestUsersExtInfoModel.LoginDelegateTo,
                        user.DelegationRequestUsersExtInfoModel.SnpDelegateTo,
                        user.DelegationRequestUsersExtInfoModel.DepartmentDelegateTo,
                        user.DelegationRequestUsersExtInfoModel.UnitDelegateTo,
                        user.DelegationRequestUsersExtInfoModel.DelegateFromDate,
                        user.DelegationRequestUsersExtInfoModel.DelegateToDate }
                        into gs
                        select new RequestDelegatePermissionsUserModel
                        {
                            Login = gs.Key.Login,
                            Snp = gs.Key.Snp,
                            Department = gs.Key.Department,
                            Unit = gs.Key.Unit,
                            Post = gs.Any() ? gs.First().Post : null,
                            Office = gs.Any() ? gs.First().Office : null,
                            Phone = gs.Any() ? gs.First().Phone : null,
                            Description = gs.Select(r => r.Description).Aggregate((v, acc) => v + "\n" + acc),
                            DelegationRequestUsersExtInfoModel = new DelegationRequestUsersExtInfoModel
                            {
                                LoginDelegateTo = gs.Key.LoginDelegateTo,
                                SnpDelegateTo = gs.Key.SnpDelegateTo,
                                DepartmentDelegateTo = gs.Key.DepartmentDelegateTo,
                                UnitDelegateTo = gs.Key.UnitDelegateTo,
                                DelegateFromDate = gs.Key.DelegateFromDate,
                                DelegateToDate = gs.Key.DelegateToDate,
                                PostDelegateTo = gs.Any() ? gs.First().DelegationRequestUsersExtInfoModel.PostDelegateTo : null,
                                OfficeDelegateTo = gs.Any() ? gs.First().DelegationRequestUsersExtInfoModel.OfficeDelegateTo : null,
                                PhoneDelegateTo = gs.Any() ? gs.First().DelegationRequestUsersExtInfoModel.PhoneDelegateTo : null
                            },
                            Rights = gs.Any(r => r.Rights != null) ?
                                gs.Where(r => r.Rights != null).Select(r => r.Rights).
                                Aggregate((v, acc) => v.Concat(acc).ToList()).Distinct().ToList() : null
                        }).ToList();
        }
    }
}