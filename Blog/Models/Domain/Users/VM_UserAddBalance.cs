using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Users
{
    public class VM_UserAddBalance
    {
        [HiddenInput]
        public int UserId { get; set; }
        [Display(Name = "Тариф")]
        [Range(1,4,ErrorMessage = "Вы не указали тариф!")]
        public int TariffId { get; set; }
        [Display(Name = "Сумма (в руб.)")]
        [Required(ErrorMessage = "Вы не указали сумму!")]
        public double Sum { get; set; }
        [Display(Name = "Период")]
        [Range(1, 10, ErrorMessage = "Вы не указали период!")]
        public string Period { get; set; }
        [Display(Name = "Комментарий")]        
        [MaxLength(255, ErrorMessage = "Максимальная длина Комментария не должна превышать 255 символов!")]
        public string Comment { get; set; }        
    }
}