using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RequestsForRightsV2.Infrastructure.Enums;
using RequestsForRightsV2.Infrastructure.Services.Interfaces;

namespace RequestsForRightsV2.Infrastructure.Services
{
    public class SecurityService: ISecurityService
    {
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
            return false;
        }
    }
}