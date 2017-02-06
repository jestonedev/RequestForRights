using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database
{
    public interface IDatabaseContext
    {
        IDbSet<ResourceRight> ResourceRights { get; set; }
        IDbSet<Resource> Resources { get; set; }
        IDbSet<ResourceGroup> ResourceGroups { get; set; }
        IDbSet<Department> Departments { get; set; }
        IDbSet<AclUser> AclUsers { get; set; }
        IDbSet<AclRole> AclRoles { get; set; }
        IDbSet<Request> Requests { get; set; }
        IDbSet<RequestType> RequestTypes { get; set; }
        IDbSet<RequestStateType> RequestStateTypes { get; set; }
        IDbSet<RequestUser> Users { get; set; }
        IDbSet<RequestUserAssoc> RequestUserAssocs { get; set; }
        IDbSet<RequestUserRightAssoc> RequestUserRightAssocs { get; set; }
        IDbSet<DelegationRequestUsersExtInfo> DelegationRequestUsersExtInfo { get; set; }
        IDbSet<RequestExtComment> RequestExtComments { get; set; }
        IDbSet<RequestAgreementType> RequestAgreementTypes { get; set; }
        IDbSet<RequestAgreementState> RequestAgreementStates { get; set; }
        IDbSet<RequestAgreement> RequestAgreements { get; set; }
        IDbSet<RequestUserLastSeen> RequestUserLastSeens { get; set; }
        IDbSet<RequestState> RequestStates { get; set; }
        IDbSet<RequestRightGrantType> RequestRightGrantTypes { get; set; }
        IDbSet<ResourceInformationType> ResourceInformationTypes { get; set; }
        IDbSet<ActFile> ActFiles { get; set; }
        int SaveChanges();
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;
    }
}
