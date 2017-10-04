using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace RequestsForRights.Domain.Entities
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScriptIgnore]
        public int IdDepartment { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        [ScriptIgnore]
        public int? IdParentDepartment { get; set; }
        [ForeignKey("IdParentDepartment")]
        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual Department ParentDepartment { get; set; }
        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual IList<Department> ChildDepartments { get; set; }
        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual IList<Resource> OwnerResources { get; set; }
        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual IList<Resource> OperatorResources { get; set; }
        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual IList<AclUser> Users { get; set; }
        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual IList<AclUser> AclUsers { get; set; }
        [DefaultValue(false)]
        [ScriptIgnore]
        public bool Deleted { get; set; }
        // Federal registry ext fields
        [DisplayName("Идентификационный номер налогоплательщика")]
        public string TaxPayerNumber { get; set; }
        [DisplayName("Полное наименование на русском языке")]
        public string OfficialNameLongRu { get; set; }
        [DisplayName("Сокращенное наименование на русском языке")]
        public string OfficialNameShortRu { get; set; }
        [DisplayName("Полное наименование на английском языке")]
        public string OfficialNameLongEn { get; set; }
        [DisplayName("Сокращенное наименование на английском языке")]
        public string OfficialNameShortEn { get; set; }
        [DisplayName("Индекс")]
        [MaxLength(6)]
        [StringLength(6, ErrorMessage = "Максимальная длина почтового индекса 6 символов")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Некорректно задан почтовый индекс")]
        public string SelfAddressIndex { get; set; }
        [DisplayName("Регион")]
        public string SelfAddressRegion { get; set; }
        [DisplayName("Район")]
        public string SelfAddressArea { get; set; }
        [DisplayName("Город")]
        public string SelfAddressCity { get; set; }
        [DisplayName("Улица")]
        public string SelfAddressStreet { get; set; }
        [DisplayName("Дом")]
        public string SelfAddressHouse { get; set; }
        [DisplayName("Совпадает с адресом постоянно действующего исполнительного органа")]
        public bool СontrolOrgAddressesAreEqualSelfAddress { get; set; }
        [DisplayName("Индекс")]
        [MaxLength(6)]
        [StringLength(6, ErrorMessage = "Максимальная длина почтового индекса 6 символов")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Некорректно задан почтовый индекс")]
        public string ControlOrgAddressIndex { get; set; }
        [DisplayName("Регион")]
        public string ControlOrgAddressRegion { get; set; }
        [DisplayName("Район")]
        public string ControlOrgAddressArea { get; set; }
        [DisplayName("Город")]
        public string ControlOrgAddressCity { get; set; }
        [DisplayName("Улица")]
        public string ControlOrgAddressStreet { get; set; }
        [DisplayName("Дом")]
        public string ControlOrgAddressHouse { get; set; }
        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual IList<Resource> RequestAllowedResources { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool IsAlienDepartment { get; set; }
    }
}
