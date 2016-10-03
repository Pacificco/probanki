using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_ReviewResponse
    {                       
        [Display(Name = "Организация")]
        [HiddenInput]
        public int OrgId { get; set; }        
        [HiddenInput]
        public string OrgAlias { get; set; }        
        [HiddenInput]
        public int Page { get; set; }
        [Display(Name = "Ваше имя")]
        [Required(ErrorMessage = "Вы не указали свое имя!")]
        public string UserName { get; set; }        
        [Display(Name = "Текст отзыва")]
        [Required(ErrorMessage = "Вы не ввели текст отзыва!")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public string ReviewText { get; set; }
        [Display(Name = "Ваша оценка")]
        [Required(ErrorMessage = "Необходимо указать оценку")]
        public int Rating { get; set; }
        [Display(Name = "Введите код с картинки")]
        [Required(ErrorMessage = "Код с картинки указан не верно!")]
        //[MaxLength(5, ErrorMessage = "Код с картинки указан не верно!")]
        public string CaptchaCode { get; set; }        
        public string SuccessMessage { get; set; }
    }
}