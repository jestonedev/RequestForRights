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
        [Required(ErrorMessage = "Обязательно для заполнения")]
        public int IdRequestRightGrantType { get; set; }
        [DisplayName("Действие")]
        public string RequestRightGrantTypeName { get; set; }
        [DisplayName("Примечание")]
        public string Description { get; set; }
        [DisplayName("Ресурс")]
        public int IdResource { get; set; } // Using on detail forms only
        [DisplayName("Ресурс")]
        public string ResourceName { get; set; } // Using on detail forms only

        public override bool Equals(object obj)
        {
            return this == obj as RequestUserRightModel;
        }

        protected bool Equals(RequestUserRightModel other)
        {
            return IdResourceRight == other.IdResourceRight && string.Equals(ResourceRightName, other.ResourceRightName) &&
                   IdRequestRightGrantType == other.IdRequestRightGrantType &&
                   string.Equals(RequestRightGrantTypeName, other.RequestRightGrantTypeName) &&
                   string.Equals(Description, other.Description) && IdResource == other.IdResource &&
                   string.Equals(ResourceName, other.ResourceName);
        }

        public static bool operator ==(RequestUserRightModel first, RequestUserRightModel second)
        {
            if ((object)first == null && (object)second == null)
                return true;
            if ((object)first == null || (object)second == null)
                return false;
            return first.Equals(second);
        }

        public static bool operator !=(RequestUserRightModel first, RequestUserRightModel second)
        {
            return !(first == second);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IdResourceRight;
                hashCode = (hashCode*397) ^ (ResourceRightName != null ? ResourceRightName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ IdRequestRightGrantType;
                hashCode = (hashCode*397) ^ (RequestRightGrantTypeName != null ? RequestRightGrantTypeName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ IdResource;
                hashCode = (hashCode*397) ^ (ResourceName != null ? ResourceName.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}