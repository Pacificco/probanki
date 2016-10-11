using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Account
{
    public class VM_Login
    {
        [Required(ErrorMessage = "Email - обязательное поле")]
        [Display(Name = "Укажите Email")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Пароль - обязательное поле")]
        [Display(Name = "Введите пароль")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }      
    }
}