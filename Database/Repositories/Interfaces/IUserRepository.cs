using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<RequestUser> FindUsers(string snpPattern);
    }
}
