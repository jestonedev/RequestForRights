using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Infrastructure.Security.Interfaces
{
    public interface IUserSecurityService: ISecurityService<RequestUser>
    {
        IQueryable<RequestUser> FilterUsers(IQueryable<RequestUser> users);
    }
}
