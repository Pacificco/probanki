using Bankiru.Models.Domain.Orgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Helpers
{
    public static class OrgsHelper
    {
        public static MvcHtmlString GetOrgsList(this HtmlHelper html, List<VM_OrgItem> orgs, int num, Func<int, string> pageUrl)
        {
            //StringBuilder result = new StringBuilder();

            int n = num;

            string table = String.Empty;
            table += "<table class=\"orgs\">";
            
            //Шапка
            table += "<tr>";
            //Инструменты
            table += "<th class=\"tools\">";

            table += "</th>";
            //Номер по порядку
            table += "<th class=\"num\">";
            table += "№";
            table += "</th>";
            //Публикация
            table += "<th class=\"published\">";
            table += "А";
            table += "</th>";
            //Название организации
            table += "<th class=\"title\">";
            table += "Наименование организации";
            table += "</th>";
            //Дочерние организации
            table += "<th class=\"childs\">";
            table += "Дочки";
            table += "</th>";
            //Регион
            table += "<th class=\"region\">";
            table += "Регион";
            table += "</th>";
            //Id
            table += "<th class=\"id\">";
            table += "Id";
            table += "</th>";

            table += "</tr>";

            bool evel = false;
            foreach (VM_OrgItem item in orgs)
            {
                if(evel)
                    table += "<tr class=\"evel\">";
                else
                    table += "<tr>";
                //Инструменты
                table += "<td class=\"tools\">";

                table += "</td>";
                //Номер по порядку
                table += "<td class=\"num\">";
                table += n.ToString();
                table += "</td>";
                //Публикация
                table += "<td class=\"published\">";
                
                table += "</td>";
                //Название организации
                table += "<td class=\"title\">";
                table += string.Format("<a title=\"{0}\" href=\"{1}\">{0}</a>", item.Title, pageUrl(item.Id));
                table += "<br />";
                table += string.Format("<span class=\"alias\">{0}</span>",item.Alias);
                if(item.Parent != null)
                {
                    table += "<br />";
                    table += "<span class=\"text-gray\">Родительская организация:</span>";
                    table += "<br />";
                    table += string.Format("<a title=\"{0}\" href=\"{1}\">{0}</a>", item.Parent.Title, pageUrl(item.Parent.Id));
                }
                table += "</td>";
                //Дочерние организации
                table += "<td class=\"childs\">";
                table += "0";
                table += "</td>";
                //Регион
                table += "<td class=\"region\">";
                table += item.Region;
                table += "</td>";
                //Id
                table += "<td class=\"id\">";
                table += item.Id.ToString();
                table += "</td>";

                table += "</tr>";

                evel = !evel;
                n++;
            }
            return MvcHtmlString.Create(table);
        }        
    }
}