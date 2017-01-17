using System;

namespace RequestsForRights.Models.Models
{
    public class DelegationRequestUsersExtInfoModel
    {
        public string SnpDelegateTo { get; set; }
        public string LoginDelegateTo { get; set; }
        public string PostDelegateTo { get; set; }
        public string DepartmentDelegateTo { get; set; }
        public string UnitDelegateTo { get; set; }
        public string OfficeDelegateTo { get; set; }
        public string PhoneDelegateTo { get; set; }
        public DateTime DelegateFromDate { get; set; }
        public DateTime DelegateToDate { get; set; }
    }
}