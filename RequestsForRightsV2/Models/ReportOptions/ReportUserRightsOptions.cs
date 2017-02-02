using System;

namespace RequestsForRights.Models.ReportOptions
{
    public class ReportUserRightsOptions : ReportOptions
    {
        public string Login{ get; set; }
        public string Snp { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        public DateTime? Date { get; set; }
    }
}