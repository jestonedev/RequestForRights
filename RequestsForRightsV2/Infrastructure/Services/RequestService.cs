using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Enums;
using RequestsForRights.Web.Infrastructure.Extensions;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.FilterOptions;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels;
using RequestsForRights.Web.Models.ViewModels.Request;
using WebGrease.Css.Extensions;
using AclRole = RequestsForRights.Web.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Web.Infrastructure.Services
{
    public class RequestService<TUserModel, TViewModel> : IRequestService<TUserModel, TViewModel>
        where TUserModel: RequestUserModel, new()
        where TViewModel: RequestViewModel<TUserModel>, new()
    {
        protected readonly IRequestRepository RequestsRepository;
        protected readonly IResourceRepository _resourceRepository;
        protected readonly IRequestSecurityService<TUserModel> RequestSecurityService;

        public RequestService(IRequestRepository requestsRepository, IResourceRepository resourceRepository,
            IRequestSecurityService<TUserModel> requestSecurityService)
        {
            if (requestsRepository == null)
            {
                throw new ArgumentNullException("requestsRepository");
            }
            RequestsRepository = requestsRepository;
            if (requestSecurityService == null)
            {
                throw new ArgumentNullException("requestSecurityService");
            }
            RequestSecurityService = requestSecurityService;
            if (resourceRepository == null)
            {
                throw new ArgumentNullException("resourceRepository");
            }
            _resourceRepository = resourceRepository;
        }

        public IQueryable<Request> GetNotSeenRequests()
        {
            var requests = RequestSecurityService.FilterRequests(RequestsRepository.GetRequests())
                .Where(r => r.RequestStates.FirstOrDefault(rs => rs.Date == new DateTime(2017, 4, 1)) == null);
            var requestsUserLastSeens = RequestsRepository.GetRequestsUserLastSeens(RequestSecurityService.CurrentUser);
            return from request in requests
                join lastSeen in requestsUserLastSeens
                    on request.IdRequest equals lastSeen.IdRequest into joinedRequestsAndLastSeens
                from joinedLastSeensRow in joinedRequestsAndLastSeens.DefaultIfEmpty()
                   where joinedLastSeensRow == null &&
                       request.User.Login.ToLower() != RequestSecurityService.CurrentUser.ToLower()
                select request;
        }

        public bool DidNotSeenRequest(Request request)
        {
            return request.User.Login.ToLower() != RequestSecurityService.CurrentUser.ToLower() &&
                   !request.RequestUserLastSeens.Any(ls =>
                       ls.User.Login.ToLower() == RequestSecurityService.CurrentUser.ToLower() &&
                       ls.IdRequest == request.IdRequest);
        }

        public IQueryable<Request> GetVisibleRequests(RequestsFilterOptions filterOptions, 
            IQueryable<Request> filteredRequests)
        {
            return Order(filteredRequests, filterOptions.SortField, filterOptions.SortDirection).
                Skip(filterOptions.PageSize * filterOptions.PageIndex).
                Take(filterOptions.PageSize);
        }

        public static IQueryable<Request> Order(IQueryable<Request> requests,
            string sortField,
            SortDirection sortDirection)
        {
            switch (sortField)
            {
                case "RequestState":
                    switch (sortDirection)
                    {
                        case SortDirection.Asc:
                            return requests.OrderBy(r => r.RequestStates.OrderByDescending(
                                rs => rs.IdRequestState).
                                FirstOrDefault().RequestStateType.Name);
                        case SortDirection.Desc:
                            return requests.OrderByDescending(r => r.RequestStates.OrderByDescending(
                                rs => rs.IdRequestState).
                                FirstOrDefault().RequestStateType.Name);
                        default:
                            return requests;
                    }
                case "CreateDate":
                    switch (sortDirection)
                    {
                        case SortDirection.Asc:
                            return requests.OrderBy(r => r.RequestStates.OrderBy(rs => rs.IdRequestState).FirstOrDefault().Date);
                        case SortDirection.Desc:
                            return requests.OrderByDescending(r => r.RequestStates.OrderBy(rs => rs.IdRequestState).FirstOrDefault().Date);
                        default:
                            return requests;
                    }
                default:
                    return requests.OrderBy(sortDirection, sortField);
            }
        }

        public IQueryable<Request> GetFilteredRequests(RequestsFilterOptions filterOptions)
        {
            var requests = RequestSecurityService.FilterRequests(RequestsRepository.GetRequests());
            if (filterOptions.IdRequestType != null)
            {
                requests = requests.Where(r => r.IdRequestType == filterOptions.IdRequestType);
            }
            if (filterOptions.IdRequestStateType != null)
            {
                requests = requests.Where(r => r.RequestStates.OrderBy(rs => rs.IdRequestState).Where(rs => !rs.Deleted).
                    OrderByDescending(rs => rs.IdRequestState).FirstOrDefault().IdRequestStateType == 
                    filterOptions.IdRequestStateType);
            }
            if (filterOptions.RequestCategory == RequestCategory.MyRequests)
            {
                requests = requests.Where(r => r.User.Login == RequestSecurityService.CurrentUser);
            }
            if (filterOptions.RequestCategory == RequestCategory.AssignedToMeRequests)
            {
                requests = requests.Where(r => r.RequestExecutors.Any(re => re.Login == RequestSecurityService.CurrentUser));
            }
            if (filterOptions.DateOfFillingFrom != null)
            {
                requests = requests.Where(r =>
                    r.RequestStates.OrderBy(rs => rs.IdRequestState).FirstOrDefault(rs => !rs.Deleted).Date >=
                    filterOptions.DateOfFillingFrom);
            }
            if (filterOptions.DateOfFillingTo != null)
            {
                requests = requests.Where(r =>
                    r.RequestStates.OrderBy(rs => rs.IdRequestState).FirstOrDefault(rs => !rs.Deleted).Date <=
                    filterOptions.DateOfFillingTo);
            }
            if (filterOptions.RequestCategory == RequestCategory.NotSeenRequests)
            {
                requests = requests.Intersect(GetNotSeenRequests());
            }
            if (string.IsNullOrEmpty(filterOptions.Filter))
            {
                return requests;
            }
            var filter = filterOptions.Filter.ToLower();
            return requests.Where(r => r.IdRequest.ToString().ToLower().Contains(filter) || 
                r.User.Snp.ToLower().Contains(filter) ||
                r.RequestStates.Where(rs => !rs.Deleted).OrderByDescending(rs => rs.IdRequestState).FirstOrDefault().RequestStateType.Name.ToLower().Contains(filter) || 
                r.RequestType.Name.ToLower().Contains(filter) || 
                (r.Description != null && r.Description.ToLower().Contains(filter)));
        }

        public RequestIndexViewModel GetRequestIndexModelView(RequestsFilterOptions filterOptions, IQueryable<Request> filteredRequests)
        {
            if (filterOptions.SortField == null)
            {
                filterOptions.SortField = "IdRequest";
                filterOptions.SortDirection = SortDirection.Desc;
            }
            var requests = GetVisibleRequests(filterOptions, filteredRequests).ToList();
            if (!requests.Any())
            {
                filterOptions.PageIndex = 0;
                requests = GetVisibleRequests(filterOptions, filteredRequests).ToList();
            }
            return new RequestIndexViewModel
            {
                VisibleRequests = requests, 
                FilterOptions = filterOptions, 
                RequestCount = filteredRequests.Count(), 
                RequestStateTypes = RequestsRepository.GetRequestStateTypes(), 
                RequestTypes = RequestsRepository.GetRequestTypes(), 
                RequestCatogories = GetRequestCategories()
            };
        }

        private static IEnumerable<RequestCategoryModel> GetRequestCategories()
        {
            return new List<RequestCategoryModel>
            {
                new RequestCategoryModel
                {
                    RequestCategory = RequestCategory.AllRequests, DisplayName = "Все заявки"
                },
                new RequestCategoryModel
                {
                    RequestCategory = RequestCategory.MyRequests, DisplayName = "Мои заявки"
                },
                new RequestCategoryModel
                {
                    RequestCategory = RequestCategory.NotSeenRequests, DisplayName = "Только непросмотренные"
                },
                new RequestCategoryModel
                {
                    RequestCategory = RequestCategory.AssignedToMeRequests, DisplayName = "Назначенные мне"
                }
            };
        }

        public Request GetRequestById(int idRequest, bool dropCache = false)
        {
            return RequestsRepository.GetRequestById(idRequest, dropCache);
        }

        public virtual RequestModel<TUserModel> GetRequestModelBy(Request request)
        {
            var lastState = request.RequestStates.OrderByDescending(r => r.IdRequestState).First(r => !r.Deleted);
            DateTime? completeDate = null;
            if (lastState.IdRequestStateType == 4)
            {
                completeDate = lastState.Date;
            }
            return new RequestModel<TUserModel>
            {
                IdRequest = request.IdRequest,
                Description = request.Description,
                OwnerSnp = request.User.Snp,
                OwnerDepartment = request.User.Department.Name,
                RequestStateName = request.RequestStates.OrderBy(r => r.IdRequestState).Last(r => !r.Deleted).RequestStateType.Name,
                RequestDate = request.RequestStates.First(r => !r.Deleted).Date,
                CompleteDate = completeDate,
                IdRequestType = request.IdRequestType,
                Users = request.RequestUserAssoc.Where(ru => !ru.Deleted).
                    Select(FillRequestUserModel).ToList()
            };
        }

        public Request DeleteRequest(int idRequest)
        {
            return RequestsRepository.DeleteRequest(idRequest);
        }

        public virtual Request UpdateRequest(RequestModel<TUserModel> requestModel)
        {
            var request = ConvertToRequest(requestModel);
            var updatedRequest = RequestsRepository.UpdateRequest(request, 
                !RequestSecurityService.InRole(AclRole.Administrator));
            return updatedRequest;
        }

        public void UpdateRequestState(Request request)
        {
            if (request.RequestStates.OrderByDescending(r => r.IdRequestState).First().IdRequestStateType == 1 &&
                !GetWaitAgreementUsers(request.IdRequest, new List<RequestAgreement>()).Any())
            {
                RequestsRepository.AddRequestState(new RequestState
                {
                    Date = DateTime.Now,
                    Request = request,
                    RequestStateType = RequestsRepository.GetRequestStateTypes().First(r => r.IdRequestStateType == 2)
                }, false);
            }
        }

        public virtual Request InsertRequest(RequestModel<TUserModel> requestModel)
        {
            var request = ConvertToRequest(requestModel);
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
            return RequestsRepository.InsertRequest(request);
        }

        protected Request ConvertToRequest(RequestModel<TUserModel> requestModel, Action<RequestUserAssoc, TUserModel> requestUserAssocAddedCallback = null)
        {
            requestModel.Users.ForEach(r => r.Snp = r.Snp.Trim());
            requestModel.Users = ClearUsersDuplicates(requestModel.Users);

            var request = new Request
            {
                IdRequest = requestModel.IdRequest,
                IdRequestType = requestModel.IdRequestType,
                Description = requestModel.Description,
                RequestUserAssoc = new List<RequestUserAssoc>()
            };
            foreach (var user in requestModel.Users)
            {
                var requestUser = new RequestUser
                {
                    Snp = user.Snp,
                    Login = user.Login,
                    Post = user.Post,
                    Department = user.Department,
                    Unit = user.Unit,
                    Office = user.Office,
                    Phone = user.Phone
                };
                var requestUserAssoc = new RequestUserAssoc
                {
                    RequestUser = requestUser,
                    Request = request,
                    Description = user.Description
                };
                request.RequestUserAssoc.Add(requestUserAssoc);
                if (requestUserAssocAddedCallback != null)
                {
                    requestUserAssocAddedCallback(requestUserAssoc, user);
                }
                if (user.Rights != null)
                {
                    requestUserAssoc.RequestUserRightAssocs = new List<RequestUserRightAssoc>();
                    foreach (var right in user.Rights)
                    {
                        var requestUserRightAssoc = new RequestUserRightAssoc
                        {
                            RequestUserAssoc = requestUserAssoc,
                            Descirption = right.Description,
                            IdRequestRightGrantType = right.IdRequestRightGrantType,
                            IdResourceRight = right.IdResourceRight
                        };
                        requestUserAssoc.RequestUserRightAssocs.Add(requestUserRightAssoc);
                    }
                }
            }
            return request;
        }

        protected virtual IList<TUserModel> ClearUsersDuplicates(IEnumerable<TUserModel> users)
        {
            return (from user in users
                group user by new {user.Login, user.Snp, user.Department, user.Unit, user.Description}
                into gs
                select new TUserModel
                {
                    Login = gs.Key.Login,
                    Snp = gs.Key.Snp,
                    Department = gs.Key.Department,
                    Unit = gs.Key.Unit,
                    Post = gs.Any() ? gs.First().Post : null,
                    Office = gs.Any() ? gs.First().Office : null,
                    Phone = gs.Any() ? gs.First().Phone : null,
                    Description = gs.Select(r => r.Description).Aggregate((v, acc) => v + "\n" + acc),
                    Rights = gs.Any(r => r.Rights != null) ? 
                        gs.Where(r => r.Rights != null).Select(r => r.Rights).
                        Aggregate((v, acc) => v.Concat(acc).ToList()).Distinct().ToList() : null
                }).ToList();
        }

        public IQueryable<RequestType> GetRequestTypes()
        {
            return RequestsRepository.GetRequestTypes();
        }

        public void UpdateUserLastSeen(int idRequest, int idUser)
        {
            RequestsRepository.UpdateUserLastSeen(idRequest, idUser);
        }

        public RequestExtComment AddComment(int idRequest, string comment)
        {
            var requestComment = new RequestExtComment
            {
                DateOfWriting = DateTime.Now,
                Comment = comment,
                IdRequest = idRequest,
                User = RequestSecurityService.GetUserInfo()
            };
            return RequestsRepository.AddComment(requestComment);
        }

        public void SetRequestState(int idRequest, int idRequestStateType, string reason)
        {
            var userInfo = RequestSecurityService.GetUserInfo();
            if (userInfo == null)
            {
                throw new DbUpdateException("Неизвестный пользователь");
            }
            var requestState = new RequestState
            {
                IdRequest = idRequest,
                IdRequestStateType = idRequestStateType,
                Date = DateTime.Now
            };
            var waitAgreementUsers = GetWaitAgreementUsers(idRequest,
                GetRequestAgreements(idRequest).ToList());
            var agreement = new RequestAgreement
            {
                IdRequest = idRequest,
                User = userInfo,
                IdUser = userInfo.IdUser,
                IdAgreementState = idRequestStateType == 2 ? 2 : 3,
                IdAgreementType = 1,
                AgreementDescription = reason,
                AgreementDate = DateTime.Now
            };
            switch (idRequestStateType)
            {
                case 1:
                    RequestsRepository.AddRequestState(requestState, true);
                    break;
                case 2:
                    if (RequestSecurityService.InRole(AclRole.Coordinator) &&
                        waitAgreementUsers.Any(r =>
                            r.RequestAgreements.Any(
                                ra => ra.IdRequest == idRequest &&
                                      ra.IdUser == userInfo.IdUser &&
                                      ra.IdAgreementType == 2) && r.IdUser == userInfo.IdUser))
                    {
                        agreement.IdAgreementType = 2;
                    } else
                    if (RequestSecurityService.InRole(AclRole.Administrator))
                    {
                        RequestsRepository.AddRequestState(requestState, false);
                        break;
                    }
                    RequestsRepository.UpdateRequestAgreement(agreement);
                    if (!NeedAdditionalAgreements(idRequest, agreement))
                    {
                        RequestsRepository.AddRequestState(requestState, false);
                    }
                    break;
                case 3:
                    RequestsRepository.AddRequestState(requestState, false);
                    break;
                case 5:
                    if (RequestSecurityService.InRole(AclRole.Coordinator) &&
                        waitAgreementUsers.Any(r =>
                            r.RequestAgreements.Any(
                                ra => ra.IdRequest == idRequest &&
                                      ra.IdUser == userInfo.IdUser &&
                                      ra.IdAgreementType == 2) && r.IdUser == userInfo.IdUser))
                    {
                        agreement.IdAgreementType = 2;
                        requestState.IdRequestStateType = 2;
                        RequestsRepository.UpdateRequestAgreement(agreement);
                        if (!NeedAdditionalAgreements(idRequest, agreement))
                        {
                            RequestsRepository.AddRequestState(requestState, false);
                        }
                    }
                    else
                    {
                        RequestsRepository.UpdateRequestAgreement(agreement);
                        RequestsRepository.AddRequestState(requestState, false);
                    }
                    break;
                case 4:
                    RequestsRepository.AddRequestState(requestState, false);
                    break;
            }
        }

        public void AddCooordinator(int idRequest, Coordinator coordinator, string sendDescription)
        {
            var agreement = new RequestAgreement
            {
                IdRequest = idRequest,
                IdAgreementType = 2,
                IdAgreementState = 1,
                SendDate = DateTime.Now,
                SendDescription = sendDescription,
                User = new AclUser
                {
                    Login = coordinator.Login,
                    Snp = coordinator.Snp,
                    Email = coordinator.Email,
                    Phone = coordinator.Phone,
                    Department = new Department
                    {
                        IdParentDepartment = null,
                        Name = coordinator.Department
                    },
                    Roles = new List<Domain.Entities.AclRole>
                    {
                        new Domain.Entities.AclRole
                        {
                            IdRole = 8
                        }
                    }
                }
            };
            var requestState = new RequestState
            {
                IdRequest = idRequest,
                IdRequestStateType = 1,
                Date = DateTime.Now
            };
            RequestsRepository.AddRequestState(requestState, false);
            RequestsRepository.AddAdditionalAgreement(agreement);
        }

        public void AcceptCancelRequest(int idRequest)
        {
            var userInfo = RequestSecurityService.GetUserInfo();
            if (userInfo == null)
            {
                throw new DbUpdateException("Неизвестный пользователь");
            }
            var requestState = new RequestState
            {
                IdRequest = idRequest,
                IdRequestStateType = 5,
                Date = DateTime.Now
            };
            RequestsRepository.AddRequestState(requestState, false);
        }

        public void ExcludeAgreementor(int idRequest, int idUser, int idRequestAgreementType)
        {
            var requestState = new RequestState
            {
                IdRequest = idRequest,
                IdRequestStateType = 2,
                Date = DateTime.Now
            };

            var agreement = new RequestAgreement
            {
                IdRequest = idRequest,
                IdUser = idUser,
                IdAgreementState = 4,
                IdAgreementType = idRequestAgreementType
            };
            RequestsRepository.ExcludeAgreementor(agreement);
            if (!NeedAdditionalAgreements(idRequest, agreement))
            {
                RequestsRepository.AddRequestState(requestState, false);
            }
        }

        private bool NeedAdditionalAgreements(int idRequest, RequestAgreement newAgreement)
        {
            return GetWaitAgreementUsers(idRequest,
                GetRequestAgreements(idRequest).ToList().Concat(new[] {newAgreement}).ToList()).Any();
        }

        public virtual TViewModel GetEmptyRequestViewModel()
        {
            return new TViewModel
            {
                RequestModel = new RequestModel<TUserModel>
                {
                    Users = new List<TUserModel>
                    {
                        new TUserModel
                        {
                            Rights = new List<RequestUserRightModel>
                            {
                                new RequestUserRightModel()
                            }
                        }
                    }
                }
            };
        }

        public int SaveChanges()
        {
            return RequestsRepository.SaveChanges();
        }

        public virtual IQueryable<RequestExtComment> GetRequestExtComments(int idRequest)
        {
            return RequestsRepository.GetRequestExtComments(idRequest);
        }

        public virtual IQueryable<RequestAgreement> GetRequestAgreements(int idRequest)
        {
            return RequestsRepository.GetRequestAgreements(idRequest);
        }

        protected TUserModel FillRequestUserModel(RequestUserAssoc userAssoc)
        {
            return new TUserModel
            {
                Description = userAssoc.Description, 
                Login = userAssoc.RequestUser.Login, 
                Post = userAssoc.RequestUser.Post, 
                Snp = userAssoc.RequestUser.Snp, 
                Phone = userAssoc.RequestUser.Phone, 
                Department = userAssoc.RequestUser.Department, 
                Unit = userAssoc.RequestUser.Unit, 
                Office = userAssoc.RequestUser.Office,
                Rights = userAssoc.RequestUserRightAssocs.Where(rur => !rur.Deleted).
                Select(RequestUserRightModelBy).ToList()
            };
        }

        protected RequestUserRightModel RequestUserRightModelBy(RequestUserRightAssoc rightAssoc)
        {
            return new RequestUserRightModel
            {
                Description = rightAssoc.Descirption, 
                IdResourceRight = rightAssoc.IdResourceRight, 
                ResourceRightName = rightAssoc.ResourceRight.Name, 
                ResourceRightDescription = rightAssoc.ResourceRight.Description, 
                IdRequestRightGrantType = rightAssoc.IdRequestRightGrantType, 
                RequestRightGrantTypeName = rightAssoc.RequestRightGrantType.Name,
                IdResource = rightAssoc.ResourceRight.IdResource,
                ResourceName = rightAssoc.ResourceRight.Resource.Name,
                ResourceDescription = rightAssoc.ResourceRight.Resource.Description
            };
        }

        public virtual TViewModel GetRequestViewModelBy(Request request)
        {
            var viewModel = GetPreRequestViewModel(request.IdRequest);
            viewModel.RequestModel = GetRequestModelBy(request);
            return viewModel;
        }

        public virtual TViewModel GetRequestViewModelBy(RequestModel<TUserModel> request)
        {
            var viewModel = GetPreRequestViewModel(request.IdRequest);
            viewModel.RequestModel = request;
            return viewModel;
        }

        private TViewModel GetPreRequestViewModel(int idRequest)
        {
            var preRequestViewModel = new TViewModel();
            if (default(int) == idRequest)
            {
                return preRequestViewModel;
            }
            var agreements = RequestsRepository.GetRequestAgreements(idRequest).ToList();
            preRequestViewModel.Comments = RequestsRepository.GetRequestExtComments(idRequest);
            preRequestViewModel.SuccessAgreements = GetSuccessAgreements(agreements);
            preRequestViewModel.CancelAgreements = GetCancelAgreements(agreements);
            preRequestViewModel.ExcludedAgreements = GetExcludedAgreements(agreements);
            preRequestViewModel.WaitAgreementUsers = GetWaitAgreementUsers(idRequest, agreements);
            return preRequestViewModel;
        }

        private static IEnumerable<RequestAgreement> GetSuccessAgreements(
            IEnumerable<RequestAgreement> agreements)
        {
            return agreements.Where(r => r.IdAgreementState == 2);
        }

        private static IEnumerable<RequestAgreement> GetCancelAgreements(
            IEnumerable<RequestAgreement> agreements)
        {
            return agreements.Where(r => r.IdAgreementState == 3).ToList();
        }

        private static IEnumerable<RequestAgreement> GetExcludedAgreements(
            IEnumerable<RequestAgreement> agreements)
        {
            return agreements.Where(r => r.IdAgreementState == 4).ToList();
        }

        public IEnumerable<AclUser> GetWaitAgreementUsers(int idRequest,
            IList<RequestAgreement> agreements)
        {
            var request = GetRequestById(idRequest);
            if (request.RequestStates.OrderBy(r => r.IdRequestState).Last(r => !r.Deleted).IdRequestStateType != 1)
            {
                return new List<AclUser>();
            }
            var requestResourceOwners = request.RequestUserAssoc.Where(r => !r.Deleted).
                SelectMany(r => r.RequestUserRightAssocs).Where(r => !r.Deleted).
                SelectMany(r =>
                {
                    var resourceRight = r.ResourceRight ?? _resourceRepository.GetResourceRights()
                        .First(rr => rr.IdResourceRight == r.IdResourceRight);
                    var aclUsers = resourceRight.Resource.OperatorDepartment.AclUsers;
                    var users = resourceRight.Resource.OperatorDepartment.Users.Where(u => 
                        u.AclDepartments == null || 
                        !u.AclDepartments.Any());
                    return aclUsers.Concat(users).Where(u => !u.Deleted && u.Roles.Any(role => role.IdRole == 2));
                }).Where(r => !agreements.Any(a => a.IdAgreementType == 1 && a.IdAgreementState == 4 && a.IdUser == r.IdUser));
            var excludeDepartments = agreements.Where(r => r.IdAgreementType == 1 &&
                    new [] {2,3}.Contains(r.IdAgreementState)).ToList().
                SelectMany(r => 
                    RequestSecurityService.GetUserAllowedDepartments(r.User).Select(u => u.IdDepartment)).
                    Concat(RequestSecurityService.GetUserAllowedDepartments(request.User).Select(u => u.IdDepartment)).
                    Distinct();
            var additionalAgreementUsers =
                agreements.Where(r => r.IdAgreementType == 2 && r.IdAgreementState == 1).Select(r => r.User).
                Where(r => !agreements.Any(a => a.IdAgreementType == 2 && a.IdAgreementState == 4 && a.IdUser == r.IdUser));
            return
                requestResourceOwners.Where(r =>
                {
                    return !excludeDepartments.Any(ed => 
                        RequestSecurityService.GetUserAllowedDepartments(r).
                        Select(d => d.IdDepartment).Contains(ed));
                }).Concat(additionalAgreementUsers).ToList().Distinct();
        }

        public IEnumerable<RequestsCountByStateTypesViewModel> GetRequestsCountByStateTypes()
        {
            var notSeenRequests = from row in GetNotSeenRequests()
                group row.IdRequest by row.RequestStates.Where(r => !r.Deleted)
                    .OrderByDescending(rs => rs.IdRequestState)
                    .FirstOrDefault().IdRequestStateType
                into gs
                select new
                {
                    IdRequestStateType = gs.Key,
                    Count = gs.Count()
                };
            return from requestStateTypesRow in RequestsRepository.GetRequestStateTypes()
                join requestRow in notSeenRequests
                    on requestStateTypesRow.IdRequestStateType equals requestRow.IdRequestStateType into rst
                from rstRow in rst.DefaultIfEmpty()
                select new RequestsCountByStateTypesViewModel
                {
                    RequestStateType = requestStateTypesRow,
                    RequestCount = rstRow == null ? 0 : rstRow.Count
                };
        }
    }
}