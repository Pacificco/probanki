using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_OrgPoint
    {
        [HiddenInput]
        public int Id { get; set; }
        [HiddenInput]
        public int OrgId { get; set; }
        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Вы не указали наименование")]
        [MaxLength(1000, ErrorMessage = "Максимальная длина поля [Наименование] 1000 символов!")]
        public string Title { get; set; }
        [Display(Name = "Псевдоним")]
        public string Alias { get; set; }
        [Display(Name = "Тип")]
        [Range(1, 10, ErrorMessage = "Вы не указали тип")]
        public EnumOrgPointType PointType { get; set; }
        [Display(Name = "Тип")]
        public string PointTypeAsString { get; set; }
        [Display(Name = "Регион")]        
        public Guid RegionId { get; set; }
        [Display(Name = "Регион")]
        public string Region { get; set; }
        [Display(Name = "Адрес")]
        [MaxLength(1000, ErrorMessage = "Максимальная длина поля [Адрес] 1000 символов!")]
        public string Address { get; set; }
        [Display(Name = "Телефоны")]
        [MaxLength(1000, ErrorMessage = "Максимальная длина поля [Телефоны] 1000 символов!")]
        public string Phones { get; set; }
        [Display(Name = "Уточнение к адресу")]
        [MaxLength(1000, ErrorMessage = "Максимальная длина поля [Уточнение к адресу] 1000 символов!")]
        public string AddressDopInfo { get; set; }
        [Display(Name = "Режим работы")]
        [MaxLength(1000, ErrorMessage = "Максимальная длина поля [Режим работы] 1000 символов!")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [AllowHtml]
        public string Schedule { get; set; }
        [Display(Name = "Дополнительная информация")]
        [MaxLength(1000, ErrorMessage = "Максимальная длина поля [Дополнительная информация] 1000 символов!")]
        public string DopInfo { get; set; }
        [Display(Name = "Активен")]
        public bool IsActive { get; set; }
    }
}