﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class RequestUserLastSeen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRequestUserLastSeen { get; set; }
        public DateTime DateOfLastSeen { get; set; }
        public int IdUser { get; set; }
        public virtual AclUser User { get; set; }
        public int IdRequest { get; set; }
        public virtual Request Request { get; set; }
    }
}
