using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Enums;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.FilterOptions;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ModelViews;

namespace RequestsForRights.Infrastructure.Services
{
    public class RequestService: IRequestService
    {
        private readonly IRequestRepository _requestsRepository;
        private readonly IRequestSecurityService _requestSecurityService;

        public RequestService(IRequestRepository requestsRepository, IRequestSecurityService requestSecurityService)
        {
            if (requestsRepository == null)
            {
                throw new ArgumentNullException("requestsRepository");
            }
            _requestsRepository = requestsRepository;
            if (requestSecurityService == null)
            {
                throw new ArgumentNullException("requestSecurityService");
            }
            _requestSecurityService = requestSecurityService;
        }

        public IQueryable<Request> GetNotSeenRequests()
        {
            var requests = _requestsRepository.GetRequests();
            var requestsUserLastSeens = _requestsRepository.GetRequestsUserLastSeens(_requestSecurityService.CurrentUser);
            return from request in requests
                join lastSeen in requestsUserLastSeens
                    on request.IdRequest equals lastSeen.IdRequest into joinedRequestsAndLastSeens
                from joinedLastSeensRow in joinedRequestsAndLastSeens.DefaultIfEmpty()
                where joinedLastSeensRow == null
                select request;
        }

        public NotSeenRequestsViewModel GetNotSeenRequestsViewModel()
        {
            var requests = GetNotSeenRequests();
            var requestStateTypes = _requestsRepository.GetRequestStateTypes();
            return new NotSeenRequestsViewModel
            {
                NotSeenRequests = requests,
                RequestStateTypes = requestStateTypes
            };
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
                            return requests.OrderBy(r => r.RequestStates.OrderByDescending(rs => rs.IdRequestState).
                                FirstOrDefault().RequestStateType.Name);
                        case SortDirection.Desc:
                            return requests.OrderByDescending(r => r.RequestStates.OrderByDescending(rs => rs.IdRequestState).
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
                case "IdRequest":
                    switch (sortDirection)
                    {
                        case SortDirection.Asc:
                            return requests.OrderBy(r => r.IdRequest);
                        case SortDirection.Desc:
                            return requests.OrderByDescending(r => r.IdRequest);
                        default:
                            return requests;
                    }
                case "User.Snp":
                    switch (sortDirection)
                    {
                        case SortDirection.Asc:
                            return requests.OrderBy(r => r.User.Snp);
                        case SortDirection.Desc:
                            return requests.OrderByDescending(r => r.User.Snp);
                        default:
                            return requests;
                    }
                case "RequestType.Name":
                    switch (sortDirection)
                    {
                        case SortDirection.Asc:
                            return requests.OrderBy(r => r.RequestType.Name);
                        case SortDirection.Desc:
                            return requests.OrderByDescending(r => r.RequestType.Name);
                        default:
                            return requests;
                    }
                case "Description":
                    switch (sortDirection)
                    {
                        case SortDirection.Asc:
                            return requests.OrderBy(r => r.Description);
                        case SortDirection.Desc:
                            return requests.OrderByDescending(r => r.Description);
                        default:
                            return requests;
                    }
                default:
                    return requests;
            }
        }

        public IQueryable<Request> GetFilteredRequests(RequestsFilterOptions filterOptions)
        {
            var requests = _requestSecurityService.FilterRequests(_requestsRepository.GetRequests());
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
                requests = requests.Where(r => r.User.Login.ToLower() == _requestSecurityService.CurrentUser.ToLower());
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
            return requests.Where(r => r.IdRequest.ToString().ToLower().Contains(filter) || 
                r.User.Snp.ToLower().Contains(filter) || 
                r.RequestStates.OrderByDescending(rs => rs.IdRequestState).
                FirstOrDefault().RequestStateType.Name.ToLower().Contains(filter) || 
                r.RequestType.Name.ToLower().Contains(filter) || 
                r.Description.ToLower().Contains(filter));
        }

        public RequestIndexModelView GetRequestIndexModelView(RequestsFilterOptions filterOptions, IQueryable<Request> filteredRequests)
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
            return new RequestIndexModelView
            {
                VisibleRequests = requests, FilterOptions = filterOptions, RequestCount = filteredRequests.Count(), RequestStateTypes = _requestsRepository.GetRequestStateTypes(), RequestTypes = _requestsRepository.GetRequestTypes(), RequestCatogories = GetRequestCategories()
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
            return _requestsRepository.GetRequestById(idRequest);
        }

        public Request DeleteRequest(int idRequest)
        {
            return _requestsRepository.DeleteRequest(idRequest);
        }

        public int SaveChanges()
        {
            return _requestsRepository.SaveChanges();
        }
    }
}