using Bankiru.Models.Domain.Categories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Articles
{
    public class VM_ArtItem
    {
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Заголовок")]        
        public string Title { get; set; }
        [Display(Name = "Подзаголовок")]        
        public string SubTitle { get; set; }
        [Display(Name = "Текст предварительного просмотра")]
        public string TextPrev { get; set; }
        [Display(Name = "Псевдоним")]
        public string Alias { get; set; }        
        [Display(Name = "Id категории")]        
        public int CategoryId { get; set; }        
        [Display(Name = "Категория")]
        public VM_Category Category { get; set; }
        [Display(Name = "Дата публикации")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime PublishedAt { get; set; }        
        [Display(Name = "Дата создания")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
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

        [Display(Name = "Комментариев")]
        public VM_ArtCommentInfo CommentsInfo { get; set; }

        public VM_ArtItem()
        {
            Category = new VM_Category();
            CommentsInfo = new VM_ArtCommentInfo();
        }
    }
}