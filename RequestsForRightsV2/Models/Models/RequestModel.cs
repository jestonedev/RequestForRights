using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RequestsForRights.Web.Models.Models
{
    public class RequestModel<T>
        where T: RequestUserModel
    {
        public int IdRequest { get; set; }
        [DisplayName(@"Описание")]
        public string Description { get; set; }
        [DisplayName(@"Дата подачи")]
        public DateTime RequestDate { get; set; }
        [DisplayName(@"Дата выполнения")]
        public DateTime? CompleteDate { get; set; }
        [DisplayName(@"Заявитель")]
        public string OwnerSnp { get; set; }
        [DisplayName(@"Департамент")]
        public string OwnerDepartment { get; set; }
        public string RequestStateName { get; set; }
        public int IdRequestType { get; set; }
        public IList<T> Users { get; set; }
    }
}