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
        [Required]
        [MaxLength(256)]
        public string Snp { get; set; }
        [Required]
        [MaxLength(256)]
        public string Email { get; set; }
        public int IdDepartment { get; set; }
        [ForeignKey("IdDepartment")]
        public virtual Department Department { get; set; }
        public virtual IList<Department> AclDepartments { get; set; }
        public virtual IList<AclRole> Roles { get; set; }
        public virtual IList<Request> Requests { get; set; }
        public virtual IList<RequestExtDescription> RequestsExtDescriptions { get; set; }
        public virtual IList<RequestAgreement> RequestAgreements { get; set; }
        public virtual IList<RequestUserLastSeen> RequestUserLastSeens { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
