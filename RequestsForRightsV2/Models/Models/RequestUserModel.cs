using System.Collections.Generic;

namespace RequestsForRights.Models.Models
{
    public class RequestUserModel
    {
        public string Snp { get; set; }
        public string Login { get; set; }
        public string Post { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public IList<RequestUserRightModel> Rights { get; set; }
    }
}