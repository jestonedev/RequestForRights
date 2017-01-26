using System;

namespace RequestsForRights.Models.Models
{
    public class ResourceUserRightModel
    {
        public int IdResourceRight { get; set; }
        public string ResourceRightName { get; set; }
        public int IdResource { get; set; }
        public string ResourceName { get; set; }
        public int IdRequestUser { get; set; }
        public string RequestUserSnp { get; set; }
        public int? IdDelegateFromUser { get; set; }
        public string DelegateFromUserSnp { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}