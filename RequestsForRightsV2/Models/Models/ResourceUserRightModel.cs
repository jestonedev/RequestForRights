using System;

namespace RequestsForRights.Web.Models.Models
{
    public class ResourceUserRightModel
    {
        public int IdResourceRight { get; set; }
        public string ResourceRightName { get; set; }
        public int IdResource { get; set; }
        public string ResourceName { get; set; }
        public string RightCategory { get; set; }
        public int IdRequestUser { get; set; }
        public string RequestUserSnp { get; set; }
        public string RequestUserDepartment { get; set; }
        public string RequestUserUnit { get; set; }
        public int? IdDelegateFromUser { get; set; }
        public string DelegateFromUserSnp { get; set; }
        public string DelegateFromUserDepartment { get; set; }
        public string DelegateFromUserUnit { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateDelegateFrom { get; set; }
        public DateTime? DateDelegateTo { get; set; }
        public string Description { get; set; }
    }
}