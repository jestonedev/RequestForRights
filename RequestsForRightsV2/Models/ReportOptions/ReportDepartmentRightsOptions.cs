using System;

namespace RequestsForRights.Web.Models.ReportOptions
{
    public class ReportDepartmentRightsOptions : ReportOptions
    {
        public string Department { get; set; }
        public DateTime? Date { get; set; }
    }
}