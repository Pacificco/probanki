using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Home
{
    public class VM_Feedback
    {
        [Display(Name = "Ваше имя")]
        [Required(ErrorMessage = "Вам необходимо указать Имя!")]
        public string Name { get; set; }
        [Display(Name = "Ваш Email")]
        [DataType(DataType.EmailAddress, ErrorMessage="Не корректный формат Email!")]
        [Required(ErrorMessage = "Вам необходимо указать Email!")]
        public string Email { get; set; }
        [Display(Name = "Сообщение")]
        [Required(ErrorMessage = "Вам необходимо ввести текст сообщения!")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public string Message { get; set; }
        [Display(Name = "Тема")]
        [Required(ErrorMessage = "Вам необходимо указать тему сообщения!")]
        public string Subject { get; set; }
        [Display(Name = "Укажите текст с картинки")]
        [Required(ErrorMessage = "Вам необходимо ввести текст с картинки!")]
        public string CaptchaCode { get; set; }
    }
}