using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class Resource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdResource { get; set; }
        [Required(ErrorMessage = "Наименование ресурса является обязательным для заполнения")]
        [MaxLength(512)]
        [StringLength(512,ErrorMessage = "Максимальная длина наименования ресурса 512 символов")]
        [DisplayName("Наименование")]
        public string Name { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        [DisplayName("Департамент-владелец")]
        [Required(ErrorMessage = "Департамент-владелец является обязательным для заполнения")]
        public int IdDepartment { get; set; }
        [ForeignKey("IdDepartment")]
        public virtual Department Department { get; set; }
        [Required(ErrorMessage = "Категория ресурсов является обязательной для заполнения")]
        [DisplayName("Категория")]
        public int IdResourceGroup { get; set; }
        public virtual ResourceGroup ResourceGroup { get; set; }
        public virtual IList<ResourceRight> ResourceRights { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
        // Federal registry ext fields

        [DisplayName("Департамент-оператор")]
        [Required(ErrorMessage = "Департамент-оператор является обязательным для заполнения")]
        [DefaultValue(24)]
        public int? IdOperatorDepartment { get; set; }
        [ForeignKey("IdOperatorDepartment")]
        public virtual Department OperatorDepartment { get; set; }
        public IList<ResourceOwnerPerson> ResourceOwnerPersons { get; set; }
        public IList<ResourceOperatorPerson> ResourceOperatorPersons { get; set; }
        [DisplayName("Email администратора")]
        public string EmailAdministrator { get; set; }
        [DisplayName("ИНН субъекта контроля")]
        public string InnControlSubject { get; set; }
        [DisplayName("Сведения о видах информации, подлежащей размещению в информационной системе с указанием категории информации")]
        public int? IdResourceInformationType { get; set; }
        public virtual ResourceInformationType ResourceInformationType { get; set; }
        [DisplayName("Сведения о персональных данных, обрабатываемых информационной системой")]
        public string PersonalInfoDescription { get; set; }
        public IList<ResourceOperatorAct> ResourceOperatorActs { get; set; }
        public IList<ResourceUsingAct> ResourceUsingActs { get; set; }
        public IList<ResourceAuthorityAct> ResourceAuthorityActs { get; set; }
        [DisplayName("Информационная система не имеет доступа к сети Интернет")]
        [DefaultValue(false)]
        public bool HasNotInternetAccess { get; set; }
        public IList<ResourceInternetAddress> ResourceInternetAddresses { get; set; }
        public IList<ResourceDeviceAddress> ResourceDeviceAddresses { get; set; }
    }
}
