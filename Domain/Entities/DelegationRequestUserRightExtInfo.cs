using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    [Table("DelegationRequestUserRightsExtInfo")]
    public class DelegationRequestUserRightExtInfo
    {
        [Key]
        public int IdAssoc { get; set; }
        [ForeignKey("IdAssoc")]
        public RequestUserRightAssoc RequestUserRightAssoc { get; set; }
        public int IdDelegateToUser { get; set; }
        [ForeignKey("IdDelegateToUser")]
        public virtual RequestUser RequestUser { get; set; }
        public DateTime DelegateFromDate { get; set; }
        public DateTime DelegateToDate { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
