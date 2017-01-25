using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RequestsForRights.Models.Models
{
    public class RequestUserRightModel
    {
        [DisplayName("Право")]
        [Required(ErrorMessage = "Право является обязательным для заполнения")]
        public int IdResourceRight { get; set; }
        [DisplayName("Право")]
        public string ResourceRightName { get; set; }
        [DisplayName("Действие")]
        public int IdRequestRightGrantType { get; set; }
        public string RequestRightGrantTypeName { get; set; }
        [DisplayName("Примечание")]
        public string Description { get; set; }
        [DisplayName("Ресурс")]
        public int IdResource { get; set; } // Using on detail forms only
        [DisplayName("Ресурс")]
        public string ResourceName { get; set; } // Using on detail forms only
    }
}