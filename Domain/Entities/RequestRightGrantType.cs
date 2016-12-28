using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public virtual ICollection<RequestUserRightAssoc> RequestUserRightAssoc { get; set; }
    }
}
