using Bankiru.Models.Domain.Comments;
using Bankiru.Models.Domain.Users;
using Bankiru.Models.ViewModels;
using System;
using System.Collections.Generic;
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
        public static string GetTariffName(VM_ForecastTariff tariff)
        {
            switch (tariff)
            {
                case VM_ForecastTariff.Platinum:
                    return "Платинум";
                case VM_ForecastTariff.Gold:
                    return "Золото";
                case VM_ForecastTariff.Silver:
                    return "Серебро";
                case VM_ForecastTariff.Bronze:
                    return "Бронза";
                default:
                    return "?";
            }
        }
    }
}