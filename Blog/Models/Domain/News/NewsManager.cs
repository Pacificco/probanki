using Bankiru.Models.DataBase;
using Bankiru.Models.Domain.Categories;
using Bankiru.Models.Domain.Other;
using Bankiru.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.News
{
    public class NewsManager
    {
        public string LastError = "";

        //Список новостей
        public VM_NewsList GetNewsList(VM_NewsFilters filter, int page = 1, int itemPerPage = 10)
        {
            VM_NewsList _arts = new VM_NewsList();
            _arts.Filters.Assign(filter);
            _arts.PagingInfo.ItemsPerPage = itemPerPage;
            _arts.PagingInfo.SetData(page, _getNewsListTotalCount(filter));
            if (_arts.PagingInfo.TotalItems == -1) return null;
            _arts.PagingInfo.CurrentPage = page;
            _arts.Items = _getNewsList(filter, _arts.PagingInfo.GetNumberFrom(), _arts.PagingInfo.GetNumberTo());
            return _arts;
        }

        //Новость
        public VM_News GetNews(int id)
        {
            string SQLQuery = _sqlGetNews(id);
            VM_News news = new VM_News();
            SqlCommand _command = null;
            SqlDataReader _reader = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    _reader = _command.ExecuteReader();
                    if (_reader == null)
                    {
                        LastError = "Ошибка во время загрузки новости (id=\"" + id.ToString() + "\")!\nСервер вернул ответ NULL.";
                        return null;
                    }

                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            news.Id = _reader.GetInt32(0);
                            news.CreatedAt = _reader.GetDateTime(1);
                            news.Title = _reader.GetString(2);
                            news.Alias= _reader.GetString(3);
                            news.NewsText= _reader.GetString(4);
                            news.MetaTitle= _reader.GetString(5);
                            news.MetaDesc = _reader.GetString(6);
                            news.MetaKeys = _reader.GetString(7);
                            news.MetaNoFollow = _reader.GetBoolean(8);
                            news.MetaNoIndex = _reader.GetBoolean(9);
                            news.IsActive = _reader.GetBoolean(10);
                            news.Author = _reader.GetString(11);
                            news.SourceUrl = _reader.GetString(12);
                            news.PublishedAt = _reader.GetDateTime(13);
                            news.NewsUrl = _reader.IsDBNull(14) ? String.Empty : _reader.GetString(14);
                            news.PictureUrl = _reader.IsDBNull(15) ? String.Empty : _reader.GetString(15);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки новости (id=\"" + id.ToString() + "\")!\n" + ex.ToString();
                    return null;
                }
                finally
                {
                    if (_reader != null)
                    {
                        _reader.Close();
                        _reader.Dispose();
                    }
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
            return news;
        }
        public VM_News GetNews(string alias)
        {
            string SQLQuery = _sqlGetNews(alias);
            VM_News news = new VM_News();
            SqlCommand _command = null;
            SqlDataReader _reader = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    _reader = _command.ExecuteReader();
                    if (_reader == null)
                    {
                        LastError = "Ошибка во время загрузки новости (alias=\"" + alias + "\")!\nСервер вернул ответ NULL.";
                        return null;
                    }

                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            news.Id = _reader.GetInt32(0);
                            news.CreatedAt = _reader.GetDateTime(1);
                            news.Title = _reader.GetString(2);
                            news.Alias = _reader.GetString(3);
                            news.NewsText = _reader.GetString(4);
                            news.MetaTitle = _reader.GetString(5);
                            news.MetaDesc = _reader.GetString(6);
                            news.MetaKeys = _reader.GetString(7);
                            news.MetaNoFollow = _reader.GetBoolean(8);
                            news.MetaNoIndex = _reader.GetBoolean(9);
                            news.IsActive = _reader.GetBoolean(10);
                            news.Author = _reader.GetString(11);
                            news.SourceUrl = _reader.GetString(12);
                            news.PublishedAt = _reader.GetDateTime(13);
                            news.NewsUrl = _reader.IsDBNull(14) ? String.Empty : _reader.GetString(14);
                            news.PictureUrl = _reader.IsDBNull(15) ? String.Empty : _reader.GetString(15);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки новости (alias=\"" + alias + "\")!\n" + ex.ToString();
                    return null;
                }
                finally
                {
                    if (_reader != null)
                    {
                        _reader.Close();
                        _reader.Dispose();
                    }
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
            return news;
        }
        public VM_NewsItem GetNewsItem(int id)
        {
            string SQLQuery = _sqlGetNewsItem(id);
            VM_NewsItem news = new VM_NewsItem();
            SqlCommand _command = null;
            SqlDataReader _reader = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    _reader = _command.ExecuteReader();
                    if (_reader == null)
                    {
                        LastError = "Ошибка во время загрузки новости (id=\"" + id.ToString() + "\")!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            news.Id = _reader.GetInt32(0);
                            news.CreatedAt = _reader.GetDateTime(1);
                            news.Title = _reader.GetString(2);
                            news.Alias = _reader.GetString(3);                            
                            news.IsActive = _reader.GetBoolean(4);
                            news.Author = _reader.GetString(5);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки новости (id=\"" + id.ToString() + "\")!\n" + ex.ToString();
                    return null;
                }
                finally
                {
                    if (_reader != null)
                    {
                        _reader.Close();
                        _reader.Dispose();
                    }
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
            return news;
        }
        public List<VM_NewsItem> GetNewsItems()
        {
            string SQLQuery = _sqlGetNewsItems();
            List<VM_NewsItem> newsList = new List<VM_NewsItem>();
            SqlCommand _command = null;
            SqlDataReader _reader = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    _reader = _command.ExecuteReader();
                    if (_reader == null)
                    {
                        LastError = "Ошибка во время загрузки новостей!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        while (_reader.Read())
                        {
                            newsList.Add(new VM_NewsItem()
                            {
                                Id = _reader.GetInt32(0),
                                CreatedAt = _reader.GetDateTime(1),
                                Title = _reader.GetString(2),
                                Alias= _reader.GetString(3),                            
                                IsActive = _reader.GetBoolean(4),
                                Author = _reader.GetString(5)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки новостей!\n" + ex.ToString();
                    return null;
                }
                finally
                {
                    if (_reader != null)
                    {
                        _reader.Close();
                        _reader.Dispose();
                    }
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
            return newsList;
        }
        public List<VM_NewsItem> GetLastNews(int rowCount)
        {
            string SQLQuery = _sqlGetLastNews(rowCount);
            List<VM_NewsItem> newsList = new List<VM_NewsItem>();
            SqlCommand _command = null;
            SqlDataReader _reader = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    _reader = _command.ExecuteReader();
                    if (_reader == null)
                    {
                        LastError = "Ошибка во время загрузки последних новостей!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_NewsItem news = null;
                        while (_reader.Read())
                        {
                            news = new VM_NewsItem();
                            news.Id = _reader.GetInt32(0);
                            news.Title = _reader.GetString(1);
                            news.Alias = _reader.GetString(2);
                            news.PublishedAt = _reader.GetDateTime(3);                            
                            newsList.Add(news);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки последних новостей!\n" + ex.ToString();
                    return null;
                }
                finally
                {
                    if (_reader != null)
                    {
                        _reader.Close();
                        _reader.Dispose();
                    }
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
            return newsList;
        }
       
        public bool CreateNews(ref VM_News news)
        {
            //Формирование ссылки
            string alias = GenerateAlias(news.Title);
            if (String.IsNullOrEmpty(alias)) return false;
            news.Alias = alias;                        
            //Формирование запроса
            string SQLQuery = _sqlCreateNews(news);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object objId = _command.ExecuteScalar();
                    if (objId == null)
                    {
                        LastError = "Ошибка во время создания новости!\nСервер вернул ответ NULL.";
                        return false;
                    }
                    //Новый Id
                    news.Id = Convert.ToInt32(objId.ToString());
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время создания новости!\n" + ex.ToString();
                    return false;
                }
                finally
                {
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
        }
        public bool UpdateNews(int id, VM_News news)
        {            
            //Формирование запроса
            string SQLQuery = _sqlUpdateNews(id, news);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время обновления новости!\nСервер вернул ответ " + i.ToString() + ".";
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время обновления новости!\n" + ex.ToString();
                    return false;
                }
                finally
                {
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
        }
        public bool DeleteNews(int id)
        {
            return true;
        }
        public bool SetNewsActive(int id, bool action)
        {
            //Формирование запроса
            string SQLQuery = _sqlSetNewsActive(id, action);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время создания новости!\nСервер вернул ответ " + i.ToString() + ".";
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время установки активности новости!\n" + ex.ToString();
                    return false;
                }
                finally
                {
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
        }

        #region ВНУТРЕННИЕ МЕТОДЫ
        //Поиск новостей
        private int _getNewsListTotalCount(VM_NewsFilters filter)
        {
            string SQLQuery = _sqlGetNewsListTotalCount(filter);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object count = _command.ExecuteScalar();
                    if (count == null)
                    {
                        LastError = "Ошибка во время определения общего числа новостей!\nСервер вернул ответ NULL.";
                        return -1;
                    }
                    return (int)count;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время определения общего числа новостей!\n" + ex.ToString();
                    return -1;
                }
                finally
                {
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
        }
        private string _sqlGetNewsListTotalCount(VM_NewsFilters filter)
        {
            string SqlConds = _sqlGetFilter(filter);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.COUNT + "(*)";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.News.TABLENAME;
            if (!String.IsNullOrEmpty(SqlConds))
            {
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += SqlConds;
            }
            SqlQuery += ";";
            return SqlQuery;
        }
        private List<VM_NewsItem> _getNewsList(VM_NewsFilters filter, int offset, int limit)
        {
            string SQLQuery = _sqlGetNewsList(filter, offset, limit);
            List<VM_NewsItem> items = new List<VM_NewsItem>();
            SqlCommand _command = null;
            SqlDataReader _reader = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    _reader = _command.ExecuteReader();
                    if (_reader == null)
                    {
                        LastError = "Ошибка во время загрузки новостей!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_NewsItem news = null;
                        while (_reader.Read())
                        {
                            news = new VM_NewsItem();
                            news.Id = _reader.GetInt32(1);
                            news.PublishedAt = _reader.GetDateTime(2);
                            news.Title = _reader.GetString(3);
                            news.Alias = _reader.GetString(4);                            
                            news.IsActive = _reader.GetBoolean(5);
                            news.Author = _reader.GetString(6);
                            items.Add(news);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки новостей!\n" + ex.ToString();
                    return null;
                }
                finally
                {
                    if (_reader != null)
                    {
                        _reader.Close();
                        _reader.Dispose();
                    }
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
            return items;
        }
        private List<VM_NewsItem> _getNewsList(int[] ids)
        {
            string SQLQuery = _sqlGetNewsList(ids);
            List<VM_NewsItem> items = new List<VM_NewsItem>();
            SqlCommand _command = null;
            SqlDataReader _reader = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    _reader = _command.ExecuteReader();
                    if (_reader == null)
                    {
                        LastError = "Ошибка во время загрузки новостей!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_NewsItem news = null;
                        while (_reader.Read())
                        {
                            news = new VM_NewsItem();
                            news.Id = _reader.GetInt32(0);
                            news.PublishedAt = _reader.GetDateTime(1);
                            news.Title = _reader.GetString(2);
                            news.Alias = _reader.GetString(3);                            
                            news.IsActive = _reader.GetBoolean(4);
                            news.Author = _reader.GetString(5);
                            items.Add(news);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки новостей!\n" + ex.ToString();
                    return null;
                }
                finally
                {
                    if (_reader != null)
                    {
                        _reader.Close();
                        _reader.Dispose();
                    }
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
            return items;
        }        
        private bool _dbAliasExists(string alias)
        {
            string SQLQuery = _sqlGetAliasExists(alias);
            SqlCommand _command = null;
            SqlDataReader _reader = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    _reader = _command.ExecuteReader();
                    if (_reader != null && _reader.HasRows)
                        return true;
                    return false;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время проверки ссылки на существование!\n" + ex.ToString();
                    return false;
                }
                finally
                {
                    if (_reader != null)
                    {
                        _reader.Close();
                        _reader.Dispose();
                    }
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
        }

        //Sql
        private string _sqlGetNewsList(VM_NewsFilters filter, int offset, int limit)
        {
            string SqlConds = _sqlGetFilter(filter);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " t1.* " + DbStruct.SE.FROM + "(";
            SqlQuery += DbStruct.SE.SELECT + " ROW_NUMBER() OVER(";
            SqlQuery += DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.News.FIELDS.PublishedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ") AS RowNumber, ";
            SqlQuery += DbStruct.News.FIELDS.Id + ",";
            SqlQuery += DbStruct.News.FIELDS.PublishedAt + ",";            
            SqlQuery += DbStruct.News.FIELDS.Title + ",";
            SqlQuery += DbStruct.News.FIELDS.Alias + ",";
            SqlQuery += DbStruct.News.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.News.FIELDS.Author;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.News.TABLENAME;
            if (!String.IsNullOrEmpty(SqlConds))
            {
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += SqlConds;
            }
            SqlQuery += ") AS t1";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += "t1.RowNumber BETWEEN {0} AND {1}";
            SqlQuery += ";";
            return String.Format(SqlQuery, offset.ToString(), limit.ToString());
        }
        private string _sqlGetFilter(VM_NewsFilters filter)
        {
            string SqlConds = String.Empty;
            if (filter.IsActive != EnumBoolType.None)
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.News.FIELDS.IsActive;
                if (filter.IsActive == EnumBoolType.True)
                    SqlConds += " = 1";
                else
                    SqlConds += " = 0";
            }
            if (!String.IsNullOrEmpty(filter.Title))
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.News.FIELDS.Title;
                SqlConds += " " + DbStruct.SE.LIKE + " ";
                SqlConds += "'%" + DbStruct.SQLRealEscapeString(filter.Title) + "%'";
            }                     
            return String.IsNullOrEmpty(SqlConds) ? String.Empty : SqlConds.Substring(5);
        }        
        private string _sqlGetNewsList(int[] ids)
        {
            if (ids.Length == 0) return String.Empty;
            string strIds = String.Empty;
            foreach (int id in ids)
                strIds += "," + id.ToString();
            strIds = strIds.Substring(1);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.News.FIELDS.Id + ",";
            SqlQuery += DbStruct.News.FIELDS.PublishedAt + ",";            
            SqlQuery += DbStruct.News.FIELDS.Title + ",";
            SqlQuery += DbStruct.News.FIELDS.Alias + ",";
            SqlQuery += DbStruct.News.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.News.FIELDS.Author;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.News.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.News.FIELDS.Id;
            SqlQuery += " " + DbStruct.SE.IN + "(" + strIds + ")";
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.News.FIELDS.PublishedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetNews(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.News.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.News.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetNews(string alias)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.News.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.News.FIELDS.Alias;
            SqlQuery += " = '" + alias + "'";
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetNewsItem(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.News.FIELDS.Id + ",";
            SqlQuery += DbStruct.News.FIELDS.PublishedAt + ",";
            SqlQuery += DbStruct.News.FIELDS.Title + ",";
            SqlQuery += DbStruct.News.FIELDS.Alias + ",";
            SqlQuery += DbStruct.News.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.News.FIELDS.Author;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.News.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.News.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetNewsItems()
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.News.FIELDS.Id + ",";
            SqlQuery += DbStruct.News.FIELDS.PublishedAt + ",";            
            SqlQuery += DbStruct.News.FIELDS.Title + ",";
            SqlQuery += DbStruct.News.FIELDS.Alias + ",";
            SqlQuery += DbStruct.News.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.News.FIELDS.Author;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.News.TABLENAME;
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.News.FIELDS.Title;
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlCreateNews(VM_News art)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.INSERT + " ";
            SqlQuery += DbStruct.News.TABLENAME;
            SqlQuery += "(";            
            SqlQuery += DbStruct.News.FIELDS.CreatedAt + ",";
            SqlQuery += DbStruct.News.FIELDS.PublishedAt + ",";
            SqlQuery += DbStruct.News.FIELDS.Title + ",";
            SqlQuery += DbStruct.News.FIELDS.Alias + ",";
            SqlQuery += DbStruct.News.FIELDS.NewsText + ",";
            SqlQuery += DbStruct.News.FIELDS.MetaTitle + ",";
            SqlQuery += DbStruct.News.FIELDS.MetaDescription + ",";
            SqlQuery += DbStruct.News.FIELDS.MetaKeywords + ",";
            SqlQuery += DbStruct.News.FIELDS.MetaNofollow + ",";
            SqlQuery += DbStruct.News.FIELDS.MetaNoindex + ",";
            SqlQuery += DbStruct.News.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.News.FIELDS.SourceUrl + ",";
            SqlQuery += DbStruct.News.FIELDS.NewsUrl + ",";
            SqlQuery += DbStruct.News.FIELDS.PictureUrl + ",";
            SqlQuery += DbStruct.News.FIELDS.Author;
            SqlQuery += ")";
            SqlQuery += " " + DbStruct.SE.VALUES + "(";
            SqlQuery += DbStruct.SE.GETDATE + ",";
            SqlQuery += DbStruct.FormatSQLDateTime(art.PublishedAt, SQLDateTimeFormat.sdfDateTime) + ",";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.Title) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.Alias) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.NewsText) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.MetaTitle) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.MetaDesc) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.MetaKeys) + "',";
            SqlQuery += art.MetaNoFollow ? "1," : "0,";
            SqlQuery += art.MetaNoIndex ? "1," : "0,";
            SqlQuery += art.IsActive ? "1," : "0,";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.SourceUrl) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.NewsUrl) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.PictureUrl) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.Author) + "'";
            SqlQuery += ")";
            SqlQuery += ";";
            SqlQuery += DbStruct.SE.SELECT_SCOPE_IDENTITY;
            return SqlQuery;
        }
        private string _sqlUpdateNews(int id, VM_News art)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.News.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";            
            SqlQuery += DbStruct.News.FIELDS.Title + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.Title) + "',";
            SqlQuery += DbStruct.News.FIELDS.NewsText + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.NewsText) + "',";
            SqlQuery += DbStruct.News.FIELDS.NewsUrl + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.NewsUrl) + "',";
            SqlQuery += DbStruct.News.FIELDS.PictureUrl + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.PictureUrl) + "',";
            SqlQuery += DbStruct.News.FIELDS.MetaTitle + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.MetaTitle) + "',";
            SqlQuery += DbStruct.News.FIELDS.MetaDescription + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.MetaDesc) + "',";
            SqlQuery += DbStruct.News.FIELDS.MetaKeywords + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.MetaKeys) + "',";
            SqlQuery += DbStruct.News.FIELDS.MetaNofollow + " = ";
            SqlQuery += art.MetaNoFollow ? "1," : "0,";
            SqlQuery += DbStruct.News.FIELDS.MetaNoindex + " = ";
            SqlQuery += art.MetaNoIndex ? "1," : "0,";
            SqlQuery += DbStruct.News.FIELDS.IsActive + " = ";
            SqlQuery += art.IsActive ? "1" : "0";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.News.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlDeleteNews(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.DELETE + " ";
            SqlQuery += DbStruct.News.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.News.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }        
        private string _sqlSetNewsActive(int id, bool action)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.News.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";
            SqlQuery += DbStruct.News.FIELDS.IsActive + " = ";
            SqlQuery += action ? "1" : "0";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.News.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetAliasExists(string alias)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.News.FIELDS.Alias;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.News.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.News.FIELDS.Alias;
            SqlQuery += " = '" + DbStruct.SQLRealEscapeString(alias) + "'";
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetLastNews(int rowCount)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.TOP + " " + rowCount.ToString() + " ";
            SqlQuery += DbStruct.News.FIELDS.Id + ",";
            SqlQuery += DbStruct.News.FIELDS.Title + ",";
            SqlQuery += DbStruct.News.FIELDS.Alias + ",";
            SqlQuery += DbStruct.News.FIELDS.PublishedAt;            
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.News.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";            
            SqlQuery += DbStruct.News.FIELDS.IsActive;
            SqlQuery += " = 1";            
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.News.FIELDS.PublishedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetMaxDateTime(string author)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.MAX + "(";            
            SqlQuery += DbStruct.News.FIELDS.PublishedAt;
            SqlQuery += ")";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.News.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.News.FIELDS.Author;
            SqlQuery += " = '" + DbStruct.SQLRealEscapeString(author) + "'";            
            SqlQuery += ";";
            return SqlQuery;
        }
       
        //Дополнительно
        protected string GenerateAlias(string title)
        {
            string result = String.Empty;
            try
            {
                result = TransliterationManager.Front(title.Replace("- ", "-").Replace(" -", "-"));
                if (_dbAliasExists(result))
                {
                    string res = result;
                    int i = 1;
                    do
                    {
                        res = result + "-" + i.ToString();
                        i++;
                    }
                    while (_dbAliasExists(res));
                    result = res;
                }
                return result;
            }
            catch (Exception ex)
            {
                LastError = "Ошибка во время генерации ссылки! " + ex.ToString();
                return null;
            }
        }
        public DateTime GetMaxDateTime(string author)
        {
            string SQLQuery = _sqlGetMaxDateTime(author);            
            SqlCommand _command = null;
            SqlDataReader _reader = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    _reader = _command.ExecuteReader();
                    if (_reader == null)
                    {
                        LastError = "Ошибка во время загрузки максимальной даты новостей!\nСервер вернул ответ NULL.";
                        return DateTime.UtcNow.AddDays(-1);
                    }

                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            return _reader.GetDateTime(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки максимальной даты новостей!\n" + ex.ToString();
                    return DateTime.UtcNow.AddDays(-1);
                }
                finally
                {
                    if (_reader != null)
                    {
                        _reader.Close();
                        _reader.Dispose();
                    }
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
            return DateTime.UtcNow.AddDays(-1);
        }
        #endregion
    }
}