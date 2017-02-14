using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    [Table("DelegationRequestUsersExtInfo")]
    public class DelegationRequestUsersExtInfo
    {
        [Key]
        public int IdRequestUserAssoc { get; set; }
        [ForeignKey("IdRequestUserAssoc")]
        public virtual RequestUserAssoc RequestUserAssoc { get; set; }
        public int IdDelegateToUser { get; set; }
        [ForeignKey("IdDelegateToUser")]
        public virtual RequestUser DelegateToUser { get; set; }
        public DateTime DelegateFromDate { get; set; }
        public DateTime DelegateToDate { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
