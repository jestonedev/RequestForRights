using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class RequestRightGrantType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRequestRightGrantType { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        public virtual IList<RequestUserRightAssoc> RequestUserRightAssoc { get; set; }
    }
}
