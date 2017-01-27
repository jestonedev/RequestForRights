using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories.Interfaces
{
    public interface IReportRepository
    {
        IQueryable<Resource> GetResources();
        IQueryable<RequestUser> GetUsers();
    }
}
