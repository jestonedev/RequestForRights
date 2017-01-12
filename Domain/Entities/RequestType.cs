using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class RequestType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRequestType { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        public virtual IList<Request> Requests { get; set; }
    }
}
