using System;
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

        public ReportSecurityService(ISecurityRepository securityRepository, IUserSecurityService userSecurityService) : 
            base(securityRepository)
        {
            if (userSecurityService == null)
            {
                throw new ArgumentNullException("userSecurityService");
            }
            _userSecurityService = userSecurityService;
        }

        public bool CanReadResourcePermissions()
        {
            return InRole(new[]
            {
                AclRole.Administrator, AclRole.Dispatcher,
                AclRole.ResourceOwner, AclRole.Executor,
                AclRole.Registrar
            });
        }

        public bool CanReadUserPermissions()
        {
            return !IsAnonimous();
        }

        public bool CanReadUserPermissions(RequestUser entity)
        {
            return _userSecurityService.FilterUsers(new[] {entity}.AsQueryable()).Any();
        }
    }
}