using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Models.Models;
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
                AclRole.ResourceManager, AclRole.Requester
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
                AclRole.ResourceManager,
                AclRole.Requester
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

        public bool CanReadAclUserRights()
        {
            return InRole(new[]
            {
                AclRole.Administrator, 
                AclRole.Dispatcher, 
                AclRole.Registrar, 
                AclRole.Executor
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

        public IEnumerable<ResourceUserRightModel> FilterResourceRights(IEnumerable<ResourceUserRightModel> resourceRights)
        {
            if (InRole(new[]
            {
                AclRole.Administrator, AclRole.Dispatcher, 
                AclRole.ResourceManager, AclRole.Executor
            }))
            {
                return resourceRights;
            }
            resourceRights = resourceRights.ToList();
            IEnumerable<ResourceUserRightModel> resultResourceRights = new List<ResourceUserRightModel>();
            if (InRole(AclRole.ResourceOperator))
            {
                var allowedResources = GetUserAllowedDepartments().SelectMany(d => d.OperatorResources).ToList();
                resultResourceRights = resultResourceRights.Union(resourceRights.Where(rr => allowedResources.Any(r => r.IdResource == rr.IdResource)));
            }
            if (InRole(AclRole.Requester))
            {
                var allowedDepartments = GetUserAllowedDepartments().ToList();
                resultResourceRights = resultResourceRights.Union(resourceRights.Where(r => 
                    allowedDepartments.Any(d => d.ParentDepartment == null ? 
                        d.Name == r.RequestUserDepartment : 
                        d.ParentDepartment.Name == r.RequestUserDepartment && d.Name == r.RequestUserUnit) ||
                    (r.IdDelegateFromUser != null &&
                    allowedDepartments.Any(d => d.ParentDepartment == null ?
                        d.Name == r.DelegateFromUserDepartment :
                        d.ParentDepartment.Name == r.DelegateFromUserDepartment && d.Name == r.DelegateFromUserUnit)
                    )));
            }
            return resultResourceRights.Distinct();
        }

        public IQueryable<Department> FilterDepartments(IQueryable<Department> departments)
        {
            if (InRole(new[]
            {
                AclRole.Administrator, AclRole.Dispatcher,
                AclRole.Executor, AclRole.Registrar, 
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