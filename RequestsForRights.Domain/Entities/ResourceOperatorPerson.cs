using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class ResourceOperatorPerson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdResourceOperatorPerson { get; set; }
        [DisplayName("Должность")]
        public string Post { get; set; }
        [DisplayName("Фамилия")]
        public string Surname { get; set; }
        [DisplayName("Имя")]
        public string Name { get; set; }
        [DisplayName("Отчество")]
        public string Patronimic { get; set; }
        public int IdResource { get; set; }
        public virtual Resource Resource { get; set; }
        public virtual IList<ResourceOperatorPersonAct> Acts { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
