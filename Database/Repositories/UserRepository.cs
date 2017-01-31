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

        public IQueryable<Request> GetRequests()
        {
            return _databaseContext.Requests.Where(r => !r.Deleted);
        }

        public IQueryable<RequestState> GetRequestStates()
        {
            return _databaseContext.RequestStates.Where(r => !r.Deleted);
        }

        public IQueryable<RequestUserAssoc> GetRequestUserAssocs()
        {
            return _databaseContext.RequestUserAssocs.Where(r => !r.Deleted);
        }

        public IQueryable<Department> GetDepartments()
        {
            return _databaseContext.Departments.Where(r => !r.Deleted && r.IdParentDepartment == null);
        }

        public IQueryable<Department> GetUnits()
        {
            return _databaseContext.Departments.Where(r => !r.Deleted && r.IdParentDepartment != null);
        }

        public RequestUser FindUser(RequestUser requestUser)
        {
            requestUser.Login = string.IsNullOrEmpty(requestUser.Login) ? null : requestUser.Login;
            requestUser.Snp = string.IsNullOrEmpty(requestUser.Snp) ? null : requestUser.Snp;
            requestUser.Department = string.IsNullOrEmpty(requestUser.Department) ? null : requestUser.Department;
            requestUser.Unit = string.IsNullOrEmpty(requestUser.Unit) ? null : requestUser.Unit;

            return _databaseContext.Users.FirstOrDefault(
                r => !r.Deleted && (requestUser.Login != null
                ? r.Login.ToLower() == requestUser.Login.ToLower()
                : r.Snp == requestUser.Snp &&
                  r.Department == requestUser.Department &&
                  r.Unit == requestUser.Unit));
        }
    }
}
