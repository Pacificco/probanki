using Bankiru.Models.Domain.Categories;
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
    public static class MenuHelpers
    {
        public static MvcHtmlString CategoriesMenu(this HtmlHelper html, List<VM_Category> categories, int currentId, Func<int, int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            TagBuilder tagSection = new TagBuilder("section");
            tagSection.MergeAttribute("class","module-menu");
            TagBuilder tagNav = new TagBuilder("nav");            
            TagBuilder tagUl = new TagBuilder("ul");
            tagUl.InnerHtml = "";
            TagBuilder tagLi = null;
            TagBuilder tagA = null;
            foreach (VM_Category cat in categories)
            {
                tagLi = new TagBuilder("li");
                tagA = new TagBuilder("a");
                tagA.MergeAttribute("href", pageUrl(cat.Id, 1));
                tagA.MergeAttribute("title", cat.Title);
                tagA.InnerHtml = cat.Title.ToString();
                if (cat.Id == currentId)
                    tagLi.AddCssClass("selected");
                tagLi.InnerHtml = tagA.ToString();
                tagUl.InnerHtml += tagLi.ToString();
            }
            tagNav.InnerHtml = tagUl.ToString();
            tagSection.InnerHtml = tagNav.ToString();            
            result.Append(tagSection.ToString());
            string res = result.ToString();
            return MvcHtmlString.Create(res);
        }
        public static MvcHtmlString GetMenuItemClass(this HtmlHelper html, string item, string currentItem)
        {
            string result = "";
            if (String.IsNullOrEmpty(currentItem))
            {
                result = "item";
            }
            else
            {
                if (currentItem.Equals(item))
                    result = "item current";
                else
                    result = "item";
            }
            return MvcHtmlString.Create(result);
        }
    }
}