using Bankiru.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Comments
{
    public class VM_Comments
    {
        public bool ShowArticleLink { get; set; }
        public VM_CommentsFilters Filters { get; set; }
        public List<VM_CommentItem> Items { get; set; }
        public VM_PagingInfo PagingInfo { get; set; }

        public VM_Comments()
        {
            Filters = new VM_CommentsFilters();
            Items = new List<VM_CommentItem>();
            PagingInfo = new VM_PagingInfo();
        }
    }
}