using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.Models;

namespace RequestsForRights.Web.Models.ViewModels
{
    public class ReportAclUserRolesViewModel
    {
        public IEnumerable<AclUserRolesModel> UserRoles { get;set;}
        public int? IdDepartment { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public int? IdRole { get; set; }
        public IEnumerable<AclRole> Roles { get; set; }
    }
}