using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using AclRole = RequestsForRights.Web.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Web.Infrastructure.Security
{
    public class ResourceGroupSecurityService : SecurityService<ResourceGroup>, IResourceGroupSecurityService
    {

        public ResourceGroupSecurityService(ISecurityRepository securityRepository): base(securityRepository)
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