using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Account
{
    public class VM_User
    {
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя - обязательное поле")]
        public string Name{ get; set; }
        [Required(ErrorMessage = "Email - обязательное поле")]
        [Display(Name = "Email")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string Email{ get; set; }
        [Display(Name = "Пароль")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password{ get; set; }
        [Display(Name = "Пол")]
        [Required(ErrorMessage = "Пол - обязательное поле")]
        public EnumSex Sex{ get; set; }
        [Display(Name = "Фамилия")]
        public string LastName{ get; set; }
        [Display(Name = "Отчество")]        
        public string FatherName{ get; set; }
        [Display(Name = "Активный")]
        public bool IsActive{ get; set; }
        [Display(Name = "Подписан на рассылку")]
        public bool IsSubscribed{ get; set; }
        [Display(Name = "Ник/псевдоним")]
        public string Nic{ get; set; }
        [Display(Name = "Роли")]
        public string[] Rols { get; set; }       
    }
}