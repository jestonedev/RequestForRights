using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class Resource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdResource { get; set; }      
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }    
        public string Description { get; set; }
        public int IdResourceGroup { get; set; }
        public virtual ResourceGroup ResourceGroup { get; set; }
        public virtual ICollection<ResourceRight> ResourceRights { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
