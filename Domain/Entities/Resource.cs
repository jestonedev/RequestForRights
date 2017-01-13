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
        [Required(ErrorMessage = "Наименование ресурса является обязательным для заполнения")]
        [MaxLength(512)]
        [StringLength(512,ErrorMessage = "Максимальная длина наименования ресурса 512 символов")]
        [DisplayName("Наименование")]
        public string Name { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Категория ресурсов является обязательной для заполнения")]
        [DisplayName("Категория")]
        public int IdResourceGroup { get; set; }
        public virtual ResourceGroup ResourceGroup { get; set; }
        public virtual IList<ResourceRight> ResourceRights { get; set; }
        [Required(ErrorMessage = "Департамент-владелец является обязательным для заполнения")]
        [DisplayName("Владелец")]
        public int IdDepartment { get; set; }
        public virtual Department Department { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
