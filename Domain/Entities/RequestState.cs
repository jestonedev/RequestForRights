using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class RequestState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRequestState { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
