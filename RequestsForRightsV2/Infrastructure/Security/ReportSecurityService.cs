using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Security.Interfaces;
using AclRole = RequestsForRights.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Infrastructure.Security
{
    public class ReportSecurityService: SecurityService<RequestUser>, IReportSecurityService
    {
        private readonly IUserSecurityService _userSecurityService;

        public ReportSecurityService(
            ISecurityRepository securityRepository, 
            IUserSecurityService userSecurityService) : 
            base(securityRepository)
        {
            if (userSecurityService == null)
            {
                throw new ArgumentNullException("userSecurityService");
            }
            _userSecurityService = userSecurityService;
        }

        public IQueryable<Resource> FilterResources(IQueryable<Resource> resources)
        {
            if (InRole(new[]
            {
                AclRole.Administrator, AclRole.Dispatcher,
                AclRole.Executor, AclRole.Registrar, 
                AclRole.ResourceManager, 
            }))
            {
                return resources;
            }
            if (InRole(AclRole.ResourceOwner))
            {
                var allowedDepartments = GetUserAllowedDepartments().Select(r => r.IdDepartment);
                return resources.Where(r => allowedDepartments.Contains(r.IdDepartment));
            }
            return new List<Resource>().AsQueryable();
        }

        public bool CanReadResourcePermissions()
        {
            return InRole(new[]
            {
                AclRole.Administrator, AclRole.Dispatcher,
                AclRole.ResourceOwner, AclRole.Executor,
                AclRole.Registrar, AclRole.ResourceManager, 
            });
        }

        public bool CanReadUserPermissions()
        {
            return InRole(new[]
            {
                AclRole.Administrator, AclRole.Dispatcher,
                AclRole.Executor, AclRole.Registrar, 
                AclRole.Requester, AclRole.ResourceManager, 
            });
        }

        public bool CanReadUserPermissions(RequestUser entity)
        {
            return _userSecurityService.FilterUsers(new[] {entity}.AsQueryable()).Any();
        }
    }
}