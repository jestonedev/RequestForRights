using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class RequestExtDescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDescription { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime DateOfWriting { get; set; }
        public int IdRequest { get; set; }
        public virtual Request Request { get; set; }
        public int IdUser { get; set; }
        public virtual AclUser User { get; set; }
    }
}
