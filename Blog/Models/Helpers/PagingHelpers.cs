using Bankiru.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Helpers
{
    /// <summary>
    /// Помошник постраничной навигации
    /// </summary>
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, VM_PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            TagBuilder tag;            
            TagBuilder tagDiv = new TagBuilder("div");
            tagDiv.AddCssClass("page-nav");
            
            tagDiv.InnerHtml = String.Format("Найдено: <span class=\"page-nav-total\">{0}</span>", pagingInfo.TotalItems);
            
            if (pagingInfo.TotalPages < 6 || pagingInfo.CurrentPage < 4)
            {                
                for (int i = 1; i <= pagingInfo.TotalPages; i++)
                {
                    tag = new TagBuilder("a");
                    tag.MergeAttribute("href", pageUrl(i));
                    tag.InnerHtml = i.ToString();
                    if (i == pagingInfo.CurrentPage)
                        tag.AddCssClass("selected");
                    tagDiv.InnerHtml += tag.ToString();

                    if (i >= 5) break;
                }
                if (pagingInfo.TotalPages > 5)
                {
                    tagDiv.InnerHtml += " . . . ";
                    tagDiv.InnerHtml += String.Format("<a class=\"page-nav-forward\" title=\"Далее\" href=\"{0}\"><img src=\"/Content/system/page-nav-forward.png\" title=\"Далее\" alt=\"Далее\" /></a>", pageUrl(pagingInfo.CurrentPage + 1));
                    tagDiv.InnerHtml += String.Format("<a class=\"page-nav-last\" title=\"В конец\" href=\"{0}\"><img src=\"/Content/system/page-nav-last.png\" title=\"В конец\" alt=\"В конец\" /></a>", pageUrl(pagingInfo.TotalPages));
                }
            }
            else
            {
                if (pagingInfo.CurrentPage < (pagingInfo.TotalPages - 2))
                {
                    tagDiv.InnerHtml += String.Format("<a class=\"page-nav-first\" title=\"К началу\" href=\"{0}\"><img src=\"/Content/system/page-nav-first.png\" title=\"К началу\" alt=\"К началу\" /></a>", pageUrl(1));
                    tagDiv.InnerHtml += String.Format("<a class=\"page-nav-back\" title=\"Назад\" href=\"{0}\"><img src=\"/Content/system/page-nav-back.png\" title=\"Назад\" alt=\"Назад\" /></a>", pageUrl(pagingInfo.CurrentPage - 1));
                    tagDiv.InnerHtml += " . . . ";
                    for (int i = pagingInfo.CurrentPage - 2; i <= pagingInfo.CurrentPage + 2; i++)
                    {
                        tag = new TagBuilder("a");
                        tag.MergeAttribute("href", pageUrl(i));
                        tag.InnerHtml = i.ToString();
                        if (i == pagingInfo.CurrentPage)
                            tag.AddCssClass("selected");
                        tagDiv.InnerHtml += tag.ToString();
                    }
                    tagDiv.InnerHtml += " . . . ";
                    tagDiv.InnerHtml += String.Format("<a class=\"page-nav-forward\" title=\"Далее\" href=\"{0}\"><img src=\"/Content/system/page-nav-forward.png\" title=\"Далее\" alt=\"Далее\" /></a>", pageUrl(pagingInfo.CurrentPage + 1));
                    tagDiv.InnerHtml += String.Format("<a class=\"page-nav-last\" title=\"В конец\" href=\"{0}\"><img src=\"/Content/system/page-nav-last.png\" title=\"В конец\" alt=\"В конец\" /></a>", pageUrl(pagingInfo.TotalPages));
                }
                else
                {
                    tagDiv.InnerHtml += String.Format("<a class=\"page-nav-first\" title=\"К началу\" href=\"{0}\"><img src=\"/Content/system/page-nav-first.png\" title=\"К началу\" alt=\"К началу\" /></a>", pageUrl(1));
                    tagDiv.InnerHtml += String.Format("<a class=\"page-nav-back\" title=\"Назад\" href=\"{0}\"><img src=\"/Content/system/page-nav-back.png\" title=\"Назад\" alt=\"Назад\" /></a>", pageUrl(pagingInfo.CurrentPage - 1));
                    tagDiv.InnerHtml += " . . . ";
                    for (int i = pagingInfo.TotalPages - 4; i <= pagingInfo.TotalPages; i++)
                    {                        
                        tag = new TagBuilder("a");
                        tag.MergeAttribute("href", pageUrl(i));
                        tag.InnerHtml = i.ToString();
                        if (i == pagingInfo.CurrentPage)
                            tag.AddCssClass("selected");
                        tagDiv.InnerHtml += tag.ToString();                        
                    }
                }
            }

            result.Append(tagDiv);
            return MvcHtmlString.Create(result.ToString());
        }
        public static MvcHtmlString BreadCrumbsLinks(this HtmlHelper html, Dictionary<string, string> links, Func<string, string> pageUrl)
        {
            string result = "";
            result += "<p id=\"bread-crumbs\">";
            result += "<span>Вы сейчас здесь:</span> ";
            if (links.Count > 0)
                result += String.Format("<a href=\"{0}\" title=\"Images\">Images</a>", pageUrl(""));
            else
                result += "Images";
                if (links != null && links.Count > 0)
                {
                    int i = 1;
                    foreach (var bc in links)
                    {
                        result += "<img class=\"separator\" src=\"/Content/system/nav-arrow.png\" title=\"Разделитель\" alt=\">>\" />";
                        if (i < links.Count)
                            result += String.Format("<a href=\"{0}\" title=\"{1}\">{1}</a>", pageUrl(bc.Value), bc.Key);
                        else
                            result += bc.Key;
                        i++;
                    }
                }
                result += "</p>";
            return MvcHtmlString.Create(result);
        }
    }
}