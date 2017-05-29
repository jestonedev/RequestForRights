using System.Collections.Generic;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Models.Models;

namespace RequestsForRights.Web.Models.ViewModels
{
    public class ReportResourceOperatorViewModel
    {
        public int? IdDepartment { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<ResourceOperatorModel> ResourceOperators { get; set; }
    }
}