using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class ResourceRight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdResourceRight { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int IdResource { get; set; }
        public virtual Resource Resource { get; set; }
        public virtual ICollection<RequestUserRightAssoc> RequestUserRightAssoc { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
