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
        public int IdUser { get; set; }
        public virtual AclUser User { get; set; }
        public int IdRequestType { get; set; }
        public virtual RequestType RequestType { get; set; }
        public virtual IList<RequestAgreement> RequestAgreements { get; set; }
        public virtual IList<RequestUserLastSeen> RequestUserLastSeens { get; set; }
        public virtual IList<RequestState> RequestStates { get; set; }
        public virtual IList<RequestUserAssoc> RequestUserAssoc { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
