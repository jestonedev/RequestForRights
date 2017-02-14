using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<RequestUser> FindUsers(string snpPattern);
        IQueryable<Department> GetDepartments();
        IQueryable<Department> GetUnits();
        IQueryable<RequestState> GetRequestStates();
        IQueryable<RequestUserAssoc> GetRequestUserAssocs();
        IQueryable<Request> GetRequests();
        RequestUser FindUser(RequestUser requestUser);
    }
}
