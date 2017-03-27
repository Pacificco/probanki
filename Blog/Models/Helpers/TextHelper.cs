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
                    return String.Format("Аналитики прогнозируют: Курс доллара (ЦБ РФ) на {0}", endDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("ru-RU")));
                case 2:
                    return String.Format("Аналитики прогнозируют: Курс евро (ЦБ РФ) на {0}", endDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("ru-RU")));
                case 3:
                    return String.Format("Аналитики прогнозируют: Курс акций Роснефти (на Мосбирже) на {0}", endDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("ru-RU")));
                case 4:
                    return String.Format("Аналитики прогнозируют: Курс акций Сбербанка (на Мосбирже) на {0}", endDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("ru-RU")));
                default:
                    return "Текущие прогнозы";
            }
        }
        /// <summary>
        /// Выполняет попытку привести строковое значение в тип double
        /// </summary>
        /// <param name="value">Строковое значение</param>
        /// <param name="success">Флаг успешного или не успешного приведения значения</param>
        /// <returns>Число</returns>
        public static double DoubleParse(string value, out bool success)
        {
            try
            {
                string v = value.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                v = v.Replace(",", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                if (v.Last().ToString() == System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                {
                    v += "0";
                }
                double result = double.Parse(v);
                success = true;
                return result;
            }
            catch
            {
                success = false;
                return 0.0F;
            }
        }
    }
}