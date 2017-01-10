using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRightsV2.Infrastructure.Security.Interfaces;
using AclRole = RequestsForRightsV2.Infrastructure.Enums.AclRole;

namespace RequestsForRightsV2.Infrastructure.Security
{
    public class ResourceSecurityService : SecurityService<Resource>, IResourceSecurityService
    {

        public ResourceSecurityService(ISecurityRepository securityRepository)
            : base(securityRepository)
        {
        }

        public override bool CanRead()
        {
            return !IsAnonimous();
        }

        public override bool CanModify()
        {
            return InRole(new[] { AclRole.Administrator, AclRole.ResourceManager });
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