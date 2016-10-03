using Bankiru.Models.Domain.Articles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Comments
{
    public class VM_Comment
    {
        [HiddenInput]
        public int Id { set; get; }
        [Display(Name = "Текст комментария")]
        [Required(ErrorMessage = "Текст комментария не может быть пустым!")]
        [AllowHtml]
        public string CommentText { set; get; }
        [Display(Name = "Автор")]
        public int UserId { set; get; }
        [Display(Name = "Автор комментария")]        
        public string UserName { set; get; }
        [Display(Name = "Родительский комментарий")]
        public int? ParentId { set; get; }
        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { set; get; }
        [Display(Name = "Публикация")]
        public VM_ArtItem Article { set; get; }
        [Display(Name = "Активный")]
        public bool IsActive { set; get; }
        //[Display(Name = "Подтвержден")]
        //public bool Confirmed { set; get; }
        [Display(Name = "Лайки")]
        public int LikeCount { set; get; }
        [Display(Name = "Дезлайки")]
        public int DisLikeCount { set; get; }
        [Display(Name = "Состояние")]
        public EnumBoolType Confirmed { set; get; }

        public VM_Comment()
        {
            ParentId = null;
            Article = new VM_ArtItem();
        }
    }
}