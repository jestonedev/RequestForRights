using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class RequestAgreementState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAgreementState { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual IList<RequestAgreement> RequestAgreements { get; set; }
    }
}
