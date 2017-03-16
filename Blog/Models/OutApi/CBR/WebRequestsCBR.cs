using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using log4net;

namespace Bankiru.Models.OutApi.CBR
{
    /// <summary>
    /// Класс HTTP-запросов к сайту Центробанка России
    /// </summary>
    public static class WebRequestsCBR
    {
        private const string _baseUrl = "http://www.cbr.ru/scripts/XML_dynamic.asp?date_req1={0}&date_req2={1}&VAL_NM_RQ={2}";
        public static readonly ILog log = LogManager.GetLogger(typeof(WebRequestsCBR));

        public static WebResponseCBRAnswer GetCurrentCourses(DateTime from, DateTime to, string subject)
        {
            string query = string.Format(_baseUrl,
                String.Format("{0}/{1}/{2}", from.ToString("dd"), from.ToString("MM"), from.ToString("yyyy")),
                String.Format("{0}/{1}/{2}", to.ToString("dd"), to.ToString("MM"), to.ToString("yyyy")),
                subject);
            //log.Info(query);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(query);
            req.Method = WebRequestMethods.Http.Get;
            req.ContentType = "application/xml";
            req.KeepAlive = false;
            //if (proxy != null)
            //{
            //    req.Proxy = proxy;
            //    req.Proxy.Credentials = CredentialCache.DefaultCredentials;//AD-авторизация (получаем разрешения пользователя по умолчанию)
            //    req.PreAuthenticate = true; //нужна для работоспособности(?)
            //}
            req.Timeout = 15000;
            try
            {
                WebResponse resp = req.GetResponse();
                int code = (int)(resp as HttpWebResponse).StatusCode;
                //log.Info(string.Format("HttpAnswer: {0}", code));
                Stream stream = resp.GetResponseStream();
                return new WebResponseCBRAnswer(code, string.Empty, stream);
            }
            catch (WebException ex)
            {
                log.Error("[currencies]\r\n" + ex.ToString());
                return new WebResponseCBRAnswer(null, ex.ToString(), null);
            }
        }
       
    }
}
