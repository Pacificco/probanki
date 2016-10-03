using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.News
{
    public class VM_News
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
        [Display(Name = "Url сайта-источника")]
        public string SourceUrl { get; set; }
        [Display(Name = "Текст новости")]
        [Required(ErrorMessage = "Вы не указали текст новости")]
        [AllowHtml]
        public string NewsText { get; set; }
        [Display(Name = "Url новости на сайте-источнике")]
        public string NewsUrl { get; set; }
        [Display(Name = "Url картинки к новости на сайте-источнике")]
        public string PictureUrl { get; set; }
        [Display(Name = "Дата публикации")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime PublishedAt { get; set; }
        [Display(Name = "Дата создания")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Активная")]
        public bool IsActive { get; set; } 
        [Display(Name = "Мета-заголовок")]
        [MaxLength(255, ErrorMessage = "Максимальная длина Meta-заголовка не должна превышать 255 символов")]
        public string MetaTitle { get; set; }        
        [Display(Name = "Мета-описание")]
        [MaxLength(255, ErrorMessage = "Максимальная длина Meta-описания не должна превышать 255 символов")]
        public string MetaDesc { get; set; }
        [Display(Name = "Мета-ключевые слова")]
        [MaxLength(255, ErrorMessage = "Максимальная длина Meta-ключевых слов не должна превышать 255 символов")]
        public string MetaKeys { get; set; }
        [Display(Name = "NoIndex")]
        public bool MetaNoIndex { get; set; }
        [Display(Name = "NoFollow")]
        public bool MetaNoFollow { get; set; }        
    }
}