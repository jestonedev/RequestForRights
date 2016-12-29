using RequestsForRightsV2.Infrastructure.Enums;

namespace RequestsForRightsV2.Infrastructure.Services.Interfaces
{
    public interface ISecurityService
    {
        string CurrentUser { get; }

        bool InRole(AclRole role);
    }
}
