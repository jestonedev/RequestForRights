using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class RequestUserAssoc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRequestUserAssoc { get; set; }
        public int IdRequest { get; set; }
        public virtual Request Request { get; set; }
        public int IdRequestUser { get; set; }
        public virtual RequestUser RequestUser { get; set; }
        public bool Deleted { get; set; }
        public virtual IList<RequestUserRightAssoc> RequestUserRightAssocs { get; set; }
    }
}
