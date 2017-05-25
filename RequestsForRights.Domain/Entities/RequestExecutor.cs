using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class RequestExecutor
    {
        [Key]
        [Column(Order = 0)]
        [Required]
        public int IdRequest { get; set; }
        public virtual Request Request { get; set; }
        [Key]
        [Column(Order = 1)]
        [Required]
        [MaxLength(256)]
        public string Login { get; set; }
        [Key]
        [Column(Order = 2)]
        [Required]
        public int AlexApplicRequestNum { get; set; }
    }
}
