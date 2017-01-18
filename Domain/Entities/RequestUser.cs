using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace RequestsForRights.Domain.Entities
{
    public class RequestUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScriptIgnore]
        public int IdRequestUser { get; set; }
        [MaxLength(256)]
        public string Login { get; set; }
        [Required]
        [MaxLength(512)]
        public string Snp { get; set; }
        [Required]
        [MaxLength(512)]
        public string Post { get; set; }
        [MaxLength(512)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(512)]
        public string Department { get; set; }
        [MaxLength(512)]
        public string Unit { get; set; }
        [Required]
        [MaxLength(512)]
        public string Office { get; set; }
        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual IList<RequestUserAssoc> RequestUserAssoc { get; set; }
        [DefaultValue(false)]
        [ScriptIgnore]
        public bool Deleted { get; set; }
    }
}
