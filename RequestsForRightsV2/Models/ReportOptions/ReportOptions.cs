using RequestsForRights.Infrastructure.Enums;

namespace RequestsForRights.Models.ReportOptions
{
    public class ReportOptions
    {
        public string SortField { get; set; }
        public SortDirection SortDirection { get; set; }
        public ReportDisplayStyle ReportDisplayStyle { get; set; } 
    }
}