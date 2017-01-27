using System;
using RequestsForRights.Infrastructure.Enums;

namespace RequestsForRights.Models.ReportOptions
{
    public class ReportResourceRightsOptions : ReportOptions
    {
        public int? IdResource { get; set; }
        public DateTime? Date { get; set; }
    }
}