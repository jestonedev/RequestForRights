using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Infrastructure.Security.Interfaces
{
    public interface IReportSecurityService: ISecurityService<RequestUser>
    {
        bool CanReadResourcePermissions();
        bool CanReadUserPermissions();
        bool CanReadUserPermissions(RequestUser requestUser);
    }
}
