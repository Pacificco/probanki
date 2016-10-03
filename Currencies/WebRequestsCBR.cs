using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using log4net;

namespace Currencies
{
    /// <summary>
    /// Класс HTTP-запросов к сайту Центробанка России
    /// </summary>
    public static class WebRequestsCBR
    {
        private const string _baseUrl = "http://www.cbr.ru/scripts/XML_daily.asp";
        public static readonly ILog log = LogManager.GetLogger(typeof(WebRequestsCBR));

        public static WebResponseAnswer GetCurrentCourses(DateTime today, WebProxy proxy = null)
        {            
            string query = string.Format("{0}?date_req={1}/{2}/{3}", _baseUrl, today.Day.ToString("00"), today.Month.ToString("00"), today.Year);
            log.Info(query);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(query);
            req.Method = WebRequestMethods.Http.Get;
            req.ContentType = "application/xml";
            req.KeepAlive = false;
            if (proxy != null)
            {
                req.Proxy = proxy;
                req.Proxy.Credentials = CredentialCache.DefaultCredentials;//AD-авторизация (получаем разрешения пользователя по умолчанию)
                req.PreAuthenticate = true; //нужна для работоспособности(?)
            }
            req.Timeout = 15000;
            try
            {
                WebResponse resp = req.GetResponse();
                int code = (int)(resp as HttpWebResponse).StatusCode;
                log.Info(string.Format("HttpAnswer: {0}", code));
                Stream stream = resp.GetResponseStream();
                return new WebResponseAnswer(code, string.Empty, stream);
            }
            catch (WebException ex)
            {
                log.Info(ex.ToString());
                return new WebResponseAnswer(null, ex.ToString(), null);
            }
        }
       
    }
}
