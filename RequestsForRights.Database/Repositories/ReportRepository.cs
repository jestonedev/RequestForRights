﻿using System;
using System.Data.Entity;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Database.Repositories
{
    public class ReportRepository: IReportRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public ReportRepository(IDatabaseContext databaseContext)
        {
            if (databaseContext == null)
            {
                throw new ArgumentNullException("databaseContext");
            }
            _databaseContext = databaseContext;
        }

        public IQueryable<Resource> GetResources()
        {
            return _databaseContext.Resources.Where(r => !r.Deleted)
                .Include(r => r.ResourceGroup)
                .Include(r => r.RequestAllowedDepartments);
        }

        public IQueryable<RequestUser> GetUsers()
        {
            return _databaseContext.Users.Where(r => !r.Deleted);
        }


        public IQueryable<Department> GetDepartments()
        {
            return _databaseContext.Departments.Where(r => !r.Deleted && (r.IdParentDepartment == null || r.IdParentDepartment == 1));
        }

        public IQueryable<ResourceOperatorPerson> GetOperatorPersons()
        {
            return _databaseContext.ResourceOperatorPersons.Where(r => !r.Deleted);
        }

        public IQueryable<ResourceOperatorPersonAct> GetOperatorPersonActs()
        {
            return _databaseContext.ResourceOperatorPersonActs.Where(r => !r.Deleted);
        }

        public IQueryable<AclRole> GetAclRoles()
        {
            return _databaseContext.AclRoles;
        }

        public IQueryable<AclUser> GetAclUsers()
        {
            return _databaseContext.AclUsers.Include(u => u.Roles).Include(u => u.Department).Where(u => !u.Deleted);
        }
    }
}
