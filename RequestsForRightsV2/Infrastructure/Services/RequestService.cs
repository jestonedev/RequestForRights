using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Enums;
using RequestsForRights.Infrastructure.Extensions;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Infrastructure.Helpers;
using RequestsForRights.Models.FilterOptions;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ModelViews;
using AclRole = RequestsForRights.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Infrastructure.Services
{
    public class RequestService<T> : IRequestService<T>
        where T: RequestUserModel, new()
    {
        protected readonly IRequestRepository RequestsRepository;
        protected readonly IRequestSecurityService<T> RequestSecurityService;

        public RequestService(IRequestRepository requestsRepository,
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

        public NotSeenRequestsViewModel GetNotSeenRequestsViewModel()
        {
            var requests = GetNotSeenRequests();
            var requestStateTypes = RequestsRepository.GetRequestStateTypes();
            return new NotSeenRequestsViewModel
            {
                NotSeenRequests = requests,
                RequestStateTypes = requestStateTypes
            };
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
                requests = requests.Where(r => r.RequestStates.OrderByDescending(rs => rs.IdRequestState).FirstOrDefault().IdRequestStateType == filterOptions.IdRequestStateType);
            }
            if (filterOptions.RequestCategory == RequestCategory.MyRequests)
            {
                requests = requests.Where(r => r.User.Login.ToLower() == RequestSecurityService.CurrentUser.ToLower());
            }
            if (filterOptions.DateOfFillingFrom != null)
            {
                requests = requests.Where(r => r.RequestStates.FirstOrDefault().Date >= DbFunctions.TruncateTime(filterOptions.DateOfFillingFrom.Value));
            }
            if (filterOptions.DateOfFillingTo != null)
            {
                requests = requests.Where(r => r.RequestStates.FirstOrDefault().Date <= DbFunctions.AddSeconds(DbFunctions.AddDays(DbFunctions.TruncateTime(filterOptions.DateOfFillingTo.Value), 1), -1));
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
            return requests.Where(r => r.IdRequest.ToString().ToLower().Contains(filter) || r.User.Snp.ToLower().Contains(filter) || r.RequestStates.OrderByDescending(rs => rs.IdRequestState).FirstOrDefault().RequestStateType.Name.ToLower().Contains(filter) || r.RequestType.Name.ToLower().Contains(filter) || r.Description.ToLower().Contains(filter));
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
            return RequestsRepository.InsertRequest(request);
        }

        private static Request ConvertToRequest(RequestModel<T> requestModel)
        {
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
                request.RequestUserAssoc.Add(new RequestUserAssoc
                {
                    RequestUser = requestUser,
                    Request = request
                });
            }
            return request;
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
                }
            };
        }

        public int SaveChanges()
        {
            return RequestsRepository.SaveChanges();
        }

        public virtual RequestViewModel<T> GetRequestViewModelBy(Request request)
        {
            return new RequestViewModel<T>
            {
                RequestModel = GetRequestModelBy(request),
                Comments = GetRequestExtComments(request.IdRequest),
                Agreements = RequestsRepository.GetRequestAgreements(request.IdRequest)
            };
        }

        public virtual IQueryable<RequestExtComment> GetRequestExtComments(int idRequest)
        {
            return RequestsRepository.GetRequestExtComments(idRequest);
        }

        protected T FillRequestUserModel(RequestUserAssoc userAssoc)
        {
            return new T
            {
                Description = userAssoc.Description, Login = userAssoc.RequestUser.Login, 
                Post = userAssoc.RequestUser.Post, Snp = userAssoc.RequestUser.Snp, 
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
                IdResourceRightGrantType = rightAssoc.IdRequestRightGrantType, 
                ResourceRightGrantTypeName = rightAssoc.RequestRightGrantType.Name
            };
        }

        public virtual RequestViewModel<T> GetRequestViewModelBy(RequestModel<T> request)
        {
            return new RequestViewModel<T>
            {
                RequestModel = request, 
                Comments = RequestsRepository.GetRequestExtComments(request.IdRequest), 
                Agreements = RequestsRepository.GetRequestAgreements(request.IdRequest)
            };
        }
    }
}