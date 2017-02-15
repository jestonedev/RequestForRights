using RequestsForRights.Web.Infrastructure.Enums;

namespace RequestsForRights.Web.Models.FilterOptions
{
    public class FilterOptions
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string Filter { get; set; }
        public string SortField { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}