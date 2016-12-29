using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRightsV2.Infrastructure.Services.Interfaces;
using RequestsForRightsV2.Models;

namespace RequestsForRightsV2.Infrastructure.Services
{
    public class RequestService: IRequestService
    {
        private readonly IRequestRepository _requestsRepository;
        private readonly ISecurityService _securityService;

        public RequestService(IRequestRepository requestsRepository, ISecurityService securityService)
        {
            if (requestsRepository == null)
            {
                throw new ArgumentNullException("requestsRepository");
            }
            _requestsRepository = requestsRepository;
            if (securityService == null)
            {
                throw new ArgumentNullException("securityService");
            }
            _securityService = securityService;
        }

        public IEnumerable<Request> GetNotSeenRequests()
        {
            var requests = _requestsRepository.GetRequests();
            var requestsUserLastSeens = _requestsRepository.GetRequestsUserLastSeens(_securityService.CurrentUser);
            return from request in requests
                join lastSeen in requestsUserLastSeens
                    on request.IdRequest equals lastSeen.IdRequest into joinedRequestsAndLastSeens
                from joinedLastSeensRow in joinedRequestsAndLastSeens.DefaultIfEmpty()
                where joinedLastSeensRow == null
                select request;
        }

        public IEnumerable<NotSeenRequestsByState> GetNotSeenRequestsByStates()
        {
            var requests = GetNotSeenRequests();
            var requestStates = _requestsRepository.GetRequestStates().ToList();
            return from requestState in requestStates
                select new NotSeenRequestsByState
                {
                    RequestState = requestState,
                    NotSeenRequests = requests.Where(r => r.IdRequestState == requestState.IdRequestState)
                };
        }
    }
}