using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Account
{
    public class VM_UserPasswordRecover
    {
        [Required(ErrorMessage = "Укажите Email!")]
        [Display(Name = "Укажите Email для восстановления пароля")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Не корректный Email!")]
        public string Email { get; set; }
        [Display(Name = "Введите код с картинки")]
        [Required(ErrorMessage = "Код с картинки указан не верно!")]
        public string CaptchaCode { get; set; }
    }
}