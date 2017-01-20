﻿using System.Linq;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Models.Models;

namespace RequestsForRights.Infrastructure.Security.Interfaces
{
    public interface IRequestSecurityService<T> : ISecurityService<RequestModel<T>>
        where T: RequestUserModel
    {
        IQueryable<Request> FilterRequests(IQueryable<Request> requests);
        bool CanDelete(Request request);
        bool CanUpdate(Request request);
        bool CanCreate(Request request);
        bool CanRead(Request request);
        bool CanSeeLogin();
        bool CanComment();
        bool CanComment(RequestModel<T> request);
        bool CanComment(Request request);
        bool CanSetRequestState(RequestModel<T> request, int idRequestStateType);
        bool CanSetRequestState(Request request, int idRequestStateType);
        bool CanSetRequestStateGlobal(Request request, int idRequestStateType);
        bool CanAgreement(RequestModel<T> request);
    }
}
