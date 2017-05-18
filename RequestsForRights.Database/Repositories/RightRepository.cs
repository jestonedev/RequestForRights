using System;
using System.Data.Entity;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories
{
    public class RightRepository: IRightRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public RightRepository(IDatabaseContext databaseContext)
        {
            if (databaseContext == null)
            {
                throw new ArgumentNullException("databaseContext");
            }
            _databaseContext = databaseContext;
        }

        public IQueryable<Request> GetRequests()
        {
            return _databaseContext.Requests.Where(r => !r.Deleted);
        }

        public IQueryable<RequestState> GetRequestStates()
        {
            return _databaseContext.RequestStates.OrderBy(rs => rs.IdRequestState).Where(r => !r.Deleted);
        }

        public IQueryable<RequestUserAssoc> GetRequestUserAssocs()
        {
            return _databaseContext.RequestUserAssocs.Where(r => !r.Deleted);
        }

        public IQueryable<RequestUserRightAssoc> GetRequestUserRightAssocs()
        {
            return _databaseContext.RequestUserRightAssocs.Where(r => !r.Deleted).Include(r => r.ResourceRight);
        }

        public IQueryable<DelegationRequestUsersExtInfo> GetDelegationRequestUsersExtInfo()
        {
            return _databaseContext.DelegationRequestUsersExtInfo.Where(r => !r.Deleted);
        }

        public IQueryable<RequestUser> GetRequestUsers()
        {
            return _databaseContext.Users.Where(r => !r.Deleted);
        }

        public IQueryable<ResourceRight> GetResourceRights()
        {
            return _databaseContext.ResourceRights.Include(r => r.Resource).Where(r => !r.Deleted);
        }
    }
}
