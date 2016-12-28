using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class AclRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRole { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        public virtual ICollection<AclUser> Users { get; set; }
    }
}
