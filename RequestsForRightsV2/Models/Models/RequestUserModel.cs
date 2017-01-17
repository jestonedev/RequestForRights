﻿using System.Collections.Generic;
using System.ComponentModel;

namespace RequestsForRights.Models.Models
{
    public class RequestUserModel
    {
        [DisplayName("ФИО")]
        public string Snp { get; set; }
        [DisplayName("Логин")]
        public string Login { get; set; }
        [DisplayName("Должность")]
        public string Post { get; set; }
        [DisplayName("Департамент")]
        public string Department { get; set; }
        [DisplayName("Отдел")]
        public string Unit { get; set; }
        [DisplayName("Кабинет")]
        public string Office { get; set; }
        [DisplayName("Телефон")]
        public string Phone { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        public IList<RequestUserRightModel> Rights { get; set; }
    }
}