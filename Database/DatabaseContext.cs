using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public IDbSet<ResourceRight> ResourceRights { get; set; }
        public IDbSet<Resource> Resources { get; set; }
        public IDbSet<ResourceGroup> ResourceGroups { get; set; }
        public IDbSet<Department> Departments { get; set; }
        public IDbSet<AclUser> AclUsers { get; set; }
        public IDbSet<AclRole> AclRoles { get; set; }
        public IDbSet<Request> Requests { get; set; }
        public IDbSet<RequestType> RequestTypes { get; set; }
        public IDbSet<RequestStateType> RequestStateTypes { get; set; }
        public IDbSet<RequestUser> Users { get; set; }
        public IDbSet<RequestUserAssoc> RequestUserAssocs { get; set; }
        public IDbSet<RequestUserRightAssoc> RequestUserRightAssocs { get; set; }
        public IDbSet<DelegationRequestUserRightExtInfo> DelegationRequestUserRightsExtInfo { get; set; }
        public IDbSet<RequestExtDescription> RequestExtDescriptions { get; set; }
        public IDbSet<RequestAgreementType> RequestAgreementTypes { get; set; }
        public IDbSet<RequestAgreementState> RequestAgreementStates { get; set; }
        public IDbSet<RequestAgreement> RequestAgreements { get; set; }
        public IDbSet<RequestUserLastSeen> RequestUserLastSeens { get; set; }
        public IDbSet<RequestState> RequestStates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AclUser>()
                .HasMany(f => f.Roles)
                .WithMany(f => f.Users)
                .Map(cs =>
                {
                    cs.MapRightKey("IdRole");
                    cs.MapLeftKey("IdUser");
                });
            modelBuilder.Entity<AclUser>()
                .HasMany(f => f.AclDepartments)
                .WithMany(f => f.AclUsers)
                .Map(cs =>
                {
                    cs.MapRightKey("IdDepartment");
                    cs.MapLeftKey("IdUser");
                    cs.ToTable("AclDepartments");
                });
            modelBuilder.Entity<Department>()
                .HasMany(f => f.Users)
                .WithRequired(f => f.Department)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<AclUser>()
                .HasMany(f => f.Requests)
                .WithRequired(f => f.User)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<AclUser>()
                .HasMany(f => f.RequestsExtDescriptions)
                .WithRequired(f => f.User)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<AclUser>()
                .HasMany(f => f.RequestAgreements)
                .WithRequired(f => f.User)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<AclUser>()
                .HasMany(f => f.RequestUserLastSeens)
                .WithRequired(f => f.User)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<ResourceGroup>()
                .HasMany(f => f.Resources)
                .WithRequired(f => f.ResourceGroup)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<ResourceRight>()
                .HasMany(f => f.RequestUserRightAssoc)
                .WithRequired(f => f.ResourceRight)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Resource>()
                .HasRequired(f => f.Department)
                .WithMany(f => f.Resources)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<RequestStateType>()
                .HasMany(f => f.RequestStates)
                .WithRequired(f => f.RequestStateType)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<RequestType>()
                .HasMany(f => f.Requests)
                .WithRequired(f => f.RequestType)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<RequestRightGrantType>()
                .HasMany(f => f.RequestUserRightAssoc)
                .WithRequired(f => f.RequestRightGrantType)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<RequestAgreementState>()
                .HasMany(f => f.RequestAgreements)
                .WithRequired(f => f.AgreementState)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<RequestAgreementType>()
                .HasMany(f => f.RequestAgreements)
                .WithRequired(f => f.AgreementType)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<RequestUserLastSeen>()
                .Property(f => f.IdRequest)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IDX_RequestUserLastSeen_IdRequest_IdUser", 1)));
            modelBuilder.Entity<RequestUserLastSeen>()
                .Property(f => f.IdUser)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IDX_RequestUserLastSeen_IdRequest_IdUser", 2)));
        }



        public new DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity: class
        {
            return base.Entry(entity);
        }

        public new int SaveChanges()
        {
            // TODO: There will be change loging
            return base.SaveChanges();
        }
    }
}
