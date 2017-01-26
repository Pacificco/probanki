using Bankiru.Models.Domain;
using Bankiru.Models.Domain.Comments;
using Bankiru.Models.Domain.Users;
using Bankiru.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Helpers
{
    public static class TextHelper
    {        
        public static string GetImageFromText(string text)
        {
            string imgText = text.ToLower();
            int i = imgText.IndexOf("<img");
            if (i == -1)
            {
                return "<br />";
            }
            else
            {
                imgText = imgText.Substring(i);
                int j = imgText.IndexOf(">");
                if (j == -1)
                {
                    return "<br />";
                }
                else
                {
                    imgText = imgText.Substring(0, j + 1);
                }
            }
            return imgText +"<br />";
        }
        public static string GetForecastName(int subjectId, DateTime endDate)
        {
            switch(subjectId)
            {
                case 1:
                    return String.Format("Текущие прогнозы курса доллара на {0}", endDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("ru-RU")));
                case 2:
                    return String.Format("Текущие прогнозы курса евро на {0}", endDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("ru-RU")));
                case 3:
                    return String.Format("Текущие прогнозы стоимости нефти на {0}", endDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("ru-RU")));
                case 4:
                    return String.Format("Текущие прогнозы акций Сбербанка на {0}", endDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("ru-RU")));
                default:
                    return "Текущие прогнозы";
            }
        }
    }
}