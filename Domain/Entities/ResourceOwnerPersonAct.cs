using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class ResourceOwnerPersonAct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdResourceOwnerPersonAct { get; set; }
        public int IdResourceOwnerPerson { get; set; }
        public virtual ResourceOwnerPerson ResourceOwnerPerson { get; set; }
        [DisplayName("Вид")]
        public string ActType { get; set; }
        [DisplayName("Наименование")]
        public string ActName { get; set; }
        [DisplayName("Дата")]
        public DateTime? ActDate { get; set; }
        [DisplayName("Номер")]
        public string ActNumber { get; set; }
        public int IdFile { get; set; }
        public virtual ActFile File { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
