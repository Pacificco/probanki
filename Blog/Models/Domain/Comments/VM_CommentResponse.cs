using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Comments
{
    public class VM_CommentResponse
    {
        [Display(Name = "Публикация")]
        public int ArtId { get; set; }
        [Display(Name = "Ваше имя")]
        [Required(ErrorMessage = "Вы не указали свое имя")]
        public string UserName { get; set; }
        [Display(Name = "Текст комментария")]
        [Required(ErrorMessage = "Вы не ввели текст комментария")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [AllowHtml]
        public string CommentText { get; set; }
        //[Display(Name = "Ваша оценка")]
        //[Required(ErrorMessage = "Вы не указали свою оценку")]
        //public int LikeDislike { get; set; }
        [Display(Name = "Введите код с картинки")]
        [Required(ErrorMessage = "Код с картинки указан не верно!")]
        //[MaxLength(5, ErrorMessage = "Код с картинки указан не верно!")]
        public string CaptchaCode { get; set; }
        public string SuccessMessage { get; set; }
        
        public bool IsEmpty()
        {
            if (ArtId > 0)
                return false;
            if (!String.IsNullOrEmpty(UserName))
                return false;
            if (!String.IsNullOrEmpty(CommentText))
                return false;
            if (!String.IsNullOrEmpty(CaptchaCode))
                return false;
            if (!String.IsNullOrEmpty(SuccessMessage))
                return false;
            return true;
        }
    }
}