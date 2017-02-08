using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Security.Interfaces;
using AclRole = RequestsForRights.Infrastructure.Enums.AclRole;

namespace RequestsForRights.Infrastructure.Security
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

        public override bool CanUpdate(Resource entity)
        {
            return CanUpdate() && entity != null && !entity.Deleted;
        }

        public override bool CanRead(Resource entity)
        {
            return CanRead() && entity != null && !entity.Deleted;
        }
    }
}