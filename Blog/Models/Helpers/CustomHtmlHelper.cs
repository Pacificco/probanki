using Bankiru.Models.Domain.Comments;
using Bankiru.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Helpers
{
    public static class CustomHtmlHelper
    {
        public static MvcHtmlString GetCommentsList(this HtmlHelper html, List<VM_Comment> comments, int articleId, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            
            TagBuilder tagDiv = null;
            TagBuilder tagP = null;
            TagBuilder tagDate = null;
            TagBuilder tagReadMore = null;
            TagBuilder tagA = null;
            foreach (VM_Comment com in comments)
            {
                tagDiv = new TagBuilder("div");
                tagDiv.MergeAttribute("class","comment-item");                
                tagDiv.InnerHtml = "";

                tagDate = new TagBuilder("p");
                tagDate.MergeAttribute("class", "comment-date");
                tagDate.InnerHtml = com.CreatedAt.ToShortDateString();
                tagDiv.InnerHtml += tagDate.ToString();

                tagP = new TagBuilder("p");
                tagP.MergeAttribute("class", "comment-text");
                tagP.InnerHtml = com.CommentText;
                tagDiv.InnerHtml += tagP.ToString();

                tagReadMore = new TagBuilder("p");
                tagReadMore.MergeAttribute("class","comment-readmore");
                tagA = new TagBuilder("a");
                tagA.MergeAttribute("href", "#");
                tagA.MergeAttribute("title", "Подробнее...");
                tagA.InnerHtml = "Подробнее...";
                tagReadMore.InnerHtml = tagA.ToString();
                tagDiv.InnerHtml += tagReadMore.ToString();

                result.Append(tagDiv.ToString());    
            }
            return MvcHtmlString.Create(result.ToString());
        }        
    }
}