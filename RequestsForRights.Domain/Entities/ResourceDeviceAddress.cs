using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class ResourceDeviceAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdResourceDeviceAddress { get; set; }
        public int IdResource { get; set; }
        public virtual Resource Resource { get; set; }
        [DisplayName("Наименование")]
        public string Name { get; set; }
        [DisplayName("Индекс")]
        [MaxLength(6)]
        [StringLength(6, ErrorMessage = "Максимальная длина почтового индекса 6 символов")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Некорректно задан почтовый индекс")]
        public string AddressIndex { get; set; }
        [DisplayName("Регион")]
        public string AddressRegion { get; set; }
        [DisplayName("Район")]
        public string AddressArea { get; set; }
        [DisplayName("Город")]
        public string AddressCity { get; set; }
        [DisplayName("Улица")]
        public string AddressStreet { get; set; }
        [DisplayName("Дом")]
        [MaxLength(32)]
        [StringLength(32, ErrorMessage = "Максимальная длина номера дома 32 символов")]
        public string AddressHouse { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
