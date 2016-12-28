using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class AclUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUser { get; set; }
        [Required]
        [MaxLength(256)]
        public string Login { get; set; }
        public int IdDepartment { get; set; }
        [ForeignKey("IdDepartment")]
        public virtual Department Department { get; set; }
        public virtual ICollection<Department> AclDepartments { get; set; }
        public virtual ICollection<AclRole> Roles { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<RequestExtDescription> RequestsExtDescriptions { get; set; }
        public virtual ICollection<RequestAgreement> RequestAgreements { get; set; }
        public virtual ICollection<RequestUserLastSeen> RequestUserLastSeens { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
