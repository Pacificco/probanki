using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Categories
{
    public class VM_Category
    {
        [HiddenInput]
        public int Id { set; get; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Вы не указали название категории")]
        public string Title { set; get; }
        [Display(Name = "Псевдоним")]
        public string Alias { set; get; }
        [Display(Name = "Описание")]        
        [Required(ErrorMessage = "Вы не указали описание категории")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [AllowHtml]
        public string Description { set; get; }
        [Display(Name = "Активна")]
        public bool IsActive { set; get; }

        [Display(Name = "Статей")]
        public int ArticlesCount { set; get; }
    }
}