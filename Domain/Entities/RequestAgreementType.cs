using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class RequestAgreementType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAgreementType { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<RequestAgreement> RequestAgreements { get; set; }
    }
}
