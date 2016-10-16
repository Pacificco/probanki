using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Account
{
    public class VM_UserChangePassword
    {
        public int Id { get; set; }
        public string Token { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Задайте пароль!")]
        [MaxLength(16, ErrorMessage = "Максимальная длина пароля 16 символов!")]
        [MinLength(6, ErrorMessage = "Минимальная длина пароля 6 символов!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        [DataType(DataType.Password)]
        public string PasswordConfirmed { get; set; }

        [Display(Name = "Введите код с картинки")]
        [Required(ErrorMessage = "Код с картинки указан не верно!")]
        public string CaptchaCode { get; set; }
    }
}