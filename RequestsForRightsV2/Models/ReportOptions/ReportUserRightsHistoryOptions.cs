using System;
using RequestsForRights.Domain.Enums;
using RequestsForRights.Web.Infrastructure.Enums;

namespace RequestsForRights.Web.Models.ReportOptions
{
    public class ReportUserRightsHistoryOptions : ReportOptions
    {
        public string Login{ get; set; }
        public string Snp { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public UsersCategory UsersCategory { get; set; }
    }
}