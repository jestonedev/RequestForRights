using System;

namespace RequestsForRights.Web.Models.ReportOptions
{
    public class ReportDepartmentAndResourceRightsOptions : ReportOptions
    {
        public int? IdResource { get; set; }
        public string Department { get; set; }
        public DateTime? Date { get; set; }
    }
}