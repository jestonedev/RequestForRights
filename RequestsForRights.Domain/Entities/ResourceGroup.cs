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
        [Required(ErrorMessage = "Наименование категории ресурсов является обязательным для заполнения")]
        [MaxLength(512)]
        [StringLength(512, ErrorMessage = "Максимальная длина наименования категории ресурсов 512 символов")]
        [DisplayName("Наименование")]
        public string Name { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        public virtual IList<Resource> Resources { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
