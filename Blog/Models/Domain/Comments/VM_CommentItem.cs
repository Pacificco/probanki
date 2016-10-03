using Bankiru.Models.Domain.Articles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Comments
{
    public class VM_CommentItem
    {
        [HiddenInput]
        public int Id { set; get; }
        [Display(Name = "Текст комментария")]        
        public string CommentText { set; get; }        
        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { set; get; }
        [Display(Name = "Идентификатор статьи")]
        public VM_ArtItem Article { set; get; }
        [Display(Name = "Активный")]
        public bool IsActive { set; get; }
        [Display(Name = "Состояние")]
        public int Confirmed { set; get; }
        [Display(Name = "Лайки")]
        public int LikeCount { set; get; }
        [Display(Name = "Дезлайки")]
        public int DisLikeCount { set; get; }
        [Display(Name = "Автор")]
        public string Author { set; get; }        

        public VM_CommentItem()
        {
            Article = new VM_ArtItem();
        }
    }
}