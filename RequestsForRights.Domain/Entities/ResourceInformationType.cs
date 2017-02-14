using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class ResourceInformationType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdResourceInformationType { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        public virtual IList<Resource> Resources { get; set; }
    }
}
