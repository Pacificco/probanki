using Bankiru.Models.DataBase;
using Bankiru.Models.Domain.Articles;
using Bankiru.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Comments
{
    public class CommentsManager
    {
        public string LastError = "";

        //Список комментариев
        public VM_Comments GetComments(VM_CommentsFilters filter, int page = 1)
        {
            VM_Comments _comments = new VM_Comments();
            _comments.Filters.Assign(filter);
            _comments.PagingInfo.SetData(page, _getCommentsTotalCount(filter));
            if (_comments.PagingInfo.TotalItems == -1) return null;
            _comments.PagingInfo.CurrentPage = page;
            _comments.Items = _getComments(filter, _comments.PagingInfo.GetNumberFrom(), _comments.PagingInfo.GetNumberTo());
            return _comments;
        }

        //Комментарий
        public VM_Comment GetComment(int id)
        {
            string SQLQuery = _sqlGetComment(id);
            VM_Comment comment = new VM_Comment();
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
                        LastError = "Ошибка во время загрузки комментария (id=\"" + id.ToString() + "\")!\nСервер вернул ответ NULL.";
                        return null;
                    }

                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            comment.Id= _reader.GetInt32(0);
                            comment.CommentText = _reader.GetString(1);
                            comment.UserId = _reader.GetInt32(2);
                            comment.UserName = _reader.GetString(3);
                            comment.ParentId = _reader.IsDBNull(4) ? null : (int?)_reader.GetInt32(4);
                            comment.CreatedAt = _reader.GetDateTime(5);
                            comment.Article.Id = _reader.GetInt32(6);
                            comment.IsActive = _reader.GetBoolean(7);                            
                            comment.LikeCount = _reader.GetInt32(8);
                            comment.DisLikeCount = _reader.GetInt32(9);
                            comment.Confirmed = _reader.GetInt32(10) == 1 ? EnumBoolType.True : EnumBoolType.False;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки комментария (id=\"" + id.ToString() + "\")!\n" + ex.ToString();
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
            return comment;
        }
        public VM_CommentItem GetCommentItem(int id)
        {
            string SQLQuery = _sqlGetCommentItem(id);
            VM_CommentItem comment = new VM_CommentItem();
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
                        LastError = "Ошибка во время загрузки комментария (id=\"" + id.ToString() + "\")!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            comment.Id = _reader.GetInt32(0);
                            comment.CommentText = _reader.GetString(1);
                            comment.CreatedAt = _reader.GetDateTime(2);
                            comment.Article.Id = _reader.GetInt32(3);
                            comment.IsActive = _reader.GetBoolean(4);
                            comment.LikeCount = _reader.GetInt32(5);
                            comment.DisLikeCount = _reader.GetInt32(6);
                            comment.Confirmed = _reader.GetInt32(7);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки комментария (id=\"" + id.ToString() + "\")!\n" + ex.ToString();
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
            return comment;
        }
        public List<VM_CommentItem> GetCommentItems()
        {
            string SQLQuery = _sqlGetCommentItems();
            List<VM_CommentItem> comments = new List<VM_CommentItem>();
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
                        VM_CommentItem c = null;
                        while (_reader.Read())
                        {
                            c = new VM_CommentItem();
                            c.Id = _reader.GetInt32(0);
                            c.CommentText = _reader.GetString(1);
                            c.CreatedAt = _reader.GetDateTime(2);
                            c.Article.Id = _reader.GetInt32(3);
                            c.IsActive = _reader.GetBoolean(4);
                            c.LikeCount = _reader.GetInt32(5);
                            c.DisLikeCount = _reader.GetInt32(6);
                            c.Confirmed = _reader.GetInt32(7);
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
        public bool CreateComment(ref VM_Comment _comment)
        {                                    
            //Формирование запроса
            string SQLQuery = _sqlCreateComment(_comment);
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
                    //Новый Id
                    _comment.Id = Convert.ToInt32(objId.ToString());
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
        public bool UpdateComment(int id, VM_Comment _comment)
        {
            //Формирование запроса
            string SQLQuery = _sqlUpdateComment(id, _comment);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время обновления комментария!\nСервер вернул ответ " + i.ToString() + ".";
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время обновления комментария!\n" + ex.ToString();
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
        public bool DeleteComment(int id)
        {
            string SQLQuery = _sqlDeleteComment(id);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время удаления комментария!\nСервер вернул ответ " + i.ToString() + ".";
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время удаления комментария!\n" + ex.ToString();
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
        public bool SetCommentActive(int id, bool action)
        {
            //Формирование запроса
            string SQLQuery = _sqlSetCommentActive(id, action);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время установки активности комментария!\nСервер вернул ответ " + i.ToString() + ".";
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время установки активности комментария!\n" + ex.ToString();
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
        public bool SetComfirmed(int id, bool action)
        {
            //Формирование запроса
            string SQLQuery = _sqlSetComfirmation(id, action);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время установки статуса подтверждения комментария!\nСервер вернул ответ " + i.ToString() + ".";
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время установки статуса подтверждения комментария!\n" + ex.ToString();
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

        #region ПРИВАТНАЯ ЧАСТЬ
        //Поиск комментариев
        private int _getCommentsTotalCount(VM_CommentsFilters filter)
        {
            string SQLQuery = _sqlGetCommentsTotalCount(filter);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object count = _command.ExecuteScalar();
                    if (count == null)
                    {
                        LastError = "Ошибка во время определения общего числа комментариев!\nСервер вернул ответ NULL.";
                        return -1;
                    }
                    return (int)count;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время определения общего числа комментариев!\n" + ex.ToString();
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
        private string _sqlGetCommentsTotalCount(VM_CommentsFilters filter)
        {
            string SqlConds = _sqlGetFilter(filter);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.COUNT + "(*)";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            if (!String.IsNullOrEmpty(SqlConds))
            {
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += SqlConds;
            }
            SqlQuery += ";";
            return SqlQuery;
        }
        private List<VM_CommentItem> _getComments(VM_CommentsFilters filter, int offset, int limit)
        {
            string SQLQuery = _sqlGetComments(filter, offset, limit);
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
                        LastError = "Ошибка во время загрузки комментариев!\nСервер вернул ответ NULL.";
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_CommentItem comment = null;
                        while (_reader.Read())
                        {
                            comment = new VM_CommentItem();
                            comment.Id = _reader.GetInt32(1);
                            comment.CommentText = _reader.GetString(2);
                            comment.CreatedAt = _reader.GetDateTime(3);
                            comment.Article.Id = _reader.GetInt32(4);
                            comment.IsActive = _reader.GetBoolean(5);            
                            comment.LikeCount = _reader.GetInt32(6);
                            comment.DisLikeCount = _reader.GetInt32(7);
                            comment.Confirmed = _reader.GetInt32(8);
                            comment.Author = _reader.GetString(9);
                            items.Add(comment);
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
                        }
                    }
                }
            }
            return items;
        }
        private List<VM_CommentItem> _getComments(int[] ids)
        {
            string SQLQuery = _sqlGetComments(ids);
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
                        LastError = "Ошибка во время загрузки комментариев!\nСервер вернул ответ NULL.";
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
                            comment.Article.Id = _reader.GetInt32(3);
                            comment.IsActive = _reader.GetBoolean(4);
                            comment.LikeCount = _reader.GetInt32(5);
                            comment.DisLikeCount = _reader.GetInt32(6);
                            comment.Confirmed = _reader.GetInt32(7);
                            items.Add(comment);
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
                            art.PublishedAt = _reader.GetDateTime(8);                            
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
        
        //Sql
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
            SqlQuery += DbStruct.Articles.FIELDS.PublishedAt;
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
        private string _sqlGetComments(VM_CommentsFilters filter, int offset, int limit)
        {
            string SqlConds = _sqlGetFilter(filter);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " t1.* " + DbStruct.SE.FROM + "(";
            SqlQuery += DbStruct.SE.SELECT + " ROW_NUMBER() OVER(";
            SqlQuery += DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Comments.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ") AS RowNumber, ";
            SqlQuery += DbStruct.Comments.FIELDS.Id + ",";            
            SqlQuery += DbStruct.Comments.FIELDS.Text + ",";
            SqlQuery += DbStruct.Comments.FIELDS.CreatedAt + ",";
            SqlQuery += DbStruct.Comments.FIELDS.ArticleId + ",";
            SqlQuery += DbStruct.Comments.FIELDS.IsActive + ",";            
            SqlQuery += DbStruct.Comments.FIELDS.LikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.DisLikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.Confirmed + ",";
            SqlQuery += DbStruct.Comments.FIELDS.UserName;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
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
        private string _sqlGetFilter(VM_CommentsFilters filter)
        {
            string SqlConds = String.Empty;
            if (filter.IsActive != EnumBoolType.None)
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.Comments.FIELDS.IsActive;
                if (filter.IsActive == EnumBoolType.True)
                    SqlConds += " = 1";
                else
                    SqlConds += " = 0";
            }
            if (!String.IsNullOrEmpty(filter.CommentText))
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.Comments.FIELDS.Text;
                SqlConds += " " + DbStruct.SE.LIKE + " ";
                SqlConds += "'%" + DbStruct.SQLRealEscapeString(filter.CommentText) + "%'";
            }
            if (filter.Confirmed != EnumBoolType.None)
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.Comments.FIELDS.Confirmed;
                if (filter.Confirmed == EnumBoolType.True)
                    SqlConds += " = 1";
                else
                    SqlConds += " = 0";
            }
            return String.IsNullOrEmpty(SqlConds) ? String.Empty : SqlConds.Substring(5);
        }        
        private string _sqlGetComments(int[] ids)
        {
            if (ids.Length == 0) return String.Empty;
            string strIds = String.Empty;
            foreach (int id in ids)
                strIds += "," + id.ToString();
            strIds = strIds.Substring(1);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Id + ",";
            SqlQuery += DbStruct.Comments.FIELDS.Text + ",";
            SqlQuery += DbStruct.Comments.FIELDS.CreatedAt + ",";
            SqlQuery += DbStruct.Comments.FIELDS.ArticleId + ",";
            SqlQuery += DbStruct.Comments.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Comments.FIELDS.LikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.DisLikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.Confirmed;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Id;
            SqlQuery += " " + DbStruct.SE.IN + "(" + strIds + ")";
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Comments.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetComment(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetCommentItem(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Id + ",";
            SqlQuery += DbStruct.Comments.FIELDS.Text + ",";
            SqlQuery += DbStruct.Comments.FIELDS.CreatedAt + ",";
            SqlQuery += DbStruct.Comments.FIELDS.ArticleId + ",";
            SqlQuery += DbStruct.Comments.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Comments.FIELDS.LikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.DisLikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.Confirmed;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetCommentItems()
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Id + ",";
            SqlQuery += DbStruct.Comments.FIELDS.Text + ",";
            SqlQuery += DbStruct.Comments.FIELDS.CreatedAt + ",";
            SqlQuery += DbStruct.Comments.FIELDS.ArticleId + ",";
            SqlQuery += DbStruct.Comments.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Comments.FIELDS.LikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.DisLikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.Confirmed;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Comments.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlCreateComment(VM_Comment _comment)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.INSERT + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += "(";
            SqlQuery += DbStruct.Comments.FIELDS.Text + ",";
            SqlQuery += DbStruct.Comments.FIELDS.UserId + ",";
            SqlQuery += DbStruct.Comments.FIELDS.UserName + ",";
            SqlQuery += DbStruct.Comments.FIELDS.ParentId + ",";
            SqlQuery += DbStruct.Comments.FIELDS.CreatedAt + ",";
            SqlQuery += DbStruct.Comments.FIELDS.ArticleId + ",";
            SqlQuery += DbStruct.Comments.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Comments.FIELDS.LikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.DisLikeCount + ",";
            SqlQuery += DbStruct.Comments.FIELDS.Confirmed;                       
            SqlQuery += ")";
            SqlQuery += " " + DbStruct.SE.VALUES + "(";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(_comment.CommentText) + "',";
            SqlQuery += _comment.UserId.ToString() + ",";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(_comment.UserName) + "',";
            SqlQuery += "'" + (_comment.ParentId == null ? DbStruct.SE.NULL : _comment.ParentId.ToString()) + ",";
            SqlQuery += DbStruct.SE.GETDATE + ",";
            SqlQuery += _comment.Article.Id.ToString() + ",";
            SqlQuery += _comment.IsActive ? "1," : "0,";
            SqlQuery += _comment.Confirmed == EnumBoolType.True ? "1," : "0,";
            SqlQuery += _comment.LikeCount.ToString() + ",";
            SqlQuery += _comment.DisLikeCount.ToString();
            SqlQuery += ")";
            SqlQuery += ";";
            SqlQuery += DbStruct.SE.SELECT_SCOPE_IDENTITY;
            return SqlQuery;
        }
        private string _sqlUpdateComment(int id, VM_Comment _comment)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Text + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(_comment.CommentText) + "',";
            //SqlQuery += DbStruct.Comments.FIELDS.UserId + " = ";
            //SqlQuery += _comment.UserId.ToString() + ",";
            //SqlQuery += DbStruct.Comments.FIELDS.UserName + " = ";
            //SqlQuery += "'" + DbStruct.SQLRealEscapeString(_comment.UserName) + "',";
            //SqlQuery += DbStruct.Comments.FIELDS.ParentId + " = ";
            //SqlQuery += "'" + (_comment.ParentId == null ? DbStruct.SE.NULL : _comment.ParentId.ToString()) + ",";
            //SqlQuery += DbStruct.Comments.FIELDS.CreatedAt + " = ";
            //SqlQuery += DbStruct.SE.GETDATE + ",";
            //SqlQuery += DbStruct.Comments.FIELDS.ArticleId + " = ";
            //SqlQuery += _comment.ArticleId.ToString() + ",";
            SqlQuery += DbStruct.Comments.FIELDS.IsActive + " = ";
            SqlQuery += _comment.IsActive ? "1," : "0,";
            SqlQuery += DbStruct.Comments.FIELDS.Confirmed + " = ";
            SqlQuery += _comment.Confirmed == EnumBoolType.True ? "1" : "0";
            //SqlQuery += DbStruct.Comments.FIELDS.LikeCount + " = ";
            //SqlQuery += _comment.LikeCount.ToString() + ",";
            //SqlQuery += DbStruct.Comments.FIELDS.DisLikeCount;
            //SqlQuery += _comment.DisLikeCount.ToString();            
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlDeleteComment(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.DELETE + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlSetCommentActive(int id, bool action)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";
            SqlQuery += DbStruct.Comments.FIELDS.IsActive + " = ";
            SqlQuery += action ? "1" : "0";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlSetComfirmation(int id, bool action)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.Comments.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Confirmed + " = ";
            SqlQuery += action ? "1" : "0";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Comments.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
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
        #endregion
    }
}