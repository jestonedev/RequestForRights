using System;
using RequestsForRights.Web.Infrastructure.Enums;

namespace RequestsForRights.Web.Models.FilterOptions
{
    public class RequestsFilterOptions : FilterOptions
    {
        public RequestCategory RequestCategory { get; set; }
        public int? IdRequestStateType { get; set; }
        public int? IdRequestType { get; set; }
        public DateTime? DateOfFillingFrom { get; set; }
        public DateTime? DateOfFillingTo { get; set; }
    }
}