using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Enums;
using RequestsForRights.Infrastructure.Extensions;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.FilterOptions;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ViewModels;
using AclRole = RequestsForRights.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Infrastructure.Services
{
    public class RequestService<T> : IRequestService<T>
        where T: RequestUserModel, new()
    {
        protected readonly IRequestRepository RequestsRepository;
        private readonly IResourceRepository _resourceRepository;
        protected readonly IRequestSecurityService<T> RequestSecurityService;

        public RequestService(IRequestRepository requestsRepository,
            IResourceRepository resourceRepository,
            IRequestSecurityService<T> requestSecurityService)
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
            var requests = RequestsRepository.GetRequests();
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
                            return requests.OrderBy(r => r.RequestStates.FirstOrDefault().Date);
                        case SortDirection.Desc:
                            return requests.OrderByDescending(r => r.RequestStates.FirstOrDefault().Date);
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
                requests = requests.Where(r => r.RequestStates.Where(rs => !rs.Deleted).
                    OrderByDescending(rs => rs.IdRequestState).FirstOrDefault().IdRequestStateType == 
                    filterOptions.IdRequestStateType);
            }
            if (filterOptions.RequestCategory == RequestCategory.MyRequests)
            {
                requests = requests.Where(r => r.User.Login.ToLower() == 
                    RequestSecurityService.CurrentUser.ToLower());
            }
            if (filterOptions.DateOfFillingFrom != null)
            {
                requests = requests.Where(r => 
                    r.RequestStates.FirstOrDefault(rs => !rs.Deleted).Date >= 
                    DbFunctions.TruncateTime(filterOptions.DateOfFillingFrom.Value));
            }
            if (filterOptions.DateOfFillingTo != null)
            {
                requests = requests.Where(r =>
                    r.RequestStates.FirstOrDefault(rs => !rs.Deleted).Date <= 
                    DbFunctions.AddSeconds(DbFunctions.AddDays(DbFunctions.TruncateTime(filterOptions.DateOfFillingTo.Value), 1), -1));
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
                r.RequestType.Name.ToLower().Contains(filter) || r.Description.ToLower().Contains(filter));
        }

        public RequestIndexViewModel GetRequestIndexModelView(RequestsFilterOptions filterOptions, IQueryable<Request> filteredRequests)
        {
            if (filterOptions.SortField == null)
            {
                filterOptions.SortField = "IdRequest";
            }
            var requests = GetVisibleRequests(filterOptions, filteredRequests).ToList();
            if (!requests.Any())
            {
                filterOptions.PageIndex = 0;
                requests = GetVisibleRequests(filterOptions, filteredRequests).ToList();
            }
            return new RequestIndexViewModel
            {
                VisibleRequests = requests, FilterOptions = filterOptions, RequestCount = filteredRequests.Count(), RequestStateTypes = RequestsRepository.GetRequestStateTypes(), RequestTypes = RequestsRepository.GetRequestTypes(), RequestCatogories = GetRequestCategories()
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
                }
            };
        }

        public Request GetRequestById(int idRequest)
        {
            return RequestsRepository.GetRequestById(idRequest);
        }

        public virtual RequestModel<T> GetRequestModelBy(Request request)
        {
            return new RequestModel<T>
            {
                IdRequest = request.IdRequest,
                Description = request.Description,
                IdRequestType = request.IdRequestType,
                Users = request.RequestUserAssoc.Where(ru => !ru.Deleted).
                    Select(FillRequestUserModel).ToList()
            };
        }

        public Request DeleteRequest(int idRequest)
        {
            return RequestsRepository.DeleteRequest(idRequest);
        }

        public virtual Request UpdateRequest(RequestModel<T> requestModel)
        {
            var request = ConvertToRequest(requestModel);
            return RequestsRepository.UpdateRequest(request, 
                !RequestSecurityService.InRole(AclRole.Administrator));
        }

        public virtual Request InsertRequest(RequestModel<T> requestModel)
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

        protected virtual Request ConvertToRequest(RequestModel<T> requestModel)
        {
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

        protected virtual IList<T> ClearUsersDuplicates(IEnumerable<T> users)
        {
            return (from user in users
                group user by new {user.Login, user.Snp, user.Department, user.Unit, user.Description}
                into gs
                select new T
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
            var agreement = new RequestAgreement
            {
                IdRequest = idRequest,
                User = userInfo,
                IdUser = userInfo.IdUser,
                IdAgreementState = idRequestStateType == 2 ? 2 : 3,
                IdAgreementType = 1,
                Description = reason,
                Date = DateTime.Now
            };
            switch (idRequestStateType)
            {
                case 1:
                    RequestsRepository.AddRequestState(requestState, true);
                    break;
                case 2:
                    if (RequestSecurityService.InRole(AclRole.Administrator))
                    {
                        RequestsRepository.AddRequestState(requestState, false);
                        break;
                    }
                    if (RequestSecurityService.InRole(AclRole.Coordinator))
                    {
                        agreement.IdAgreementType = 2;
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
                    RequestsRepository.AddRequestState(requestState, false);
                    if (RequestSecurityService.InRole(AclRole.Coordinator))
                    {
                        agreement.IdAgreementType = 2;
                    }
                    RequestsRepository.UpdateRequestAgreement(agreement);
                    break;
                case 4:
                    RequestsRepository.AddRequestState(new RequestState
                    {
                        IdRequest = idRequest,
                        IdRequestStateType = 3,
                        Date = DateTime.Now
                    }, false);
                    RequestsRepository.AddRequestState(requestState, false);
                    break;
            }
        }

        public void AddCooordinator(int idRequest, Coordinator coordinator)
        {
            var agreement = new RequestAgreement
            {
                IdRequest = idRequest,
                IdAgreementType = 2,
                IdAgreementState = 1,
                User = new AclUser
                {
                    Login = "pwr\\"+coordinator.Login,
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

        private bool NeedAdditionalAgreements(int idRequest, RequestAgreement newAgreement)
        {
            return GetWaitAgreementUsers(idRequest,
                GetRequestAgreements(idRequest).ToList().Concat(new[] {newAgreement}).ToList()).Any();
        }

        public virtual RequestViewModel<T> GetEmptyRequestViewModel()
        {
            return new RequestViewModel<T>
            {
                RequestModel = new RequestModel<T>
                {
                    Users = new List<T>
                    {
                        new T
                        {
                            Rights = new List<RequestUserRightModel>
                            {
                                new RequestUserRightModel()
                            }
                        }
                    }
                },
                Resources = _resourceRepository.GetResources().OrderBy(r => r.Name).ToList(),
                ResourceRights = _resourceRepository.GetResourceRights().OrderBy(r => r.Name).ToList()
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

        protected T FillRequestUserModel(RequestUserAssoc userAssoc)
        {
            return new T
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
                IdRequestRightGrantType = rightAssoc.IdRequestRightGrantType, 
                RequestRightGrantTypeName = rightAssoc.RequestRightGrantType.Name,
                IdResource = rightAssoc.ResourceRight.IdResource,
                ResourceName = rightAssoc.ResourceRight.Resource.Name
            };
        }

        public virtual RequestViewModel<T> GetRequestViewModelBy(Request request)
        {
            var viewModel = GetPreRequestViewModel(request.IdRequest);
            viewModel.RequestModel = GetRequestModelBy(request);
            return viewModel;
        }

        public virtual RequestViewModel<T> GetRequestViewModelBy(RequestModel<T> request)
        {
            var viewModel = GetPreRequestViewModel(request.IdRequest);
            viewModel.RequestModel = request;
            return viewModel;
        }

        private RequestViewModel<T> GetPreRequestViewModel(int idRequest)
        {
            var preRequestViewModel = new RequestViewModel<T>
            {
                Resources = _resourceRepository.GetResources().OrderBy(r => r.Name).ToList(),
                ResourceRights = _resourceRepository.GetResourceRights().OrderBy(r => r.Name).ToList()
            };
            if (default(int) == idRequest)
            {
                return preRequestViewModel;
            }
            var agreements = RequestsRepository.GetRequestAgreements(idRequest).ToList();
            preRequestViewModel.Comments = RequestsRepository.GetRequestExtComments(idRequest);
            preRequestViewModel.SuccessAgreements = GetSuccessAgreements(agreements);
            preRequestViewModel.CancelAgreements = GetCancelAgreements(agreements);
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

        private IEnumerable<AclUser> GetWaitAgreementUsers(int idRequest,
            IList<RequestAgreement> agreements)
        {
            var request = GetRequestById(idRequest);
            if (request.RequestStates.Last(r => !r.Deleted).IdRequestStateType != 1)
            {
                return new List<AclUser>();
            }
            var requestResourceOwners = request.RequestUserAssoc.SelectMany(r => r.RequestUserRightAssocs).
                SelectMany(r =>
                {
                    var aclUsers = r.ResourceRight.Resource.Department.AclUsers;
                    var users = r.ResourceRight.Resource.Department.Users.Where(u => 
                        u.AclDepartments == null || 
                        !u.AclDepartments.Any());
                    return aclUsers.Concat(users).Where(u => u.Roles.Any(role => role.IdRole == 2));
                });
            var excludeDepartments = agreements.Where(r => r.IdAgreementType == 1 &&
                                                           new[] {2, 3}.Contains(r.IdAgreementState)).
                SelectMany(r => 
                    RequestSecurityService.GetUserAllowedDepartments(r.User).Select(u => u.IdDepartment)).
                    Concat(RequestSecurityService.GetUserAllowedDepartments(request.User).Select(u => u.IdDepartment)).
                    Distinct();
            var additionalAgreementUsers =
                agreements.Where(r => r.IdAgreementType == 2 && r.IdAgreementState == 1).Select(r => r.User);
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
                group row.IdRequest by row.RequestStates.OrderByDescending(rs => rs.IdRequestState).
                    FirstOrDefault().IdRequestStateType
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
                    RequestCount = rstRow == null ? 0 : (rstRow.Count > 99 ? 99 : rstRow.Count)
                };
        }
    }
}