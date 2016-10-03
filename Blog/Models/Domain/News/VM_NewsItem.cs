using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.News
{
    public class VM_NewsItem
    {
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Заголовок")]
        [Required(ErrorMessage = "Вы не указали заголовок новости")]
        public string Title { get; set; }
        [Display(Name = "Псевдоним")]
        public string Alias { get; set; }
        [Display(Name = "Автор")]
        public string Author { get; set; }
        [Display(Name = "Url")]
        public string SourceUrl { get; set; }
        [Display(Name = "Дата публикации")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime PublishedAt { get; set; }
        [Display(Name = "Дата создания")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime CreatedAt { get; set; }        
        [Display(Name = "Активная")]
        public bool IsActive { get; set; }        
    }
}