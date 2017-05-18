using System;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Web.Models.Models
{
    public class ResourceUserRightHistoryModel
    {
        public int IdRequest { get; set; }
        public int IdRequestType { get; set; }
        public DateTime RequestCompleteDate { get; set; }
        public AclUser AclUser { get; set; }
        public ResourceRight Right { get; set; }
        public string RequestRightDescription { get; set; }
        public RequestRightGrantType GrantType { get; set; }
        public DelegationRequestUsersExtInfo DelegationExtInfo { get; set; }
        public RequestUser RequestUser { get; set; }
    }
}