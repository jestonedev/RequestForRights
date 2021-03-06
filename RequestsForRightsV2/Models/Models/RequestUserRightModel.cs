﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RequestsForRights.Web.Models.Models
{
    public class RequestUserRightModel
    {
        [DisplayName(@"Право")]
        [Required(ErrorMessage = @"Право является обязательным для заполнения")]
        public int IdResourceRight { get; set; }
        [DisplayName(@"Право")]
        public string ResourceRightName { get; set; }
        [DisplayName(@"Описание права")]
        public string ResourceRightDescription { get; set; }
        [DisplayName(@"Действие")]
        [Required(ErrorMessage = @"Обязательно для заполнения")]
        public int IdRequestRightGrantType { get; set; }
        [DisplayName(@"Действие")]
        public string RequestRightGrantTypeName { get; set; }
        [DisplayName(@"Примечание")]
        public string Description { get; set; }
        [DisplayName(@"Ресурс")]
        public int IdResource { get; set; } // Using on detail forms only
        [DisplayName(@"Ресурс")]
        public string ResourceName { get; set; } // Using on detail forms only
        [DisplayName(@"Описание ресурса")]
        public string ResourceDescription { get; set; } // Using on detail forms only

        public override bool Equals(object obj)
        {
            return this == obj as RequestUserRightModel;
        }

        protected bool Equals(RequestUserRightModel other)
        {
            return IdResourceRight == other.IdResourceRight && string.Equals(ResourceRightName, other.ResourceRightName) &&
                   string.Equals(ResourceRightDescription, other.ResourceRightDescription) && IdRequestRightGrantType == other.IdRequestRightGrantType &&
                   string.Equals(RequestRightGrantTypeName, other.RequestRightGrantTypeName) &&
                   string.Equals(Description, other.Description) && IdResource == other.IdResource &&
                   string.Equals(ResourceName, other.ResourceName) &&
                   string.Equals(ResourceDescription, other.ResourceDescription);
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
                hashCode = (hashCode*397) ^ (ResourceRightDescription != null ? ResourceRightDescription.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ IdRequestRightGrantType;
                hashCode = (hashCode*397) ^ (RequestRightGrantTypeName != null ? RequestRightGrantTypeName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ IdResource;
                hashCode = (hashCode*397) ^ (ResourceName != null ? ResourceName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ResourceDescription != null ? ResourceDescription.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}