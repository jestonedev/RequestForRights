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
        public int IdRequest { get; set; }
        public virtual Request Request { get; set; }
        public int IdRequestUser { get; set; }
        public virtual RequestUser RequestUser { get; set; }
        public int IdResourceRight { get; set; }
        public virtual ResourceRight ResourceRight { get; set; }
        public int IdRequestRightGrantType { get; set; }
        public virtual RequestRightGrantType RequestRightGrantType { get; set; }
        public string Descirption { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
