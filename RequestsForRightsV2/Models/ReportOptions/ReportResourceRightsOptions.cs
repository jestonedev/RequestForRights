using System;

namespace RequestsForRights.Models.ReportOptions
{
    public class ReportResourceRightsOptions : ReportOptions
    {
        public int? IdResource { get; set; }
        public DateTime? Date { get; set; }
    }
}