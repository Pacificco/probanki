using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using log4net;
using System.Xml;
using System.Xml.Serialization;
using Currencies;
using System.Globalization;

namespace Bankiru.Models.OutApi
{
    public class CurrenciesDownloader : IHttpModule
    {
        static Timer timer;
        long _interval = 300000; //5 минут
        static object _synclock = new object();
        static string[] currenciesNames = new string[] { "USD", "EUR" };
        static bool _isDownLoading = false;
        private TimeZoneInfo _moscowZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
        
        public static readonly ILog log = LogManager.GetLogger(typeof(CurrenciesDownloader));

        public void Init(HttpApplication app)
        {
            timer = new Timer(new TimerCallback(LoadCurrencies), null, 0, _interval);
        }
        private void LoadCurrencies(object obj)
        {
            if (_isDownLoading) return;
            _isDownLoading = true;
            double rateNow = 0.0F;            
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ",";
            try
            {
                //Загружаем текущий курс                
                //DateTime utcTime = DateTime.Now.ToUniversalTime();
                //DateTime dateNow = TimeZoneInfo.ConvertTime(utcTime, _moscowZone);
                DateTime dateNow = DateTime.Now.ToUniversalTime().AddHours(3).AddDays(1);
                //DateTime dateNow = DateTime.Now.AddDays(1);
                WebResponseAnswer answer = WebRequestsCBR.GetCurrentCourses(dateNow);
                if (answer.HttpCode == 200) //успешный  запрос
                {
                    using (XmlReader xmlReader = new XmlTextReader(answer.Stream))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(XML_Answer));
                        XML_Answer xmlAnswer = (XML_Answer)xmlSerializer.Deserialize(xmlReader);
                        CurrenciesManager.RateDate = xmlAnswer.Date;
                        foreach (string name in currenciesNames)
                        {
                            if (!CurrenciesManager.Currencies.Keys.Contains(name))
                                CurrenciesManager.Currencies.Add(name, new CurrencyRate());

                            XML_CurrencyItem xmlCurrencyItem = xmlAnswer.CurrencyItems.FirstOrDefault(a => a.CharCode.Equals(name));                            
                            if (xmlCurrencyItem != null)
                            {
                                try
                                {
                                    rateNow = Convert.ToDouble(xmlCurrencyItem.Value, provider);
                                }
                                catch
                                {
                                    rateNow = 0.0F;
                                }
                            }
                            if (rateNow > 0.0F )
                            {
                                CurrenciesManager.Currencies[name].RateNow = rateNow;
                            }
                        }
                    }
                }
                else
                {
                    log.Error("Не удалось загрузить курсы валют!");
                    return;
                }

                _loadCurrenciesRatePrev();                
            }
            catch (Exception ex)
            {
                log.Error("Ошибка во время загрузки курсов валют!\n" + ex.ToString());
            }
            finally
            {
                _isDownLoading = false;
            }
        }        
        private void _loadCurrenciesRatePrev()
        {
            double ratePrev = 0.0F;
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ",";
            //DateTime utcTime = DateTime.Now.ToUniversalTime();
            //DateTime today = TimeZoneInfo.ConvertTime(utcTime, _moscowZone);
            DateTime today = DateTime.Now.ToUniversalTime().AddHours(3).AddDays(1);
            int counter = 0;
            bool rateFined = false;
            try
            {
                do
                {
                    counter++;
                    today = today.AddDays(-1);
                    WebResponseAnswer answer = WebRequestsCBR.GetCurrentCourses(today);
                    if (answer.HttpCode == 200) //успешный  запрос
                    {
                        using (XmlReader xmlReader = new XmlTextReader(answer.Stream))
                        {
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XML_Answer));
                            XML_Answer xmlAnswer = (XML_Answer)xmlSerializer.Deserialize(xmlReader);
                            foreach (string name in currenciesNames)
                            {
                                if (!CurrenciesManager.Currencies.Keys.Contains(name))
                                    CurrenciesManager.Currencies.Add(name, new CurrencyRate());

                                XML_CurrencyItem xmlCurrencyItem = xmlAnswer.CurrencyItems.FirstOrDefault(a => a.CharCode.Equals(name));
                                if (xmlCurrencyItem != null)
                                {
                                    try
                                    {
                                        ratePrev = Convert.ToDouble(xmlCurrencyItem.Value, provider);
                                    }
                                    catch
                                    {
                                        ratePrev = 0.0F;
                                    }
                                }
                                if (ratePrev > 0.0F & CurrenciesManager.Currencies[name].RateNow != ratePrev)
                                {
                                    CurrenciesManager.Currencies[name].RatePrev = ratePrev;
                                    rateFined = true;
                                }
                                else
                                {
                                    Thread.Sleep(100);
                                }
                            }
                        }
                    }
                    else
                    {
                        log.Error("Не удалось загрузить курсы валют!");
                    }
                }
                while (counter < 6 & !rateFined);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка во время загрузки курсов валют!\n" + ex.ToString());
            }
        }

        private void _saveCurrenciesRates(double usdRate, double eurRate)
        {
            
        }

        public void Dispose()
        {

        }
    }
}