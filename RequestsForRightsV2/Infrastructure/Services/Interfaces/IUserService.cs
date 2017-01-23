using RequestsForRights.Domain.Entities;
using System.Collections.Generic;

namespace RequestsForRights.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<RequestUser> FindUsers(string snpPattern, int maxCount);
        IEnumerable<Department> GetUnits();
        IEnumerable<Department> GetDepartments();
    }
}