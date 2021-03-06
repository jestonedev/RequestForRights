﻿using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Models.Models;
using AclRole = RequestsForRights.Web.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Web.Infrastructure.Security
{
    public class RequestSecurityService<T> : SecurityService<RequestModel<T>>, IRequestSecurityService<T>
        where T: RequestUserModel
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IResourceRepository _resourceRepository;

        public RequestSecurityService(ISecurityRepository securityRepository,
            IRequestRepository requestRepository, IResourceRepository resourceRepository)
            : base(securityRepository)
        {
            if (requestRepository == null)
            {
                throw new ArgumentNullException("requestRepository");
            }
            _requestRepository = requestRepository;
            if (resourceRepository == null)
            {
                throw new ArgumentNullException("resourceRepository");
            }
            _resourceRepository = resourceRepository;
        }

        public bool CanRead(Request request)
        {
            var result = FilterRequests(new List<Request> { request }.AsQueryable());
            return result.Any();
        }

        public override bool CanRead(RequestModel<T> entity)
        {
            var request = _requestRepository.GetRequestById(entity.IdRequest);
            return CanRead(request);
        }

        public bool CanDelete(Request request)
        {
            return InRole(AclRole.Administrator) ||
                   (InRole(AclRole.Requester) &&
                    request.IdCurrentRequestStateType == 1 &&
                    request.RequestStates.Where(r => !r.Deleted).All(r => r.IdRequestStateType != 2) &&
                    GetUserAllowedDepartments(request.User)
                        .Any(r => GetUserAllowedDepartments(GetUserInfo())
                            .Any(cu => r.IdDepartment == cu.IdDepartment)));
        }

        public override bool CanDelete(RequestModel<T> entity)
        {
            var request = _requestRepository.GetRequestById(entity.IdRequest);
            return CanDelete(request);
        }

        public bool CanCreate(Request request)
        {
            return InRole(new[]
            {
                AclRole.Administrator,
                AclRole.Requester
            });
        }

        public override bool CanCreate(RequestModel<T> entity)
        {
            return CanCreate(null);
        }

        public bool CanUpdate(Request request)
        {
            return InRole(AclRole.Administrator) ||
                   (InRole(AclRole.Requester) &&
                    request.IdCurrentRequestStateType == 1 &&
                    request.RequestStates.Where(r => !r.Deleted)
                        .All(r => r.IdRequestStateType != 2) &&
                    GetUserAllowedDepartments(request.User)
                        .Any(r => GetUserAllowedDepartments(GetUserInfo())
                            .Any(cu => r.IdDepartment == cu.IdDepartment)));
        }

        public override bool CanUpdate(RequestModel<T> entity)
        {
            var request = _requestRepository.GetRequestById(entity.IdRequest);
            return CanUpdate(request);
        }

        public IQueryable<Request> FilterRequests(IQueryable<Request> requests)
        {
            var filteredRequests = requests.Where(r => false);
            requests = requests.Where(r => !r.Deleted);
            if (InRole(new[] { AclRole.Administrator, AclRole.Dispatcher, AclRole.Registrar }))
            {
                return requests;
            }
            var allowedDepartments = GetUserAllowedDepartments().Select(r => r.IdDepartment).ToList();
            var userInfo = GetUserInfo();
            if (userInfo == null)
            {
                return filteredRequests;
            }
            if (InRole(AclRole.ResourceOperator) && InRole(AclRole.Requester))
            {
                filteredRequests = filteredRequests.Concat(
                    from row in requests
                    where row.IdUser == userInfo.IdUser ||
                          allowedDepartments.Any(ad => row.User.AclDepartments.Any()
                              ? row.User.AclDepartments.Any(acld => acld.IdDepartment == ad)
                              : row.User.IdDepartment == ad) ||
                          row.RequestUserAssoc.Any(ru =>
                              !ru.Deleted && ru.RequestUserRightAssocs.Any(rur =>
                                  !rur.Deleted &&
                                  allowedDepartments.Any(d => d == rur.ResourceRight.Resource.IdOperatorDepartment)))
                    select row);
            } else
            if (InRole(AclRole.Requester))
            {
                filteredRequests = filteredRequests.Concat(
                    from row in requests
                    where row.IdUser == userInfo.IdUser ||
                        allowedDepartments.Any(ad => row.User.AclDepartments.Any() ? 
                            row.User.AclDepartments.Any(acld => acld.IdDepartment == ad) : 
                            row.User.IdDepartment == ad)
                    select row);
            } else
            if (InRole(AclRole.ResourceOperator))
            {
                filteredRequests = filteredRequests.Concat(
                    requests.Where(r => r.RequestUserAssoc.Any(ru =>
                        !ru.Deleted && ru.RequestUserRightAssocs.Any(rur =>
                            !rur.Deleted &&
                            allowedDepartments.Any(d => d == rur.ResourceRight.Resource.IdOperatorDepartment)))));

            }
            if (InRole(AclRole.Executor))
            {
                filteredRequests = filteredRequests.Concat(
                    requests.Where(r => r.RequestStates.Any(
                        rs => !rs.Deleted && rs.IdRequestStateType == 3)));
            }
            if (InRole(AclRole.Coordinator))
            {
                filteredRequests = filteredRequests.Concat(
                    requests.Where(r => r.RequestAgreements.Any(
                        ra => ra.User.IdUser == userInfo.IdUser && ra.IdAgreementType == 2)));
            }
            return filteredRequests.Distinct();
        }

        public IQueryable<Resource> FilterResources(IQueryable<Resource> resources)
        {
            if (InRole(new[]
            {
                AclRole.Administrator, AclRole.Dispatcher,
                AclRole.Executor, 
                AclRole.ResourceManager
            }))
            {
                return resources;
            }
            if (InRole(AclRole.Requester))
            {
                var allowedDepartments = GetUserAllowedDepartments().Select(r => r.IdDepartment);
                return resources.Where(r =>
                    !r.RequestAllowedDepartments.Any() ||
                    r.RequestAllowedDepartments.Select(rd => rd.IdDepartment).Intersect(allowedDepartments).Any() ||
                    r.RequestAllowedDepartments.SelectMany(rd => rd.ChildDepartments).Select(rd => rd.IdDepartment).Intersect(allowedDepartments).Any());
            }
            return new List<Resource>().AsQueryable();
        }

        public override bool CanRead()
        {
            return InRole(new[]
            {
                AclRole.Coordinator,
                AclRole.Administrator, 
                AclRole.Dispatcher, 
                AclRole.Executor, 
                AclRole.Registrar, 
                AclRole.Requester, 
                AclRole.ResourceOperator
            });
        }

        public override bool CanModify()
        {
            return InRole(new[]
            {
                AclRole.Administrator, 
                AclRole.Requester
            });
        }

        public override bool CanUpdate()
        {
            return CanModify();
        }

        public override bool CanDelete()
        {
            return CanModify();
        }

        public override bool CanCreate()
        {
            return CanModify();
        }

        public bool CanSeeLogin()
        {
            return InRole(new[] { AclRole.Executor, AclRole.Administrator });
        }

        public bool CanSendTransferUserNotification()
        {
            return InRole(new[] {AclRole.Administrator, AclRole.Dispatcher});
        }

        public bool CanComment()
        {
            return CanRead();
        }

        public bool CanComment(RequestModel<T> entity)
        {
            var request = _requestRepository.GetRequestById(entity.IdRequest);
            return CanComment(request);
        }

        public bool CanComment(Request request)
        {
            return CanRead(request);
        }

        public bool CanAddCoordinator()
        {
            return InRole(new [] {AclRole.Dispatcher, AclRole.Administrator });
        }

        public bool CanAddCoordinator(RequestModel<T> entity)
        {
            var request = _requestRepository.GetRequestById(entity.IdRequest);
            return CanAddCoordinator(request);
        }

        public bool CanAddCoordinator(Request request)
        {
            return InRole(new[] { AclRole.Dispatcher, AclRole.Administrator }) && 
                new[] { 1, 2 }.Contains(request.IdCurrentRequestStateType ?? 0);
        }

        public bool CanExcludeAgreementor(RequestModel<T> entity)
        {
            var request = _requestRepository.GetRequestById(entity.IdRequest);
            return CanExcludeAgreementor(request);
        }

        public bool CanExcludeAgreementor(Request request)
        {
            return InRole(new[] { AclRole.Dispatcher, AclRole.Administrator }) &&
                new[] { 1, 2 }.Contains(request.IdCurrentRequestStateType ?? 0);
        }

        public bool CanAcceptCancelRequest(RequestModel<T> entity)
        {
            var request = _requestRepository.GetRequestById(entity.IdRequest);
            return CanAcceptCancelRequest(request);
        }

        public bool CanAcceptCancelRequest(Request request)
        {
            return InRole(new[] { AclRole.Dispatcher, AclRole.Administrator }) &&
                 new[] { 1, 2 }.Contains(request.IdCurrentRequestStateType ?? 0) && 
                 request.RequestAgreements.Any(r => r.IdAgreementType == 2 && r.IdAgreementState == 3);
        }

        public bool CanSetRequestState(Request request, int idRequestStateType)
        {
            if (request == null)
            {
                return false;
            }
            if (request.IdCurrentRequestStateType == idRequestStateType)
            {
                return false;
            }
            switch (idRequestStateType)
            {
                case 1:
                    return InRole(AclRole.Administrator) &&
                           CanSetRequestStateGlobal(request, idRequestStateType);
                case 2:
                    return InRole(AclRole.Administrator) ||
                           (InRole(AclRole.ResourceOperator) &&
                            ResourceOperatorCanSetRequestState(request)) ||
                           (InRole(AclRole.Coordinator) &&
                            CoordinatorCanSetRequestState(request));
                case 3:
                    return InRole(AclRole.Administrator) ||
                           (InRole(AclRole.Dispatcher) &&
                            DispatcherCanSetRequestState(request));
                case 4:
                    return InRole(AclRole.Administrator);
                case 5:
                    return InRole(AclRole.Administrator) ||
                           (InRole(AclRole.ResourceOperator) &&
                            ResourceOperatorCanSetRequestState(request)) ||
                           (InRole(AclRole.Coordinator) &&
                            CoordinatorCanSetRequestState(request)) ||
                           (InRole(AclRole.Dispatcher) &&
                            DispatcherCanSetRequestState(request));
            }
            return false;
        }

        public bool CanSetRequestStateGlobal(Request request, int idRequestStateType)
        {
            if (idRequestStateType != 1) return true;
            var idResourceRights = request.RequestUserAssoc.Where(r => !r.Deleted && 
                    r.RequestUserRightAssocs != null)
                .SelectMany(r => r.RequestUserRightAssocs.Where(rur => !rur.Deleted))
                .Select(r => r.IdResourceRight).ToList();
            if (!idResourceRights.Any())
            {
                return false;
            }
           var resourceDepartments = _resourceRepository.GetResourceRights().Where(r => !r.Deleted &&
                idResourceRights.Any(idResourceRight => idResourceRight == r.IdResourceRight)).
                Select(r => r.Resource.IdOperatorDepartment).Where(r => r != 24);
            var allowedDepartments = GetUserAllowedDepartments(request.User).Select(r => r.IdDepartment);
            var ownerDepartments = GetUsersBy(AclRole.ResourceOperator).ToList().
                SelectMany(GetUserAllowedDepartments).Select(r => r.IdDepartment).ToList();
            return resourceDepartments.Any(dep => !allowedDepartments.Contains(dep) && ownerDepartments.Contains(dep));
        }

        public bool CanSetRequestState(RequestModel<T> entity, int idRequestStateType)
        {
            var request = _requestRepository.GetRequestById(entity.IdRequest);
            return CanSetRequestState(request, idRequestStateType);
        }

        private static bool DispatcherCanSetRequestState(Request request)
        {
            return
                request.RequestStates.Any(r => !r.Deleted && new [] {1,2}.Contains(r.IdRequestStateType)) &&
                new[] {1, 2}.Contains(request.IdCurrentRequestStateType ?? 0);
        }

        private bool ResourceOperatorCanSetRequestState(Request request)
        {
            var isFirstState = request.IdCurrentRequestStateType == 1;
            if (!isFirstState)
            {
                return false;
            }
            if (request.RequestAgreements.Any(r => r.IdAgreementState == 4 && r.IdUser == GetUserInfo().IdUser))
            {
                return false;
            }
            var allowedDepartments = GetUserAllowedDepartments().Select(r => r.IdDepartment);
            var resourceDepartments = request.RequestUserAssoc.
                Where(ru => !ru.Deleted && ru.RequestUserRightAssocs != null).
                SelectMany(ru => ru.RequestUserRightAssocs.Where(r => !r.Deleted).Select(
                    r => r.ResourceRight.Resource.IdOperatorDepartment)).Distinct();
            resourceDepartments = resourceDepartments.Except(new[] { 24 }).Except(
                GetUserAllowedDepartments(request.User).Select(r => r.IdDepartment));
            var agreementDepartments = request.RequestAgreements
                .Where(r => r.IdAgreementType == 1 && new[] {2, 3}.Contains(r.IdAgreementState))
                .SelectMany(r =>
                    GetUserAllowedDepartments(r.User)).Select(r => r.IdDepartment);
            return resourceDepartments.Intersect(allowedDepartments).Except(agreementDepartments).Any();
        }

        private bool CoordinatorCanSetRequestState(Request request)
        {
            var userInfo = GetUserInfo();
            if (userInfo == null)
            {
                return false;
            }
            return
                request.IdCurrentRequestStateType == 1 &&
                request.RequestAgreements.Any(r =>
                    r.IdUser == userInfo.IdUser &&
                    r.IdAgreementType == 2 && r.IdAgreementState == 1);
        }

        public bool CanAgreement(RequestModel<T> entity)
        {
            var request = _requestRepository.GetRequestById(entity.IdRequest);
            if (request.Deleted)
            {
                return false;
            }
            foreach (var idRequestStateType in 
                _requestRepository.GetRequestStateTypes().Select(r => r.IdRequestStateType))
            {
                var can = CanSetRequestState(request, idRequestStateType);
                if (can)
                {
                    return true;
                }
            }
            return CanAddCoordinator(entity);
        }

        public bool CanVisibleUser(RequestModel<T> entity, T user)
        {
            return user.Rights.Any(r => CanVisibleRight(entity, r));
        }

        public bool CanVisibleRight(RequestModel<T> entity, RequestUserRightModel right)
        {
            if (InRole(new[]
            {
                AclRole.Administrator,
                AclRole.Dispatcher,
                AclRole.Executor,
                AclRole.Registrar
            }))
            {
                return true;
            }
            var allowedDepartments = GetUserAllowedDepartments().Select(r => r.IdDepartment).ToList();
            if (InRole(AclRole.Requester))
            {
                var request = _requestRepository.GetRequestById(entity.IdRequest);
                if (request.IdUser == GetUserInfo().IdUser ||
                    allowedDepartments.Any(r =>
                    (request.User.AclDepartments.Any() ?
                            request.User.AclDepartments :
                            new[] { request.User.Department }).Select(d => d.IdDepartment).
                            Any(req => req == r)
                   
                    ))
                {
                    return true;
                }
            }
            if (InRole(AclRole.Coordinator))
            {
                var request = _requestRepository.GetRequestById(entity.IdRequest);
                if (request.RequestAgreements.Any(r => r.IdUser == GetUserInfo().IdUser 
                    && r.IdAgreementType == 2 
                    && r.IdRequest == entity.IdRequest))
                {
                    return true;
                }
            }
            if (!InRole(AclRole.ResourceOperator)) return false;
            var resource = _resourceRepository.GetResourceById(right.IdResource);
            return allowedDepartments.Contains(resource.IdOperatorDepartment);
        }

        public bool CanViewAgreementSendDescription(RequestAgreement agreement)
        {
            return InRole(new [] { AclRole.Administrator, AclRole.Dispatcher, AclRole.Registrar  }) ||
                (InRole(AclRole.Coordinator) && agreement.IdUser == GetUserInfo().IdUser);
        }

        public bool CanVisibleExecutors()
        {
            return InRole(new[]
            {
                AclRole.Administrator, 
                AclRole.Dispatcher, 
                AclRole.Registrar, 
                AclRole.Executor
            });
        }
    }
}