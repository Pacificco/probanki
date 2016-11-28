using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Account
{
    public class VM_UserLogin
    {
        [Required(ErrorMessage = "Укажите Email!")]
        [Display(Name = "Email")]
        //[DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress, ErrorMessage ="Не корректный Email!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный Email!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Укажите пароль!")]
        [Display(Name = "Пароль")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]        
        public string Password { get; set; }
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }      
    }
}