﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RequestsForRights.Web.Models.Models
{
    public class RequestUserModel
    {
        [DisplayName(@"ФИО")]
        [Required(ErrorMessage = @"ФИО является обязательным для заполнения")]
        public string Snp { get; set; }
        [DisplayName(@"Логин")]
        public string Login { get; set; }
        [DisplayName(@"Должность")]
        public string Post { get; set; }
        [DisplayName(@"Организация")]
        [Required(ErrorMessage = @"Организация является обязательной для заполнения")]
        public string Department { get; set; }
        [DisplayName(@"Отдел")]
        public string Unit { get; set; }
        [DisplayName(@"Кабинет")]
        public string Office { get; set; }
        [DisplayName(@"Телефон")]
        public string Phone { get; set; }
        [DisplayName(@"Примечание")]
        public string Description { get; set; }
        public IList<RequestUserRightModel> Rights { get; set; }
    }
}