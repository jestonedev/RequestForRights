﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDepartment { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        public int? IdParentDepartment { get; set; }
        [ForeignKey("IdParentDepartment")]
        public virtual Department ParentDepartment { get; set; }
        public virtual ICollection<Department> ChildDepartments { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<AclUser> Users { get; set; }
        public virtual ICollection<AclUser> AclUsers { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
