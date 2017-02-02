﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RequestsForRights.Models.Models
{
    public class DelegationRequestUsersExtInfoModel
    {
        [DisplayName("ФИО")]
        [Required(ErrorMessage = "ФИО является обязательным для заполнения")]
        public string SnpDelegateTo { get; set; }
        [DisplayName("Логин")]
        public string LoginDelegateTo { get; set; }
        [DisplayName("Должность")]
        [Required(ErrorMessage = "Должность является обязательной для заполнения")]
        public string PostDelegateTo { get; set; }
        [DisplayName("Департамент")]
        [Required(ErrorMessage = "Департамент является обязательным для заполнения")]
        public string DepartmentDelegateTo { get; set; }
        [DisplayName("Отдел")]
        public string UnitDelegateTo { get; set; }
        [DisplayName("Кабинет")]
        public string OfficeDelegateTo { get; set; }
        [DisplayName("Телефон")]
        public string PhoneDelegateTo { get; set; }
        [DisplayName("Дата начала")]
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        public DateTime DelegateFromDate { get; set; }
        [DisplayName("Дата окончания")]
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        public DateTime DelegateToDate { get; set; }
    }
}