using System;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public UserRepository(IDatabaseContext databaseContext)
        {
            if (databaseContext == null)
            {
                throw new ArgumentNullException("databaseContext");
            }
            _databaseContext = databaseContext;
        }

        public IQueryable<RequestUser> FindUsers(string snpPattern)
        {
            return _databaseContext.Users.Where(r => !r.Deleted && 
                r.Snp.Contains(snpPattern));
        }

        public IQueryable<Department> GetDepartments()
        {
            return _databaseContext.Departments.Where(r => !r.Deleted && r.IdParentDepartment == null);
        }

        public IQueryable<Department> GetUnits()
        {
            return _databaseContext.Departments.Where(r => !r.Deleted && r.IdParentDepartment != null);
        }
    }
}
