using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class RequestUserRightAssoc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAssoc { get; set; }
        public int IdRequestUserAssoc { get; set; }
        public virtual RequestUserAssoc RequestUserAssoc { get; set; }
        public int IdResourceRight { get; set; }
        public virtual ResourceRight ResourceRight { get; set; }
        public int IdRequestRightGrantType { get; set; }
        public virtual RequestRightGrantType RequestRightGrantType { get; set; }
        public string Descirption { get; set; }
        public DateTime? GrantedFrom { get;set; }
        public DateTime? GrantedTo { get;set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
