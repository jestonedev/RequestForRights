using System;
using System.Collections.Generic;

namespace RequestsForRights.Web.Models.Models
{
    public class AclUserRolesModel
    {
        public string Snp { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public DateTime DateCreated { get; set; }
    }
}