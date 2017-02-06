using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class ResourceInternetAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdResourceInternetAddress { get; set; }
        public int IdResource { get; set; }
        public virtual Resource Resource { get; set; }
        [DisplayName("Наименование ТС")]
        public string NetName { get; set; }
        [DisplayName("Идентификатор оборудования")]
        public string DeviceNumber { get; set; }
        [DisplayName("IP-адрес оборудования")]
        [MaxLength(15)]
        [RegularExpression(@"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b",
            ErrorMessage = "Некорректно задан IP-адрес")]
        public string DeviceIpAddress { get; set; }
        [DisplayName("IP-адрес шлюза")]
        [MaxLength(15)]
        [RegularExpression(@"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b",
            ErrorMessage = "Некорректно задан IP-адрес")]
        public string GateIpAddress { get; set; }
        [DisplayName("IP-адрес DHCP")]
        [MaxLength(15)]
        [RegularExpression(@"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b",
            ErrorMessage = "Некорректно задан IP-адрес")]
        public string DhcpIpAddress { get; set; }
        [DisplayName("Динамический IP-адрес")]
        [DefaultValue(false)]
        public bool IsDynamicIpAddress { get; set; }
        [DisplayName("Доменные имена")]
        public string DomainNames { get; set; }
        [DisplayName("IP-адрес домена")]
        [MaxLength(15)]
        [RegularExpression(@"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b",
            ErrorMessage = "Некорректно задан IP-адрес")]
        public string DomainIpAddress { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
