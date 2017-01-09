using System;
using System.Linq;
using System.Web;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRightsV2.Infrastructure.Enums;
using RequestsForRightsV2.Infrastructure.Services.Interfaces;

namespace RequestsForRightsV2.Infrastructure.Services
{
    public class SecurityService: ISecurityService
    {
        private readonly ISecurityRepository _securityRepository;

        public SecurityService(ISecurityRepository securityRepository)
        {
            if (securityRepository == null)
            {
                throw new ArgumentNullException("securityRepository");
            }
            _securityRepository = securityRepository;
        }

        public string CurrentUser
        {
            get
            {
                return HttpContext.Current != null
                       && HttpContext.Current.User != null
                       && HttpContext.Current.User.Identity != null
                    ? HttpContext.Current.User.Identity.Name
                    : null;
            }
        }

        public bool InRole(AclRole role)
        {
            return _securityRepository.GetUserRoles(CurrentUser).Any(r => r.IdRole == (int) role);
        }
    }
}