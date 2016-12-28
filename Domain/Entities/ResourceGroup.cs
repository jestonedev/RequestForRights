using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class ResourceGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdResourceGroup { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
