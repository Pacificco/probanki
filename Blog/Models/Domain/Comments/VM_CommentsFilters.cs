using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Comments
{
    public class VM_CommentsFilters
    {
        [Display(Name = "Текст комментария")]
        public string CommentText { get; set; }        
        [Display(Name = "Статус")]
        public EnumBoolType Confirmed { get; set; }        
        [Display(Name = "Активность")]
        public EnumBoolType IsActive  { get; set; }

        public VM_CommentsFilters()
        {
            CommentText = "";
            Confirmed = EnumBoolType.None;
            IsActive = EnumBoolType.None;
        }
        public void Assign(VM_CommentsFilters obj)
        {
            CommentText = obj.CommentText;
            Confirmed = obj.Confirmed;
            IsActive = obj.IsActive;
        }
        public object GetFilterAsRouteValues(int currentPage)
        {
            return new
            {
                ConfirmStatus = Confirmed,
                commentText = CommentText,
                IsActive = (int)IsActive,
                page = currentPage
            };
        }
    }
}