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
        [DisplayName("Организация-владелец")]
        [Required(ErrorMessage = "Организация-владелец является обязательной для заполнения")]
        public int IdOwnerDepartment { get; set; }
        [ForeignKey("IdOwnerDepartment")]
        public virtual Department OwnerDepartment { get; set; }
        [Required(ErrorMessage = "Категория ресурсов является обязательной для заполнения")]
        [DisplayName("Категория")]
        public int IdResourceGroup { get; set; }
        public virtual ResourceGroup ResourceGroup { get; set; }
        public virtual IList<ResourceRight> ResourceRights { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
        // Federal registry ext fields

        [DisplayName("Организация-оператор")]
        [Required(ErrorMessage = "Организация-оператор является обязательной для заполнения")]
        [DefaultValue(24)]
        public int IdOperatorDepartment { get; set; }
        [ForeignKey("IdOperatorDepartment")]
        public virtual Department OperatorDepartment { get; set; }
        public virtual IList<ResourceOwnerPerson> ResourceOwnerPersons { get; set; }
        public virtual IList<ResourceOperatorPerson> ResourceOperatorPersons { get; set; }
        [DisplayName("Email администратора")]
        [RegularExpression(@"^(([^<>()\[\]\\.,;:\s@""]+(\.[^<>()\[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$",
            ErrorMessage = "Некорректно задан почтовый адрес")]
        public string EmailAdministrator { get; set; }
        [DisplayName("ИНН субъекта контроля")]
        public string InnControlSubject { get; set; }
        [DisplayName("Сведения о видах информации, подлежащей размещению в информационной системе с указанием категории информации")]
        public int? IdResourceInformationType { get; set; }
        public virtual ResourceInformationType ResourceInformationType { get; set; }
        [DisplayName("Сведения о персональных данных, обрабатываемых информационной системой")]
        public string PersonalInfoDescription { get; set; }
        public virtual IList<ResourceOperatorAct> ResourceOperatorActs { get; set; }
        public virtual IList<ResourceUsingAct> ResourceUsingActs { get; set; }
        public virtual IList<ResourceAuthorityAct> ResourceAuthorityActs { get; set; }
        [DisplayName("Информационная система не имеет доступа к сети Интернет")]
        [DefaultValue(false)]
        public bool HasNotInternetAccess { get; set; }
        public virtual IList<ResourceInternetAddress> ResourceInternetAddresses { get; set; }
        public virtual IList<ResourceDeviceAddress> ResourceDeviceAddresses { get; set; }
        public virtual IList<Department> RequestAllowedDepartments { get; set; }
    }
}
