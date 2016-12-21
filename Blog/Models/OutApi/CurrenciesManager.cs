using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Bankiru.Models.OutApi
{
    public static class CurrenciesManager
    {
        public static string RateDate = "";
        public static Dictionary<string, CurrencyRate> Currencies = new Dictionary<string, CurrencyRate>();
        public static readonly ILog log = LogManager.GetLogger(typeof(CurrenciesManager));

        public static string CurrenciesToHtml()
        {
            try
            {
                string result = String.Empty;
                if (Currencies.Count > 0)
                {
                    foreach (var c in Currencies)
                    {
                        result += "<div class=\"currency\">";
                        result += "<span class=\"currency-name\">";
                        result += c.Key == "USD" ? "<img src=\"/Content/system/usd.png\" title=\"Динамика курса доллара\" alt=\"Курс доллара\" />" :
                            "<img src=\"/Content/system/euro.png\" title=\"Динамика курса евро\" alt=\"Курс евро\" />";
                        result += "</span>";
                        result += c.Value.CurrencyToHtml();
                        result += "</div>";
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
                return "";
            }
        }
    }

    public class CurrencyRate
    {
        public double RateNow {get; set;}
        public double RatePrev {get; set;}
        public CurrencyRate()
        {
            RateNow = 0.0F;
            RatePrev = 0.0F;
        }
        public string CurrencyToHtml()
        {
            string result = String.Empty;

            string rate = RateNow.ToString(CultureInfo.CurrentCulture);
            if (rate.Length < 7)
                while (rate.Length < 7)
                    rate += "0";

            string dif = Math.Round(Math.Abs(RateNow - RatePrev), 4).ToString(CultureInfo.CurrentCulture);
                if(dif.Length < 6)
                    while (dif.Length < 6)
                        dif += "0";

            if (RateNow > 0.0F & RatePrev > 0.0F)
            {
                bool up = (RateNow - RatePrev) > 0.0F ? true : false; 
                result += "<span class=\"currency-rate\">";
                result += rate;
                result += "</span>";
                result += String.Format("<img src=\"/Content/system/{0}\" title=\"Динамика курса\" alt=\"курс\" />",
                    up ? "rate_up.png" : "rate_down.png");
                result += "<br />";
                result += String.Format("<span class=\"currency-{0}\">", up ? "up" : "down");
                result += up ? "+" : "-";
                result += dif;
                result += "</span>";
            }
            else if (RateNow > 0.0F)
            {
                result += "<span class=\"currency-rate\">";
                result += RateNow.ToString(CultureInfo.CurrentCulture);
                result += "</span>";
            }
            return result;
        }
    }
}