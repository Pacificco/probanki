using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_Review
    {
        [HiddenInput]
        public int Id { get; set; }
        //[Display(Name = "Идентификатор организации")]
        [HiddenInput]
        public int OrgId { get; set; }               
        [Display(Name = "Пользователь")]
        public int UserId { get; set; }
        [HiddenInput]
        [Display(Name = "Автор")]
        public string UserName { get; set; }
        [Display(Name = "Заголовок отзыва")]
        public string ReviewTitle { get; set; }
        [Display(Name = "Текст отзыва")]
        public string ReviewText { get; set; }
        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Оценка")]
        public int Rating { get; set; }
        [Display(Name = "Активный")]
        public bool IsActive { get; set; }
        [Display(Name = "Состояние")]
        public EnumConfirmStatus Confirmed { set; get; }
        
        public VM_OrgItem Org { get; set; }
    }
}