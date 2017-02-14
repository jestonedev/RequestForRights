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
        [Required(ErrorMessage = "Наименование права является обязательным для заполнения")]
        [MaxLength(512)]
        [StringLength(512, ErrorMessage = "Максимальная длина наименования права 512 символов")]
        [DisplayName("Наименование")]
        public string Name { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        public int IdResource { get; set; }
        public virtual Resource Resource { get; set; }
        public virtual IList<RequestUserRightAssoc> RequestUserRightAssoc { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
