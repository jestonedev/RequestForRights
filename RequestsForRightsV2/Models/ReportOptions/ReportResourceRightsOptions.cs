using System;

namespace RequestsForRights.Web.Models.ReportOptions
{
    public class ReportResourceRightsOptions : ReportOptions
    {
        public int? IdResource { get; set; }
        public DateTime? Date { get; set; }
    }
}