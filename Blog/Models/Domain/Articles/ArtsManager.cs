using Bankiru.Models.DataBase;
using Bankiru.Models.Domain.Categories;
using Bankiru.Models.Domain.Comments;
using Bankiru.Models.Domain.Other;
using Bankiru.Models.Helpers;
using Bankiru.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Articles
{
    public class ArtsManager
    {
        public string LastError = "";

        #region ПУБЛИКАЦИИ
        public VM_Articles GetArts(VM_ArtsFilters filter, int page = 1)
        {
            VM_Articles _arts = new VM_Articles();
            _arts.Filters.Assign(filter);
            _arts.PagingInfo.SetData(page, _getArtsTotalCount(filter));
            if (_arts.PagingInfo.TotalItems == -1) return null;
            _arts.PagingInfo.CurrentPage = page;
            _arts.Items = _getArts(filter, _arts.PagingInfo.GetNumberFrom(), _arts.PagingInfo.GetNumberTo());
            return _arts;
        }
        public VM_Articles GetArts(string catAlias, int page = 1)
        {
            VM_Articles _arts = new VM_Articles();
            int catId = _getCategoryIdByAlias(catAlias);
            _arts.PagingInfo.SetData(page, _getArtsTotalCount(catId));
            if (_arts.PagingInfo.TotalItems == -1) return null;
            _arts.PagingInfo.CurrentPage = page;
            _arts.Items = _getArts(catId, _arts.PagingInfo.GetNumberFrom(), _arts.PagingInfo.GetNumberTo());
            return _arts;
        }        
        public VM_Article GetArt(int id)
        {
            string SQLQuery = _sqlGetArt(id);
            VM_Article art = null;
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
                        LastError = "Ошибка во время загрузки статьи (id=\"" + id.ToString() + "\")!\nСервер вернул ответ NULL.";
                        return null;
                    }

                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            art = new VM_Article();
                            art.Id = _reader.GetInt32(0);
                            art.Title = _reader.GetString(1);
                            art.Alias = _reader.GetString(2);
                            art.SubTitle = _reader.GetString(3);
                            art.TextPrev = _reader.GetString(4);
                            art.CategoryId = _reader.GetInt32(5);
                            art.IsActive = _reader.GetBoolean(6);
                            art.MetaTitle = _reader.GetString(7);
                            art.MetaKeys = _reader.GetString(8);
                            art.MetaDesc = _reader.GetString(9);
                            art.MetaNoIndex = _reader.GetBoolean(10);
                            art.MetaNoFollow = _reader.GetBoolean(11);
                            art.UpdatedAt = _reader.GetDateTime(12);
                            art.CreatedAt = _reader.GetDateTime(13);
                            art.UserId = _reader.GetInt32(14);
                            art.TextFull = _reader.GetString(15);
                            art.OtherUser = _reader.IsDBNull(16) ? "" : _reader.GetString(16);
                            art.PublishedAt = _reader.GetDateTime(17);
                            art.Hits = _reader.GetInt32(18);
                            art.IsCentral = _reader.GetBoolean(19);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки статьи (id=\"" + id.ToString() + "\")!\n" + ex.ToString();
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
            if (art == null) return null;
            art.Category = GetCategory(art.CategoryId);
            Dictionary<int, VM_ArtCommentInfo> comments = _getArtsCommentsInfo(new int[] { art.Id });
            if (comments != null && comments.Count > 0)
            {
                art.CommentInfo = comments.First().Value;
            }
            return art;
        }
        public VM_Article GetArt(string alias)
        {
            string SQLQuery = _sqlGetArt(alias);
            VM_Article art = null;
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
                        LastError = "Ошибка во время загрузки публикации (alias=\"" + alias + "\")!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            art = new VM_Article();
                            art.Id = _reader.GetInt32(0);
                            art.Title = _reader.GetString(1);
                            art.Alias = _reader.GetString(2);
                            art.SubTitle = _reader.GetString(3);
                            art.TextPrev = _reader.GetString(4);
                            art.CategoryId = _reader.GetInt32(5);
                            art.IsActive = _reader.GetBoolean(6);
                            art.MetaTitle = _reader.GetString(7);
                            art.MetaKeys = _reader.GetString(8);
                            art.MetaDesc = _reader.GetString(9);
                            art.MetaNoIndex = _reader.GetBoolean(10);
                            art.MetaNoFollow = _reader.GetBoolean(11);
                            art.UpdatedAt = _reader.GetDateTime(12);
                            art.CreatedAt = _reader.GetDateTime(13);
                            art.UserId = _reader.GetInt32(14);
                            art.TextFull = _reader.GetString(15);
                            art.OtherUser = _reader.IsDBNull(16) ? "" : _reader.GetString(16);
                            art.PublishedAt = _reader.GetDateTime(17);
                            art.Hits = _reader.GetInt32(18);
                            art.IsCentral = _reader.GetBoolean(19);                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки публикации (alias=\"" + alias + "\")!\n" + ex.ToString();
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
            if (art == null) return null;
            art.Category = GetCategory(art.CategoryId);            
            Dictionary<int, VM_ArtCommentInfo> comments = _getArtsCommentsInfo(new int[] { art.Id });
            if (comments != null && comments.Count > 0)
            {
                art.CommentInfo = comments.First().Value;
            }
            return art;
        }
        public VM_ArtItem GetArtItem(int id)
        {
            string SQLQuery = _sqlGetArtItem(id);
            VM_ArtItem art = new VM_ArtItem();
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
                        LastError = "Ошибка во время загрузки статьи (id=\"" + id.ToString() + "\")!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            art.Id = _reader.GetInt32(0);
                            art.Alias = _reader.GetString(1);
                            art.Title = _reader.GetString(2);
                            art.CategoryId = _reader.GetInt32(3);
                            art.IsActive = _reader.GetBoolean(4);
                            art.Hits = _reader.GetInt32(5);
                            art.UserId = _reader.GetInt32(6);
                            art.OtherUser = _reader.IsDBNull(7) ? "" : _reader.GetString(7);
                            art.PublishedAt = _reader.GetDateTime(8);
                            art.CreatedAt = _reader.GetDateTime(9);

                            if (art.Category == null) art.Category = new VM_Category();
                            art.Category.Id = art.CategoryId;
                            art.Category.Title = _getCategoryNameById(art.CategoryId);
                            art.Category.Alias = _getCategoryAliasById(art.CategoryId);

                            art.CommentsInfo = new VM_ArtCommentInfo();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки статьи (id=\"" + id.ToString() + "\")!\n" + ex.ToString();
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
            return art;
        }
        public VM_ArtItem GetCentralArtItem()
        {
            string SQLQuery = _sqlGetCentralArticle();
            VM_ArtItem art = null;
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
                        LastError = "Ошибка во время загрузки центральной статьи!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            art = new VM_ArtItem();
                            art.Id = _reader.GetInt32(0);
                            art.Alias = _reader.GetString(1);
                            art.Title = _reader.GetString(2);
                            art.CategoryId = _reader.GetInt32(3);
                            art.IsActive = _reader.GetBoolean(4);
                            art.Hits = _reader.GetInt32(5);
                            art.UserId = _reader.GetInt32(6);
                            art.OtherUser = _reader.IsDBNull(7) ? "" : _reader.GetString(7);
                            art.PublishedAt = _reader.GetDateTime(8);
                            art.TextPrev = _reader.GetString(9);
                            art.CreatedAt = _reader.GetDateTime(10);
                            art.SubTitle = _reader.GetString(11);

                            if (art.Category == null) art.Category = new VM_Category();
                            art.Category.Id = art.CategoryId;
                            art.Category.Title = _getCategoryNameById(art.CategoryId);
                            art.Category.Alias = _getCategoryAliasById(art.CategoryId);

                            art.CommentsInfo = new VM_ArtCommentInfo();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки центральной статьи!\n" + ex.ToString();
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
            if (art != null && art.Id > 0)
            {
                Dictionary<int, VM_ArtCommentInfo> artComments = _getArtsCommentsInfo(new int[] { art.Id });
                if (artComments != null && artComments.Count > 0)
                {
                    art.CommentsInfo.Assign(artComments.First().Value);
                }
            }
            return art;
        }
        public List<VM_ArtItem> GetArtItems()
        {
            string SQLQuery = _sqlGetArtItems();
            List<VM_ArtItem> arts = new List<VM_ArtItem>();
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
                        LastError = "Ошибка во время загрузки статей!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        while (_reader.Read())
                        {
                            arts.Add(new VM_ArtItem()
                            {
                                Id = _reader.GetInt32(0),
                                Alias = _reader.GetString(1),
                                Title = _reader.GetString(2),
                                CategoryId = _reader.GetInt32(3),
                                IsActive = _reader.GetBoolean(4),
                                Hits = _reader.GetInt32(5),
                                UserId = _reader.GetInt32(6),
                                OtherUser = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                                PublishedAt = _reader.GetDateTime(8),
                                CreatedAt = _reader.GetDateTime(9)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки статей!\n" + ex.ToString();
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
            return arts;
        }
        public List<VM_ArtItem> GetArtItems(List<int> excludeIds, List<int> catIds, int rowCount = 5)
        {
            string SQLQuery = _sqlGetArtItems(excludeIds, catIds, rowCount);
            List<VM_ArtItem> arts = new List<VM_ArtItem>();
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
                        LastError = "Ошибка во время загрузки статей!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_ArtItem item = null;
                        while (_reader.Read())
                        {
                            item = new VM_ArtItem();
                            item.Id = _reader.GetInt32(0);
                            item.Alias = _reader.GetString(1);
                            item.Title = _reader.GetString(2);
                            item.CategoryId = _reader.GetInt32(3);
                            item.IsActive = _reader.GetBoolean(4);
                            item.Hits = _reader.GetInt32(5);
                            item.UserId = _reader.GetInt32(6);
                            item.OtherUser = _reader.IsDBNull(7) ? "" : _reader.GetString(7);
                            item.CreatedAt = _reader.GetDateTime(8);                            
                            item.Category.Id = item.CategoryId;
                            item.Category.Alias = _getCategoryAliasById(item.CategoryId);
                            item.Category.Title = _getCategoryNameById(item.CategoryId);
                            arts.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки статей!\n" + ex.ToString();
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
            return arts;
        }        
        public bool CreateArt(ref VM_Article art)
        {
            //Формирование ссылки
            string alias = GenerateAlias(art.Title);
            if (String.IsNullOrEmpty(alias)) return false;
            art.Alias = alias;
            art.UserId = 2;
            //art.PublishedAt = DateTime.Now;
            //Формирование запроса
            string SQLQuery = _sqlCreateArt(art);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object objId = _command.ExecuteScalar();
                    if (objId == null)
                    {
                        LastError = "Ошибка во время создания статьи!\nСервер вернул ответ NULL.";
                        return false;
                    }
                    //Новый Id
                    art.Id = Convert.ToInt32(objId.ToString());
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время создания статьи!\n" + ex.ToString();
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
        public bool UpdateArt(int id, VM_Article art)
        {            
            //Формирование запроса
            //art.PublishedAt = DateTime.Now;
            string SQLQuery = _sqlUpdateArt(id, art);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время обновления статьи!\nСервер вернул ответ " + i.ToString() + ".";
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время обновления статьи!\n" + ex.ToString();
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
        public bool DeleteArt(int id)
        {
            string SQLQuery = _sqlDeleteArt(id);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время удаления публикации!\nСервер вернул ответ " + i.ToString() + ".";
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время удаления публикации!\n" + ex.ToString();
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
        public bool SetArtActive(int id, bool action)
        {
            //Формирование запроса
            string SQLQuery = _sqlSetArtActive(id, action);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время установки активности статьи!\nСервер вернул ответ " + i.ToString() + ".";
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время установки активности статьи!\n" + ex.ToString();
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
        public bool SetArticleAsCentral(int id)
        {
            //Формирование запроса            
            if (DbStruct.BeginTransaction("SetArticleAsCentral"))
            {
                LastError = "Ошибка во время открытия транзакции";
                return false;
            }
            string SQLQuery = _sqlArticleAsCentral(id);
            bool success = true;            
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время установки центральной статьи!\nСервер вернул ответ " + i.ToString() + ".";
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время установки центральной статьи!\n" + ex.ToString();
                    success = false;
                }
                finally
                {
                    if (_command != null)
                    {
                        _command.Dispose();
                    }
                }
            }
            if (success)
            {
                if (!DbStruct.CommitTransaction("SetArticleAsCentral"))
                {
                    LastError = "Ошибка во время закрытия транзакции!";
                    DbStruct.RollBackTransaction("SetArticleAsCentral");
                    return false;
                }
                return true;
            }
            else
            {
                DbStruct.RollBackTransaction("SetArticleAsCentral");
                return false;
            }
        }
        #endregion

        #region КОММЕНТАРИИ
        public VM_Comments GetLastComments(int categoryId, int rowCount, bool show_article_link)
        {
            VM_Comments comments = new VM_Comments();
            comments.Filters = new VM_CommentsFilters();
            comments.Items = _getLastComments(categoryId, rowCount);
            comments.ShowArticleLink = show_article_link;
            return comments;
        }
        public VM_Comments GetLastComments(int art_id)
        {
            VM_Comments comments = new VM_Comments();
            comments.Filters = new VM_CommentsFilters();
            comments.Items = _getLastComments(art_id);
            comments.ShowArticleLink = false;
            return comments;
        }
        private List<VM_CommentItem> _getLastComments(int art_id)
        {
            string SQLQuery = _sqlGetLastComments(art_id);
            List<VM_CommentItem> items = new List<VM_CommentItem>();
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
                        LastError = "Ошибка во время загрузки комментариев к публикации!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_CommentItem comment = null;
                        while (_reader.Read())
                        {
                            comment = new VM_CommentItem();
                            comment.Id = _reader.GetInt32(0);
                            comment.CommentText = _reader.GetString(1);
                            comment.CreatedAt = _reader.GetDateTime(2);
                            comment.LikeCount = _reader.GetInt32(3);
                            comment.DisLikeCount = _reader.GetInt32(4);
                            comment.Article.Id = _reader.GetInt32(5);
                            comment.Author = _reader.GetString(6);
                            items.Add(comment);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки комментариев к публикации!\n" + ex.ToString();
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
            if (items.Count > 0)
            {
                int[] ids = (from i in items select i.Article.Id).Distinct().ToArray<int>();
                List<VM_ArtItem> artItems = _getArts(ids);
                if (artItems != null && artItems.Count > 0)
                {
                    VM_ArtItem art = null;
                    foreach (VM_CommentItem c in items)
                    {
                        art = artItems.FirstOrDefault(a => a.Id == c.Article.Id);
                        if (art != null)
                        {
                            c.Article.Alias = art.Alias;
                            c.Article.Title = art.Title;
                            c.Article.CategoryId = art.CategoryId;
                            c.Article.Category.Id = art.CategoryId;
                            c.Article.Category.Alias = _getCategoryAliasById(art.CategoryId);
                            c.Article.Category.Title = _getCategoryNameById(art.CategoryId);
                        }
                    }
                }
            }
            return items;
        }
        private List<VM_CommentItem> _getLastComments(int categoryId, int rowCount)
        {
            string SQLQuery = _sqlGetLastComments(categoryId, rowCount);
            List<VM_CommentItem> items = new List<VM_CommentItem>();
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
                        LastError = "Ошибка во время загрузки последних комментариев!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_CommentItem comment = null;
                        while (_reader.Read())
                        {
                            comment = new VM_CommentItem();
                            comment.Id = _reader.GetInt32(0);
                            comment.CommentText = _reader.GetString(1);
                            comment.CreatedAt = _reader.GetDateTime(2);
                            comment.LikeCount = _reader.GetInt32(3);
                            comment.DisLikeCount = _reader.GetInt32(4);
                            comment.Article.Id = _reader.GetInt32(5);
                            comment.Author = _reader.GetString(6);
                            items.Add(comment);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки последних комментариев!\n" + ex.ToString();
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
            if (items.Count > 0)
            {
                int[] ids = (from i in items select i.Article.Id).Distinct().ToArray<int>();
                List<VM_ArtItem> artItems = _getArts(ids);
                if (artItems != null && artItems.Count > 0)
                {
                    VM_ArtItem art = null;
                    foreach (VM_CommentItem c in items)
                    {
                        art = artItems.FirstOrDefault(a => a.Id == c.Article.Id);
                        if (art != null)
                        {
                            c.Article.Alias = art.Alias;
                            c.Article.Title = art.Title;
                            c.Article.CategoryId = art.CategoryId;
                            c.Article.Category.Id = art.CategoryId;
                            c.Article.Category.Alias = _getCategoryAliasById(art.CategoryId);
                            c.Article.Category.Title = _getCategoryNameById(art.CategoryId);
                        }
                    }
                }
            }
            return items;
        }
        public VM_ArtComments GetArtComments(string artAlias, int page)
        {
            VM_ArtComments artComments = new VM_ArtComments();
            artComments.Art = GetArt(artAlias);
            if (artComments.Art == null)
                return null;
            artComments.PagingInfo.ItemsPerPage = 10;
            artComments.PagingInfo.SetData(page, _getArtCommentsTotalCount(artComments.Art.Id));
            if (artComments.PagingInfo.TotalItems == -1) return null;
            artComments.PagingInfo.CurrentPage = page;
            artComments.Items = GetComments(artComments.Art.Id, artComments.PagingInfo.GetNumberFrom(), artComments.PagingInfo.GetNumberTo());
            return artComments;
        }
        public VM_ArtComment GetArtComment(string artAlias, int comment_id)
        {
            VM_ArtComment artComment = new VM_ArtComment();
            artComment.Art = GetArt(artAlias);
            if (artComment.Art == null)
                return null;
            artComment.Review = GetComment(comment_id);
            return artComment;
        }
        private int _getArtCommentsTotalCount(int art_id)
        {
            string SQLQuery = _sqlGetArtCommentsTotalCount(art_id);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object count = _command.ExecuteScalar();
                    if (count == null)
                    {
                        LastError = "Ошибка во время определения общего числа комментариев публикации!\nСервер вернул ответ NULL.";
                        return -1;
                    }
                    return (int)count;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время определения общего числа комментариев публикации!\n" + ex.ToString();
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
        public List<VM_Comment> GetComments(int artId, int offset, int limit)
        {
            string SQLQuery = _sqlGetComments(artId, offset, limit);
            List<VM_Comment> comments = new List<VM_Comment>();
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
                        LastError = "Ошибка во время загрузки комментариев!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_Comment c = null;
                        while (_reader.Read())
                        {
                            c = new VM_Comment();
                            c.Id = _reader.GetInt32(0);
                            c.CommentText = DbHelper.GetPartUseTags(_reader.GetString(1), 500);
                            c.UserId = _reader.GetInt32(2);
                            c.UserName = _reader.GetString(3);
                            c.ParentId = _reader.IsDBNull(4) ? null : (int?)_reader.GetInt32(4);
                            c.CreatedAt = _reader.GetDateTime(5);
                            if (c.Article != null)
                                c.Article = new VM_ArtItem();
                            c.Article.Id = _reader.GetInt32(6);
                            c.IsActive = _reader.GetBoolean(7);
                            c.LikeCount = _reader.GetInt32(8);
                            c.DisLikeCount = _reader.GetInt32(9);
                            c.Confirmed = _reader.GetInt32(10) == 1 ? EnumBoolType.True : EnumBoolType.False;
                            comments.Add(c);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки комментариев!\n" + ex.ToString();
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
            return comments;
        }
        public VM_Comment GetComment(int comment_id)
        {
            string SQLQuery = _sqlGetComment(comment_id);
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
                        LastError = String.Format("Ошибка во время загрузки комментария (id={0})!\nСервер вернул ответ NULL", comment_id);
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            VM_Comment c = new VM_Comment();
                            c.Id = _reader.GetInt32(0);
                            c.CommentText = DbHelper.GetPartUseTags(_reader.GetString(1), 500);
                            c.UserId = _reader.GetInt32(2);
                            c.UserName = _reader.GetString(3);
                            c.ParentId = _reader.IsDBNull(4) ? null : (int?)_reader.GetInt32(4);
                            c.CreatedAt = _reader.GetDateTime(5);
                            if (c.Article != null)
                                c.Article = new VM_ArtItem();
                            c.Article.Id = _reader.GetInt32(6);
                            c.IsActive = _reader.GetBoolean(7);
                            c.LikeCount = _reader.GetInt32(8);
                            c.DisLikeCount = _reader.GetInt32(9);
                            c.Confirmed = _reader.GetInt32(10) == 1 ? EnumBoolType.True : EnumBoolType.False;
                            return c;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = String.Format("Ошибка во время загрузки комментария (id={0})!\n{1}", comment_id, ex.ToString());
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
            return null;
        }
        public bool CreateComment(VM_CommentResponse commentResponse)
        {
            VM_Comment comment = new VM_Comment();
            comment.UserId = 2;
            comment.UserName = commentResponse.UserName;
            comment.IsActive = true;
            comment.Confirmed = EnumBoolType.False;
            //if(commentResponse.LikeDislike == 0)            
            //    comment.LikeCount = 1;
            //else
            //    comment.DisLikeCount = 1;
            comment.LikeCount = 0;
            comment.DisLikeCount = 0;
            comment.ParentId = null;
            comment.CommentText = commentResponse.CommentText;
            if (comment.Article == null)
                comment.Article = new VM_ArtItem();
            comment.Article.Id = commentResponse.ArtId;
            //Формирование запроса
            string SQLQuery = _sqlCreateComment(comment);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object objId = _command.ExecuteScalar();
                    if (objId == null)
                    {
                        LastError = "Ошибка во время создания комментария!\nСервер вернул ответ NULL.";
                        return false;
                    }                    
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время создания комментария!\n" + ex.ToString();
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
        #endregion

        #region КАТЕГОРИИ
        public List<VM_Category> GetCategories()
        {
            string SQLQuery = _sqlGetCategories();
            List<VM_Category> cats = new List<VM_Category>();
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
                        LastError = "Ошибка во время загрузки категорий статей!\nСервер вернул ответ NULL.";
                        return null;
                    }

                    if (_reader.HasRows)
                    {
                        while (_reader.Read())
                        {
                            cats.Add(new VM_Category()
                            {
                                Id = _reader.GetInt32(0),
                                Alias = _reader.GetString(1),
                                Title = _reader.GetString(2)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки категорий статей!\n" + ex.ToString();
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
            return cats;
        }
        public VM_Category GetCategory(string alias)
        {
            string SQLQuery = _sqlGetCategoryByAlias(alias);
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
                        LastError = String.Format("Ошибка во время загрузки категории публикаций ({0})!\nСервер вернул ответ NULL.", alias);
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            VM_Category cat = new VM_Category();
                            cat.Id = _reader.GetInt32(0);
                            cat.Alias = _reader.GetString(2);
                            cat.Title = _reader.GetString(1);
                            cat.Description = _reader.GetString(6);
                            cat.IsActive = _reader.GetBoolean(5);
                            return cat;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = String.Format("Ошибка во время загрузки категории публикаций ({0})!\n{1}", alias, ex.ToString());
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
            return null;
        }
        public VM_Category GetCategory(int cat_id)
        {
            string SQLQuery = _sqlGetCategoryById(cat_id);
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
                        LastError = String.Format("Ошибка во время загрузки категории публикаций ({0})!\nСервер вернул ответ NULL.", cat_id);
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            VM_Category cat = new VM_Category();
                            cat.Id = _reader.GetInt32(0);
                            cat.Title = _reader.GetString(1);
                            cat.Alias = _reader.GetString(2);
                            cat.IsActive = _reader.GetBoolean(5);
                            cat.Description = _reader.GetString(6);
                            return cat;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = String.Format("Ошибка во время загрузки категории публикаций ({0})!\n{1}", cat_id, ex.ToString());
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
            return null;
        }
        #endregion

        #region ВНУТРЕННИЕ МЕТОДЫ
        //Поиск
        private int _getArtsTotalCount(VM_ArtsFilters filter)
        {
            string SQLQuery = _sqlGetArtsTotalCount(filter);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object count = _command.ExecuteScalar();
                    if (count == null)
                    {
                        LastError = "Ошибка во время определения общего числа статей!\nСервер вернул ответ NULL.";
                        return -1;
                    }
                    return (int)count;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время определения общего числа статей!\n" + ex.ToString();
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
        private string _sqlGetArtsTotalCount(VM_ArtsFilters filter)
        {
            string SqlConds = _sqlGetFilter(filter);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.COUNT + "(*)";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            if (!String.IsNullOrEmpty(SqlConds))
            {
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += SqlConds;
            }
            SqlQuery += ";";
            return SqlQuery;
        }
        private int _getArtsTotalCount(int catId)
        {
            string SQLQuery = _sqlGetArtsTotalCount(catId);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object count = _command.ExecuteScalar();
                    if (count == null)
                    {
                        LastError = "Ошибка во время определения общего числа публикаций!\nСервер вернул ответ NULL.";
                        return -1;
                    }
                    return (int)count;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время определения общего числа публикаций!\n" + ex.ToString();
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
        private List<VM_ArtItem> _getArts(VM_ArtsFilters filter, int offset, int limit)
        {
            string SQLQuery = _sqlGetArts(filter, offset, limit);
            List<VM_ArtItem> items = new List<VM_ArtItem>();
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
                        LastError = "Ошибка во время загрузки статей!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_ArtItem art = null;
                        while (_reader.Read())
                        {
                            art = new VM_ArtItem();
                            art.Id = _reader.GetInt32(1);
                            art.Alias = _reader.GetString(2);
                            art.Title = _reader.GetString(3);
                            art.CategoryId = _reader.GetInt32(4);                            
                            art.IsActive = _reader.GetBoolean(5);
                            art.Hits = _reader.GetInt32(6);
                            art.UserId = _reader.GetInt32(7);
                            art.OtherUser = _reader.IsDBNull(8) ? "" : _reader.GetString(8);
                            art.PublishedAt = _reader.GetDateTime(9);
                            art.CreatedAt = _reader.GetDateTime(10);

                            if (art.Category == null) art.Category = new VM_Category();
                            art.Category.Id = art.CategoryId;
                            art.Category.Title = _getCategoryNameById(art.CategoryId);
                            art.Category.Alias = _getCategoryAliasById(art.CategoryId);
                            
                            art.CommentsInfo = new VM_ArtCommentInfo();

                            items.Add(art);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки статей!\n" + ex.ToString();
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
            if (items.Count > 0)
            {
                int[] artsIds = (from i in items                                   
                                   select i.Id).Distinct().ToArray<int>();
                if (artsIds != null && artsIds.Length > 0)
                {
                    Dictionary<int, VM_ArtCommentInfo> artComments = _getArtsCommentsInfo(artsIds);
                    if (artComments != null && artComments.Count > 0)
                    {
                        VM_ArtItem item = null;
                        foreach (var cInfo in artComments)
                        {
                            item = items.FirstOrDefault(i => i.Id == cInfo.Key);
                            if (item != null)
                                item.CommentsInfo.Assign(cInfo.Value);
                        }
                    }
                }
            }
            return items;
        }
        private List<VM_ArtItem> _getArts(int[] ids)
        {
            string SQLQuery = _sqlGetArts(ids);
            List<VM_ArtItem> items = new List<VM_ArtItem>();
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
                        LastError = "Ошибка во время загрузки статей!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_ArtItem art = null;
                        while (_reader.Read())
                        {
                            art = new VM_ArtItem();
                            art.Id = _reader.GetInt32(0);
                            art.Alias = _reader.GetString(1);
                            art.Title = _reader.GetString(2);
                            art.CategoryId = _reader.GetInt32(3);
                            art.IsActive = _reader.GetBoolean(4);
                            art.Hits = _reader.GetInt32(5);
                            art.UserId = _reader.GetInt32(6);
                            art.OtherUser = _reader.IsDBNull(7) ? "" : _reader.GetString(7);
                            //art.PublishedAt = _reader.GetDateTime(8);
                            art.CreatedAt = _reader.GetDateTime(8);
                            items.Add(art);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки статей!\n" + ex.ToString();
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
        private List<VM_ArtItem> _getArts(int catId, int offset, int limit)
        {
            string SQLQuery = _sqlGetArts(catId, offset, limit);
            List<VM_ArtItem> items = new List<VM_ArtItem>();
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
                        LastError = "Ошибка во время загрузки публикаций!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_ArtItem item = null;
                        while (_reader.Read())
                        {
                            item = new VM_ArtItem();
                            item.Id = _reader.GetInt32(1);
                            item.Alias = _reader.GetString(2);
                            item.Title = _reader.GetString(3);
                            item.TextPrev = _reader.GetString(4);
                            item.CreatedAt = _reader.GetDateTime(5);
                            item.Hits = _reader.GetInt32(6);
                            if (item.Category == null)
                                item.Category = new VM_Category();
                            item.CategoryId = _reader.GetInt32(7);
                            item.SubTitle = _reader.GetString(8);

                            item.Category.Id = _reader.GetInt32(7);
                            item.Category.Alias = _getCategoryAliasById(_reader.GetInt32(7));
                            item.Category.Title = _getCategoryNameById(_reader.GetInt32(7));

                            items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки публикаций!\n" + ex.ToString();
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
            if (items.Count > 0)
            {
                int[] ids = (from i in items
                             select i.Id).Distinct().ToArray<int>();
                if (ids != null && ids.Length > 0)
                {                    
                    Dictionary<int, VM_ArtCommentInfo> comments = _getArtsCommentsInfo(ids);
                    if (comments != null && comments.Count > 0)
                    {
                        VM_ArtItem item = null;
                        foreach (var r in comments)
                        {
                            item = items.FirstOrDefault(i => i.Id == r.Key);
                            if (item != null)
                            {
                                item.CommentsInfo.CommentsCount = r.Value.CommentsCount;
                                item.CommentsInfo.LikesCount = r.Value.LikesCount;
                                item.CommentsInfo.DisLikesCount = r.Value.DisLikesCount;
                            }
                        }
                    }
                }
            }
            return items;
        }
        private Dictionary<int, VM_ArtCommentInfo> _getArtsCommentsInfo(int[] ids)
        {
            Dictionary<int, VM_ArtCommentInfo> result = new Dictionary<int, VM_ArtCommentInfo>();
            string SQLQuery = _sqlGetArtsCommentsInfo(ids);
            SqlCommand _command = null;
            SqlDataReader _reader = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    _reader = _command.ExecuteReader();
                    if (_reader != null && _reader.HasRows)
                    {
                        while (_reader.Read())
                        {
                            result.Add(
                                _reader.GetInt32(0), 
                                new VM_ArtCommentInfo() 
                                {
                                    CommentsCount = _reader.GetInt32(1),
                                    LikesCount = _reader.GetInt32(2),
                                    DisLikesCount = _reader.GetInt32(3)
                                });    
                        }
                        
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время получения информации о комментариях для статей!\n" + ex.ToString();
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

        #region ЗАПРОСЫ
        //Публикации
        private string _sqlGetArts(VM_ArtsFilters filter, int offset, int limit)
        {
            string SqlConds = _sqlGetFilter(filter);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " t1.* " + DbStruct.SE.FROM + "(";
            SqlQuery += DbStruct.SE.SELECT + " ROW_NUMBER() OVER(";
            SqlQuery += DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Articles.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ") AS RowNumber, ";
            SqlQuery += DbStruct.Articles.FIELDS.Id + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Title + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CategoryId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.IsActive + ",";            
            SqlQuery += DbStruct.Articles.FIELDS.Hits + ",";
            SqlQuery += DbStruct.Articles.FIELDS.UserId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.OtherUser + ",";
            SqlQuery += DbStruct.Articles.FIELDS.PublishedAt + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
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
        private string _sqlGetArts(int catId, int offset, int limit)
        {
            string SqlCond = String.Empty;
            SqlCond += " " + DbStruct.SE.AND + " ";
            SqlCond += DbStruct.Articles.FIELDS.IsActive;
            SqlCond += " = 1";
            if (catId != -1)
            {
                SqlCond += " " + DbStruct.SE.AND + " ";
                SqlCond += DbStruct.Articles.FIELDS.CategoryId;
                SqlCond += " = " + catId.ToString();
            }

            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " t1.* " + DbStruct.SE.FROM + "(";
            SqlQuery += DbStruct.SE.SELECT + " ROW_NUMBER() OVER(";
            SqlQuery += DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Articles.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ") AS RowNumber, ";
            SqlQuery += DbStruct.Articles.FIELDS.Id + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Title + ",";
            SqlQuery += DbStruct.Articles.FIELDS.TextPrev + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CreatedAt + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Hits + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CategoryId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.SubTitle;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            if (!String.IsNullOrEmpty(SqlCond))
            {
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += SqlCond.Substring(5);
            }
            SqlQuery += ") AS t1";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += "t1.RowNumber BETWEEN {0} AND {1}";
            SqlQuery += ";";
            return String.Format(SqlQuery, offset.ToString(), limit.ToString());
        }
        private string _sqlGetFilter(VM_ArtsFilters filter)
        {
            string SqlConds = String.Empty;
            if (filter.IsActive != EnumBoolType.None)
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.Articles.FIELDS.IsActive;
                if (filter.IsActive == EnumBoolType.True)
                    SqlConds += " = 1";
                else
                    SqlConds += " = 0";
            }
            if (!String.IsNullOrEmpty(filter.Title))
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.Articles.FIELDS.Title;
                SqlConds += " " + DbStruct.SE.LIKE + " ";
                SqlConds += "'%" + DbStruct.SQLRealEscapeString(filter.Title) + "%'";
            }
            if (filter.CategoryId > 0)
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.Articles.FIELDS.CategoryId;
                SqlConds += " = " + filter.CategoryId.ToString();
            }            
            return String.IsNullOrEmpty(SqlConds) ? String.Empty : SqlConds.Substring(5);
        }
        private string _sqlGetArtsTotalCount(int catId)
        {
            string SqlCond = String.Empty;
            SqlCond += " " + DbStruct.SE.AND + " ";
            SqlCond += DbStruct.Articles.FIELDS.IsActive;
            SqlCond += " = 1";
            if (catId != -1)
            {
                SqlCond += " " + DbStruct.SE.AND + " ";
                SqlCond += DbStruct.Articles.FIELDS.CategoryId;
                SqlCond += " = " + catId.ToString();
            }            
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.COUNT + "(*)";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            if (!String.IsNullOrEmpty(SqlCond))
            {
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += SqlCond.Substring(5);
            }
            SqlQuery += ";";
            return SqlQuery;
        }        
        private string _sqlGetArts(int[] ids)
        {
            if (ids.Length == 0) return String.Empty;
            string strIds = String.Empty;
            foreach (int id in ids)
                strIds += "," + id.ToString();
            strIds = strIds.Substring(1);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Id + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Title + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CategoryId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Hits + ",";
            SqlQuery += DbStruct.Articles.FIELDS.UserId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.OtherUser + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Id;
            SqlQuery += " " + DbStruct.SE.IN + "(" + strIds + ")";
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Articles.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetArt(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetArt(string alias)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Alias;
            SqlQuery += " = '" + alias + "'";
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.Articles.FIELDS.IsActive;
            SqlQuery += " = 1";
            SqlQuery += ";";            
            return SqlQuery;
        }
        private string _sqlGetArtItem(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Id + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Title + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CategoryId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Hits + ",";
            SqlQuery += DbStruct.Articles.FIELDS.UserId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.OtherUser + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetCentralArticle()
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Id + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Title + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CategoryId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Hits + ",";
            SqlQuery += DbStruct.Articles.FIELDS.UserId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.OtherUser + ",";
            SqlQuery += DbStruct.Articles.FIELDS.PublishedAt + ",";
            SqlQuery += DbStruct.Articles.FIELDS.TextPrev + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CreatedAt + ",";
            SqlQuery += DbStruct.Articles.FIELDS.SubTitle;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Articles.FIELDS.IsCentral;
            SqlQuery += " = 1";
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetArtItems()
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Id + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Title + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CategoryId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Hits + ",";
            SqlQuery += DbStruct.Articles.FIELDS.UserId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.OtherUser + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Articles.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetArtItems(List<int> excludeIds, List<int> catIds, int rowCount)
        {
            string sqlFilter = String.Empty;
            if (excludeIds != null && excludeIds.Count > 0)
            {
                string ids = String.Empty;
                foreach (int id in excludeIds)
                    ids += "," + id.ToString();
                sqlFilter += " " + DbStruct.SE.AND + " ";
                sqlFilter += DbStruct.Articles.FIELDS.Id;
                sqlFilter += " " + DbStruct.SE.NOT_IN + " (";
                sqlFilter += ids.Substring(1);
                sqlFilter += ")";
            }
            if (catIds != null && catIds.Count > 0)
            {
                string ids = String.Empty;
                foreach (int id in catIds)
                    ids += "," + id.ToString();
                ids = ids.Substring(1);
                sqlFilter += " " + DbStruct.SE.AND + " ";
                sqlFilter += DbStruct.Articles.FIELDS.CategoryId;
                sqlFilter += " " + DbStruct.SE.IN + "(" + ids + ")";                
            }
            sqlFilter += " " + DbStruct.SE.AND + " ";
            sqlFilter += DbStruct.Articles.FIELDS.IsActive + " = 1";
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.TOP + " " + rowCount.ToString() + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Id + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Title + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CategoryId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Hits + ",";
            SqlQuery += DbStruct.Articles.FIELDS.UserId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.OtherUser + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            if (!String.IsNullOrEmpty(sqlFilter))
            {
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += sqlFilter.Substring(5);
            }
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Articles.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.DESC + " ";
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlCreateArt(VM_Article art)
        {
            string SqlQuery = String.Empty;
            //Если центральная статья
            if (art.IsCentral)
            {
                SqlQuery += DbStruct.SE.UPDATE + " ";
                SqlQuery += DbStruct.Articles.TABLENAME;
                SqlQuery += " " + DbStruct.SE.SET + " ";
                SqlQuery += DbStruct.Articles.FIELDS.IsCentral;
                SqlQuery += " = 0";
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += DbStruct.Articles.FIELDS.IsCentral;
                SqlQuery += " = 1";
                SqlQuery += ";";
            }
            //Создание новой публикации
            SqlQuery += DbStruct.SE.INSERT + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            SqlQuery += "(";
            //SqlQuery += DbStruct.Articles.FIELDS.Id + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Title + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Articles.FIELDS.SubTitle + ",";
            SqlQuery += DbStruct.Articles.FIELDS.TextPrev + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CategoryId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Articles.FIELDS.IsCentral + ",";
            SqlQuery += DbStruct.Articles.FIELDS.MetaTitle + ",";
            SqlQuery += DbStruct.Articles.FIELDS.MetaKeys + ",";
            SqlQuery += DbStruct.Articles.FIELDS.MetaDesc + ",";
            SqlQuery += DbStruct.Articles.FIELDS.MetaNoIndex + ",";
            SqlQuery += DbStruct.Articles.FIELDS.MetaNoFollow + ",";
            SqlQuery += DbStruct.Articles.FIELDS.ChangedAt + ",";
            SqlQuery += DbStruct.Articles.FIELDS.CreatedAt + ",";
            SqlQuery += DbStruct.Articles.FIELDS.UserId + ",";
            SqlQuery += DbStruct.Articles.FIELDS.TextFull + ",";
            SqlQuery += DbStruct.Articles.FIELDS.OtherUser + ",";
            SqlQuery += DbStruct.Articles.FIELDS.PublishedAt + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Hits;
            SqlQuery += ")";
            SqlQuery += " " + DbStruct.SE.VALUES + "(";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.Title) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.Alias) + "',";            
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.SubTitle) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.TextPrev) + "',";
            SqlQuery += art.CategoryId.ToString() + ",";
            SqlQuery += art.IsActive ? "1," : "0,";
            SqlQuery += art.IsCentral ? "1," : "0,";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.MetaTitle) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.MetaKeys) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.MetaDesc) + "',";
            SqlQuery += art.MetaNoFollow ? "1," : "0,";
            SqlQuery += art.MetaNoIndex ? "1," : "0,";
            SqlQuery += DbStruct.SE.GETDATE + ",";
            SqlQuery += DbStruct.SE.GETDATE + ",";
            SqlQuery += art.UserId.ToString() + ",";            
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.TextFull) + "',";            
            SqlQuery += (String.IsNullOrEmpty(art.OtherUser) ? DbStruct.SE.NULL :
                "'" + DbStruct.SQLRealEscapeString(art.OtherUser) + "'") + ",";
            SqlQuery += DbStruct.FormatSQLDateTime(art.PublishedAt, SQLDateTimeFormat.sdfDateTime) + ",";
            SqlQuery += art.Hits.ToString();            
            SqlQuery += ")";
            SqlQuery += ";";
            SqlQuery += DbStruct.SE.SELECT_SCOPE_IDENTITY;
            return SqlQuery;
        }
        private string _sqlUpdateArt(int id, VM_Article art)
        {
            string SqlQuery = String.Empty;
            //Если центральная статья
            if (art.IsCentral)
            {
                SqlQuery += DbStruct.SE.UPDATE + " ";
                SqlQuery += DbStruct.Articles.TABLENAME;
                SqlQuery += " " + DbStruct.SE.SET + " ";
                SqlQuery += DbStruct.Articles.FIELDS.IsCentral;
                SqlQuery += " = 0";
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += DbStruct.Articles.FIELDS.IsCentral;
                SqlQuery += " = 1";
                SqlQuery += ";";
            }
            //Обновление публикации
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";            
            SqlQuery += DbStruct.Articles.FIELDS.Title + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.Title) + "',";            
            SqlQuery += DbStruct.Articles.FIELDS.SubTitle + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.SubTitle) + "',";
            SqlQuery += DbStruct.Articles.FIELDS.TextPrev + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.TextPrev) + "',";
            SqlQuery += DbStruct.Articles.FIELDS.CategoryId + " = ";
            SqlQuery += art.CategoryId.ToString() + ",";
            SqlQuery += DbStruct.Articles.FIELDS.IsActive + " = ";
            SqlQuery += art.IsActive ? "1," : "0,";
            SqlQuery += DbStruct.Articles.FIELDS.IsCentral + " = ";
            SqlQuery += art.IsCentral ? "1," : "0,";
            SqlQuery += DbStruct.Articles.FIELDS.MetaTitle + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.MetaTitle) + "',";
            SqlQuery += DbStruct.Articles.FIELDS.MetaKeys + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.MetaKeys) + "',";
            SqlQuery += DbStruct.Articles.FIELDS.MetaDesc + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.MetaDesc) + "',";
            SqlQuery += DbStruct.Articles.FIELDS.MetaNoIndex + " = ";
            SqlQuery += art.MetaNoFollow ? "1," : "0,";
            SqlQuery += DbStruct.Articles.FIELDS.MetaNoFollow + " = ";
            SqlQuery += art.MetaNoIndex ? "1," : "0,";
            SqlQuery += DbStruct.Articles.FIELDS.ChangedAt + " = ";
            SqlQuery += DbStruct.SE.GETDATE + ",";
            //SqlQuery += DbStruct.Articles.FIELDS.PublishedAt + " = ";
            //SqlQuery += art.UserId.ToString() + ",";
            SqlQuery += DbStruct.Articles.FIELDS.TextFull + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(art.TextFull) + "',";
            SqlQuery += DbStruct.Articles.FIELDS.OtherUser + " = ";
            SqlQuery += (String.IsNullOrEmpty(art.OtherUser) ? DbStruct.SE.NULL :
                "'" + DbStruct.SQLRealEscapeString(art.OtherUser) + "'") + ",";
            SqlQuery += DbStruct.Articles.FIELDS.PublishedAt + " = ";
            SqlQuery += DbStruct.FormatSQLDateTime(art.PublishedAt, SQLDateTimeFormat.sdfDateTime) + ",";
            SqlQuery += DbStruct.Articles.FIELDS.Hits + " = ";
            SqlQuery += art.Hits.ToString();            
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlDeleteArt(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.DELETE + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Comments.FIELDS.ArticleId;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            SqlQuery += DbStruct.SE.DELETE + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }        
        private string _sqlSetArtActive(int id, bool action)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";
            SqlQuery += DbStruct.Articles.FIELDS.IsActive + " = ";
            SqlQuery += action ? "1" : "0";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlArticleAsCentral(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";
            SqlQuery += DbStruct.Articles.FIELDS.IsCentral + " = 0";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Articles.FIELDS.IsCentral + " = 1";
            SqlQuery += ";";
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";
            SqlQuery += DbStruct.Articles.FIELDS.IsCentral + " = 1";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetAliasExists(string alias)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Alias;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Articles.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Articles.FIELDS.Alias;
            SqlQuery += " = '" + DbStruct.SQLRealEscapeString(alias) + "'";
            SqlQuery += ";";
            return SqlQuery;
        }
        //Комментарии
        private string _sqlGetLastComments(int art_id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";            
            SqlQuery += DbStruct.Comments.FIELDS.Id + ",";
            SqlQuery += DbStruct.Comments.FIELDS.Text + ",";
            SqlQuery += DbStruct.Comments.FIELDS.CreatedAt + ",";
            SqlQuery += DbStruct.Comments.FIELDS.LikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.DisLikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.ArticleId + ",";
            SqlQuery += DbStruct.Comments.FIELDS.UserName;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Confirmed;
            SqlQuery += " = 1";
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.Comments.FIELDS.IsActive;
            SqlQuery += " = 1";
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.Comments.FIELDS.ArticleId;
            SqlQuery += " = " + art_id.ToString();
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Comments.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetLastComments(int categoryId, int rowCount)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.TOP + " " + rowCount.ToString() + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Id + ",";
            SqlQuery += DbStruct.Comments.FIELDS.Text + ",";
            SqlQuery += DbStruct.Comments.FIELDS.CreatedAt + ",";
            SqlQuery += DbStruct.Comments.FIELDS.LikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.DisLikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.ArticleId + ",";
            SqlQuery += DbStruct.Comments.FIELDS.UserName;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Confirmed;
            SqlQuery += " = 1";
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.Comments.FIELDS.IsActive;
            SqlQuery += " = 1";
            if (categoryId > 0)
            {
                SqlQuery += " " + DbStruct.SE.AND + " ";
                SqlQuery += DbStruct.Comments.FIELDS.ArticleId;
                SqlQuery += " " + DbStruct.SE.IN + " (";
                SqlQuery += DbStruct.SE.SELECT + " ";                
                SqlQuery += DbStruct.Articles.FIELDS.Id;
                SqlQuery += " " + DbStruct.SE.FROM + " ";
                SqlQuery += DbStruct.Articles.TABLENAME;
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += DbStruct.Articles.FIELDS.IsActive;
                SqlQuery += " = 1";
                SqlQuery += " " + DbStruct.SE.AND + " ";
                SqlQuery += DbStruct.Articles.FIELDS.CategoryId;
                SqlQuery += categoryId.ToString();
                SqlQuery += ")";
            }
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Comments.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetArtsCommentsInfo(int[] ids)
        {
            if (ids.Length == 0) return String.Empty;
            string strIds = String.Empty;
            foreach (int id in ids)
                strIds += "," + id.ToString();
            strIds = strIds.Substring(1);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Articles.ALIAS + "." + DbStruct.Articles.FIELDS.Id + ",";
            SqlQuery += DbStruct.SE.COUNT + "(";
            SqlQuery += DbStruct.Comments.ALIAS + "." + DbStruct.Comments.FIELDS.Id;
            SqlQuery += "),";
            SqlQuery += DbStruct.SE.SUM + "(";
            SqlQuery += DbStruct.Comments.ALIAS + "." + DbStruct.Comments.FIELDS.LikeCount;
            SqlQuery += "),";
            SqlQuery += DbStruct.SE.SUM + "(";
            SqlQuery += DbStruct.Comments.ALIAS + "." + DbStruct.Comments.FIELDS.DisLikeCount;
            SqlQuery += ")";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Articles.TABLENAME + " " + DbStruct.Articles.ALIAS;
            SqlQuery += " " + DbStruct.SE.JOIN + " ";
            SqlQuery += DbStruct.Comments.TABLENAME + " " + DbStruct.Comments.ALIAS;
            SqlQuery += " " + DbStruct.SE.ON + " ";
            SqlQuery += DbStruct.Articles.ALIAS + "." + DbStruct.Articles.FIELDS.Id;
            SqlQuery += " = ";
            SqlQuery += DbStruct.Comments.ALIAS + "." + DbStruct.Comments.FIELDS.ArticleId;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Articles.ALIAS + "." + DbStruct.Articles.FIELDS.Id;
            SqlQuery += " " + DbStruct.SE.IN + "(" + strIds + ")";
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.Comments.ALIAS + "." + DbStruct.Comments.FIELDS.IsActive + " = 1";
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.Comments.ALIAS + "." + DbStruct.Comments.FIELDS.Confirmed + " = 1";
            SqlQuery += " " + DbStruct.SE.GROUP_BY + " ";
            SqlQuery += DbStruct.Articles.ALIAS + "." + DbStruct.Articles.FIELDS.Id;
            SqlQuery += ";";
            return SqlQuery;
        }        
        private string _sqlGetArtCommentsTotalCount(int art_id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.COUNT + "(*)";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Comments.FIELDS.ArticleId;
            SqlQuery += " = " + art_id.ToString();
            //SqlQuery += " " + DbStruct.SE.AND + " ";
            //SqlQuery += DbStruct.Comments.FIELDS.IsActive + " = 1";
            //SqlQuery += " " + DbStruct.SE.AND + " ";
            //SqlQuery += DbStruct.Comments.FIELDS.Confirmed + " = 1";
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetComments(int art_id, int offset, int limit)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " t1.* " + DbStruct.SE.FROM + "(";
            SqlQuery += DbStruct.SE.SELECT + " ROW_NUMBER() OVER(";
            SqlQuery += DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Comments.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ") AS RowNumber, *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Comments.FIELDS.ArticleId;
            SqlQuery += " = " + art_id.ToString();
            SqlQuery += ") AS t1";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += "t1.RowNumber BETWEEN {0} AND {1}";
            SqlQuery += ";";
            return String.Format(SqlQuery, offset.ToString(), limit.ToString());
        }
        private string _sqlGetComment(int comment_id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Id;
            SqlQuery += " = " + comment_id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlCreateComment(VM_Comment comment)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.INSERT + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += "(";
            SqlQuery += DbStruct.Comments.FIELDS.ArticleId + ",";
            SqlQuery += DbStruct.Comments.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Comments.FIELDS.ParentId + ",";
            SqlQuery += DbStruct.Comments.FIELDS.Text + ",";
            SqlQuery += DbStruct.Comments.FIELDS.UserId + ",";
            SqlQuery += DbStruct.Comments.FIELDS.UserName + ",";
            SqlQuery += DbStruct.Comments.FIELDS.LikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.DisLikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.Confirmed + ",";
            SqlQuery += DbStruct.Comments.FIELDS.CreatedAt;
            SqlQuery += ")";
            SqlQuery += " " + DbStruct.SE.VALUES + "(";
            SqlQuery += comment.Article.Id.ToString() + ",";
            SqlQuery += comment.IsActive ? "1," : "0,";
            SqlQuery += (comment.ParentId == null ? DbStruct.SE.NULL : comment.ParentId.ToString()) + ",";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(comment.CommentText) + "',";
            SqlQuery += comment.UserId.ToString() + ",";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(comment.UserName) + "',";
            SqlQuery += comment.LikeCount.ToString() + ",";
            SqlQuery += comment.DisLikeCount.ToString() + ",";
            SqlQuery += comment.Confirmed == EnumBoolType.True ? "1," : "0,";
            SqlQuery += DbStruct.SE.GETDATE;
            SqlQuery += ")";
            SqlQuery += ";";
            SqlQuery += DbStruct.SE.SELECT_SCOPE_IDENTITY;
            return SqlQuery;
        }
        //Категории
        private string _sqlGetCategories()
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Categories.FIELDS.Id + ",";
            SqlQuery += DbStruct.Categories.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Categories.FIELDS.Title;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Categories.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Categories.FIELDS.IsActive + " = 1";
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Categories.FIELDS.Id;
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetCategoryByAlias(string alias)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Categories.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Categories.FIELDS.Alias;
            SqlQuery += " = '" + alias + "'";
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetCategoryById(int catId)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Categories.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Categories.FIELDS.Id;
            SqlQuery += " = " + catId.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private int _getCategoryIdByAlias(string alias)
        {
            switch (alias)
            {
                case "statii":
                    return 1;
                case "intervju":
                    return 2;
                case "mnenija":                    
                    return 3;
                case "lirika":
                    return 7;
                case "revizija":
                    return 8;
                default:
                    return -1;
            }
        }
        private string _getCategoryAliasById(int id)
        {
            switch (id)
            {
                case 1: return "statii";
                case 2: return "intervju";
                case 3: return "mnenija";
                case 7: return "lirika";
                case 8: return "revizija";
                default:
                    return "";
            }
        }
        private string _getCategoryNameById(int id)
        {
            switch (id)
            {
                case 1: return "Статьи";
                case 2: return "Интервью";
                case 3: return "Мнения";
                case 7: return "Лирика";
                case 8: return "Ревизия";
                default:
                    return "";
            }
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
        protected string _getCategoryName(int categoryId)
        {
            switch (categoryId)
            { 
                case 1:
                    return "Статьи";
                case 2:
                    return "Интервью";
                case 3:
                    return "Мнения";
                case 7:
                    return "Лирика";
                case 8:
                    return "Ревизия";
                default:
                    return "?";
            }
        }
        #endregion

        #endregion
    }
}