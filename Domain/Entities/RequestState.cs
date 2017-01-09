using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestsForRights.Domain.Entities
{
    public class RequestState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRequestState { get; set; }

        public int IdRequestStateType { get; set; }
        public virtual RequestStateType RequestStateType { get; set; }

        public int IdRequest { get; set; }
        public virtual Request Request { get; set; }
        public DateTime Date { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
    }
}
