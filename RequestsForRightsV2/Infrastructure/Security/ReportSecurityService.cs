using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using AclRole = RequestsForRights.Web.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Web.Infrastructure.Security
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
                AclRole.Executor, 
                AclRole.ResourceManager
            }))
            {
                return resources;
            }
            if (InRole(AclRole.ResourceOperator))
            {
                var allowedDepartments = GetUserAllowedDepartments().Select(r => r.IdDepartment);
                return resources.Where(r => 
                    (!r.RequestAllowedDepartments.Any() ||
                    r.RequestAllowedDepartments.Select(rd => rd.IdDepartment).Intersect(allowedDepartments).Any()) &&
                    allowedDepartments.Contains(r.IdOperatorDepartment));
            }
            return new List<Resource>().AsQueryable();
        }

        public bool CanReadResourcePermissions()
        {
            return InRole(new[]
            {
                AclRole.Administrator, AclRole.Dispatcher,
                AclRole.ResourceOperator, AclRole.Executor,
                AclRole.ResourceManager
            });
        }

        public bool CanReadUserPermissions()
        {
            return InRole(new[]
            {
                AclRole.Administrator, AclRole.Dispatcher,
                AclRole.Executor,
                AclRole.Requester, AclRole.ResourceManager, 
            });
        }

        public bool CanReadUserPermissions(RequestUser entity)
        {
            return _userSecurityService.FilterUsers(new[] {entity}.AsQueryable()).Any();
        }

        public bool CanReadDepartmentPermissions()
        {
            return CanReadUserPermissions();
        }

        public bool CanReadDepartmentAndResourcePermissions()
        {
            return CanReadResourcePermissions();
        }

        public bool CanReadResourceOperatorInfo()
        {
            return InRole(new []
            {
                AclRole.Administrator, 
                AclRole.Dispatcher, 
                AclRole.ResourceManager, 
                AclRole.Registrar,
                AclRole.ResourceViewer
            });
        }

        public bool CanVisiblieAllDepartmentsMark()
        {
            return InRole(new[]
            {
                AclRole.Administrator, 
                AclRole.Dispatcher, 
                AclRole.ResourceManager,
                AclRole.Executor, 
                AclRole.Registrar,
                AclRole.ResourceViewer
            });
        }

        public IQueryable<Department> FilterDepartments(IQueryable<Department> departments)
        {
            if (InRole(new[]
            {
                AclRole.Administrator, AclRole.Dispatcher,
                AclRole.Executor, 
                AclRole.ResourceManager, AclRole.ResourceViewer
            }))
            {
                return departments;
            }
            if (InRole(AclRole.Requester))
            {
                var allowedDepartments = GetUserAllowedDepartments().Select(r => r.IdDepartment);
                return departments.Where(r => allowedDepartments.Contains(r.IdDepartment));
            }
            return new List<Department>().AsQueryable();
        }
    }
}