﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class RequestAgreement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRequestAgreement { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int IdUser { get; set; }
        public AclUser User { get; set; }
        public int IdRequest { get; set; }
        public virtual Request Request { get; set; }
        public int IdAgreementState { get; set; }
        public virtual RequestAgreementState AgreementState { get; set; }
        public int IdAgreementType { get; set; }
        public virtual RequestAgreementType AgreementType { get; set; }
    }
}
