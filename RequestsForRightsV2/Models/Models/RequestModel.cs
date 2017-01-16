using System.Collections.Generic;

namespace RequestsForRights.Models.Models
{
    public class RequestModel<T>
        where T: RequestUserModel
    {
        public int IdRequest { get; set; }
        public string Description { get; set; }
        public int IdRequestType { get; set; }
        public string RequestTypeName { get; set; }
        public IList<T> Users { get; set; }
    }
}