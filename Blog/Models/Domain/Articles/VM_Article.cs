using Bankiru.Models.Domain.Categories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Articles
{
    public class VM_Article
    {
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Заголовок")]
        [Required(ErrorMessage = "Вы не указали заголовок статьи")]
        [MaxLength(500, ErrorMessage = "Максимальная длина заголовка не должна превышать 500 символов")]
        public string Title { get; set; }
        [Display(Name = "Псевдоним")]
        public string Alias { get; set; }
        [MaxLength(255, ErrorMessage = "Максимальная длина подзаголовка не должна превышать 255 символов")]
        [Display(Name = "Подзаголовок")]
        public string SubTitle { get; set; }
        [Display(Name = "Предпросмотр")]
        [Required(ErrorMessage = "Вы не указали краткое описание статьи")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string TextPrev { get; set; }
        [Display(Name = "Полный текст статьи")]
        [Required(ErrorMessage = "Вы не указали полное описание статьи")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string TextFull { get; set; }        
        [Display(Name = "Категория")]
        [Range(1, 10)]
        public int CategoryId { get; set; }
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
        [Display(Name = "Дата публикации")]
        [DataType(DataType.Date)]        
        public DateTime PublishedAt { get; set; }        
        [Display(Name = "Дата изменения")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; }
        [Display(Name = "Дата создания")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Автор")]
        public int UserId { get; set; }        
        [Display(Name = "Псевдоним")]
        public string OtherUser { get; set; }
        [Display(Name = "Активная")]
        public bool IsActive { get; set; }
        [Display(Name = "Центральная статья")]
        public bool IsCentral { get; set; }
        [Display(Name = "Просмотров")]
        public int Hits { get; set; }
        
        public VM_Category Category { get; set; }
        public VM_ArtCommentInfo CommentInfo { get; set; }
    }
}