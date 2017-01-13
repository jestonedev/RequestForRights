using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Security.Interfaces;
using AclRole = RequestsForRights.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Infrastructure.Security
{
    public class RequestSecurityService : SecurityService<Request>, IRequestSecurityService
    {

        public RequestSecurityService(ISecurityRepository securityRepository)
            : base(securityRepository)
        {
        }

        public override bool CanRead()
        {
            return InRole(new []
            {
                AclRole.Coordinator,
                AclRole.Administrator, 
                AclRole.Dispatcher, 
                AclRole.Executor, 
                AclRole.Registrar, 
                AclRole.Requester, 
                AclRole.ResourceOwner
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

        public override bool CanDelete(Request entity)
        {
            return InRole(AclRole.Administrator) || 
                (InRole(AclRole.Requester) && 
                 entity.User.Login.ToLower() == CurrentUser.ToLower() &&
                 entity.RequestStates.Last().IdRequestStateType == 1);
        }

        public override bool CanCreate(Request entity)
        {
            return InRole(AclRole.Administrator) ||
                (InRole(AclRole.Requester) && 
                 entity.User.Login.ToLower() == CurrentUser.ToLower());
        }

        public override bool CanUpdate(Request entity)
        {
            return InRole(AclRole.Administrator) ||
                (InRole(AclRole.Requester) && 
                 entity.User.Login.ToLower() == CurrentUser.ToLower() &&
                 entity.RequestStates.Last().IdRequestStateType == 1);
        }

        public IQueryable<Request> FilterRequests(IQueryable<Request> requests)
        {
            var filteredRequests = requests.Where(r => false);
            if (InRole(AclRole.Administrator))
            {
                return requests;
            }
            var allowedDepartments = GetUserAllowedDepartments().Select(r => r.IdDepartment);
            var userInfo = GetUserInfo();
            if (InRole(AclRole.Requester))
            {
                filteredRequests = filteredRequests.Concat(
                    requests.Where(r => allowedDepartments.Any() ?
                        allowedDepartments.Any(d => 
                        d == r.User.Department.IdDepartment) :
                        userInfo.IdDepartment == r.User.Department.IdDepartment));
            }
            if (InRole(AclRole.ResourceOwner))
            {
                filteredRequests = filteredRequests.Concat(
                    requests.Where(r => r.RequestUserAssoc.Any(ru => 
                         !ru.Deleted && ru.RequestUserRightAssocs.Any(rur => 
                         !rur.Deleted && (allowedDepartments.Any() ? 
                            allowedDepartments.Any(d => d
                            == rur.ResourceRight.Resource.IdDepartment) :
                            userInfo.IdDepartment == rur.ResourceRight.Resource.IdDepartment)))));
            }
            if (InRole(new[] {AclRole.Dispatcher, AclRole.Registrar}))
            {
                filteredRequests = filteredRequests.Concat(
                    requests.Where(r => r.RequestStates.Any(
                        rs => !rs.Deleted && rs.IdRequestStateType == 2)));
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
                        ra => ra.User.IdUser == userInfo.IdUser)));
            }
            return filteredRequests.Distinct();
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
    }
}