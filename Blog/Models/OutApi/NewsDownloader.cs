using Bankiru.Models.Domain.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Xml;
using System.ServiceModel.Syndication;

namespace Bankiru.Models.OutApi
{
    public class NewsDownloader : IHttpModule
    {
        static Timer timer;
        long _interval = 600000;
        static object _synclock = new object();
        static string _lastError = "";        
        static int _counter = 0;
        static bool _isDownLoading = false;
        static TimeZoneInfo _moscowZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");

        static DateTime _lastLoadDateTime_ria_economy = DateTime.UtcNow;
        static DateTime _lastLoadDateTime_rambler_finance = DateTime.UtcNow;
        static DateTime _lastLoadDateTime_kommersant_economics = DateTime.UtcNow;
        static DateTime _lastLoadDateTime_vedomosti_finance = DateTime.UtcNow;

        public void Init(HttpApplication app)
        {
            NewsManager manager = new NewsManager();
            _lastLoadDateTime_rambler_finance = manager.GetMaxDateTime("RNS");
            _lastLoadDateTime_kommersant_economics = manager.GetMaxDateTime("Коммерсантъ");
            _lastLoadDateTime_vedomosti_finance = manager.GetMaxDateTime("Ведомости");
            timer = new Timer(new TimerCallback(LoadNews), null, 0, _interval);
        }

        private void LoadNews(object obj)
        {
            _counter++;
            if (_isDownLoading) return;
            try
            {
                _isDownLoading = true;
                DateTime lastLoadDateTime = DateTime.UtcNow;
                //LoadNews("http://ria.ru/export/rss2/economy/index.xml", "http://ria.ru", "РИА Новости", _lastLoadDateTime_ria_economy, ref lastLoadDateTime);
                //_lastLoadDateTime_ria_economy = lastLoadDateTime;
                LoadNews("http://finance.rambler.ru/xml/rambler_finance_news.xml", "http://finance.rambler.ru", "RNS", _lastLoadDateTime_rambler_finance, ref lastLoadDateTime);
                _lastLoadDateTime_rambler_finance = lastLoadDateTime;
                LoadNews("http://www.kommersant.ru/RSS/section-economics.xml", "http://www.kommersant.ru", "Коммерсантъ", _lastLoadDateTime_kommersant_economics, ref lastLoadDateTime);
                _lastLoadDateTime_kommersant_economics = lastLoadDateTime;
                LoadNews("http://www.vedomosti.ru/rss/rubric/finance", "http://www.vedomosti.ru", "Ведомости", _lastLoadDateTime_vedomosti_finance, ref lastLoadDateTime);
                _lastLoadDateTime_vedomosti_finance = lastLoadDateTime;
            }
            finally
            {
                _isDownLoading = false;
            }            
        }

        private void LoadNews(string rss, string url, string author, DateTime lastLoadDateTime, ref DateTime newLastLoadDateTime)
        {
            //newLastLoadDateTime = DateTimeOffset.Now;                      
            //newLastLoadDateTime = TimeZoneInfo.ConvertTime(DateTime.Now.ToUniversalTime(), _moscowZone);
            newLastLoadDateTime = DateTime.UtcNow;            
            lock (_synclock)
            {
                try
                {
                    NewsManager manager = new NewsManager();
                    VM_News news = null;
                    using (XmlReader reader = XmlReader.Create(rss))
                    {
                        var formatter = new Rss20FeedFormatter();
                        formatter.ReadFrom(reader);
                        if (formatter.Feed.Items != null && formatter.Feed.Items.Count<SyndicationItem>() > 0)
                        {                            
                            foreach (SyndicationItem item in formatter.Feed.Items)
                            {
                                try
                                {
                                    if (item.PublishDate.UtcDateTime > lastLoadDateTime)
                                    {
                                        news = new VM_News();
                                        news.IsActive = true;
                                        news.Title = item.Title.Text;
                                        news.NewsText = item.Summary.Text;
                                        news.PublishedAt = item.PublishDate.UtcDateTime;
                                        news.MetaTitle = item.Title.Text;
                                        news.MetaDesc = item.Summary.Text.Length > 150 ? item.Summary.Text.Substring(0, 150) + "..." : item.Summary.Text;
                                        news.MetaNoFollow = false;
                                        news.MetaNoIndex = false;
                                        news.Author = author;
                                        news.SourceUrl = url;

                                        var newsUrl = item.Links.FirstOrDefault(l => l.RelationshipType == "alternate");
                                        if (newsUrl != null)
                                            news.NewsUrl = newsUrl.Uri.AbsoluteUri.ToString();

                                        var imgUrl = item.Links.FirstOrDefault(l => l.RelationshipType == "enclosure");
                                        if (imgUrl != null)
                                            news.PictureUrl = imgUrl.Uri.AbsoluteUri.ToString();

                                        if (!manager.CreateNews(ref news))
                                        {
                                            _lastError = manager.LastError;
                                        }
                                    }
                                }
                                catch
                                {
                                    //log
                                }
                            }
                        }
                        newLastLoadDateTime = (from i in formatter.Feed.Items select i.PublishDate).Max().UtcDateTime;
                    }                    
                }
                catch (WebException ex)
                {
                    _lastError = "Syndication Reader: " + ex.ToString();
                }
            }
        }

        public void Dispose()
        {
        
        }
    }

}