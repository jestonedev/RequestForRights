﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class RequestStateType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRequestStateType { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        public virtual IList<RequestState> RequestStates { get; set; }
        public virtual IList<Request> Requests { get; set; }
    }
}
