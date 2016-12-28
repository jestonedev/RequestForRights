﻿using System.Data.Entity;
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
        IDbSet<RequestUser> RequestUsers { get; set; }
        IDbSet<RequestUserRightAssoc> RequestUserRightAssocs { get; set; }
        IDbSet<DelegationRequestUserRightExtInfo> DelegationRequestUserRightsExtInfo { get; set; }
        IDbSet<RequestExtDescription> RequestExtDescriptions { get; set; }
        IDbSet<RequestAgreementType> RequestAgreementTypes { get; set; }
        IDbSet<RequestAgreementState> RequestAgreementStates { get; set; }
        IDbSet<RequestAgreement> Request { get; set; }

        int SaveChanges();
    }
}
