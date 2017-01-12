﻿using System.Security.Cryptography.X509Certificates;
using RequestsForRightsV2.Infrastructure.Enums;

namespace RequestsForRightsV2.Infrastructure.Security.Interfaces
{
    public interface ISecurityService<in T>
        where T: class
    {
        string CurrentUser { get; }
        bool InRole(AclRole role);
        bool InRole(AclRole[] role);
        bool IsAnonimous();
        bool CanRead(T entity);
        bool CanUpdate(T entity);
        bool CanDelete(T entity);
        bool CanCreate(T entity);
        bool CanModify(T entity);
        bool CanRead();
        bool CanUpdate();
        bool CanDelete();
        bool CanCreate();
        bool CanModify();
    }
}