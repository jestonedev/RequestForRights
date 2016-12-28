using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRequest { get; set; }
        public string Description { get; set; }
        public DateTime DateOfFilling { get; set; }
        public DateTime? DateOfCompletion { get; set; }
        public int IdUser { get; set; }
        public virtual AclUser User { get; set; }
        public int IdRequestState { get; set; }
        public virtual RequestState RequestState { get; set; }
        public int IdRequestType { get; set; }
        public virtual RequestType RequestType { get; set; }
        public virtual ICollection<RequestUserRightAssoc> RequestUserRightAssoc { get; set; }
        public virtual ICollection<RequestAgreement> RequestAgreements { get; set; }
        public virtual ICollection<RequestUserLastSeen> RequestUserLastSeens { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
