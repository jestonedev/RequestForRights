using System;
using RequestsForRightsV2.Infrastructure.Enums;

namespace RequestsForRightsV2.Models.FilterOptions
{
    public class RequestsFilterOptions : FilterOptions
    {
        public RequestCategory RequestCategory { get; set; }
        public int? IdRequestState { get; set; }
        public int? IdRequestType { get; set; }
        public DateTime? DateOfFillingFrom { get; set; }
        public DateTime? DateOfFillingTo { get; set; }
    }
}