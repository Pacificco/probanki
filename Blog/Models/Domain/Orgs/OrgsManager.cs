using Bankiru.Models.DataBase;
using Bankiru.Models.Domain.OrgsCategories;
using Bankiru.Models.Domain.Other;
using Bankiru.Models.Helpers;
using Bankiru.Models.Infrastructure;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Orgs
{
    public class OrgsManager
    {
        private string _imagePath = "/Images/Orgs/Icons";
        public string LastError = "";
        public static readonly ILog log = LogManager.GetLogger(typeof(OrgsManager));

        #region ОРГАНИЗАЦИИ
        //Организации
        public VM_Orgs GetOrgs(VM_OrgsFilters filter, int page = 1)
        {
            VM_Orgs _orgs = new VM_Orgs();
            _orgs.Filters.Assign(filter);
            _orgs.PagingInfo.SetData(page, _getOrgsTotalCount(filter));
            if (_orgs.PagingInfo.TotalItems == -1) return null;
            _orgs.PagingInfo.CurrentPage = page;
            _orgs.Items = _getOrgs(filter, _orgs.PagingInfo.GetNumberFrom(), _orgs.PagingInfo.GetNumberTo());
            return _orgs;
        }
        public VM_Orgs GetOrgs(string catAlias, string letter, int page = 1)
        {
            VM_Orgs _orgs = new VM_Orgs();
            int catId = _getOrgCategoryIdByAlias(catAlias);            
            _orgs.PagingInfo.SetData(page, _getOrgsTotalCount(catId, letter));
            if (_orgs.PagingInfo.TotalItems == -1) return null;
            _orgs.PagingInfo.CurrentPage = page;
            _orgs.Items = _getOrgs(catId, letter, _orgs.PagingInfo.GetNumberFrom(), _orgs.PagingInfo.GetNumberTo());
            return _orgs;
        }        
        public VM_Org GetOrg(int id)
        {
            string SQLQuery = _sqlGetOrg(id);
            VM_Org org = null;
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
                        LastError = "Ошибка во время загрузки организации (id=\"" + id.ToString() + "\")!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }

                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            org = new VM_Org();
                            org.Id = _reader.GetInt32(0);
                            org.Alias = _reader.GetString(1);
                            org.Title = _reader.GetString(2);
                            org.Descriptions = _reader.GetString(3);
                            org.Comment = _reader.GetString(4);
                            org.Icon = _reader.IsDBNull(5) ? "" : _reader.GetString(5);
                            org.IconTitle = _reader.GetString(6);
                            org.IconAlt = _reader.GetString(7);
                            org.CategoryId = _reader.GetInt32(8);                            
                            org.CategoryAlias = _getOrgCategoryAliasById(org.CategoryId);
                            org.ParentId = _reader.IsDBNull(9) ? 0 : _reader.GetInt32(9);                            
                            org.RegionId = _reader.IsDBNull(10) ? Guid.Empty : _reader.GetGuid(10);                            
                            org.MetaTitle = _reader.GetString(11);
                            org.MetaDescriptions = _reader.GetString(12);
                            org.MetaNoFollow = _reader.GetBoolean(13);
                            org.MetaNoIndex = _reader.GetBoolean(14);
                            org.CreatedAt = _reader.GetDateTime(15);
                            org.UpdatedAt = _reader.GetDateTime(16);
                            org.IsActive = _reader.GetBoolean(17);
                            org.Hits = _reader.GetInt32(18);
                            org.LastVisitedAt = _reader.IsDBNull(19) ? null : (DateTime?)_reader.GetDateTime(19);
                            org.MetaKeywords = _reader.GetString(20);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки организации (id=\"" + id.ToString() + "\")!\n" + ex.ToString();
                    log.Error(LastError);
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
            return org;
        }
        public VM_Org GetOrg(string alias)
        {
            string SQLQuery = _sqlGetOrg(alias);
            VM_Org org = null;
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
                        LastError = "Ошибка во время загрузки организации (alias=\"" + alias + "\")!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            org = new VM_Org();
                            org.Id = _reader.GetInt32(0);
                            org.Alias = _reader.GetString(1);
                            org.Title = _reader.GetString(2);
                            org.Descriptions = _reader.GetString(3);
                            org.Comment = _reader.GetString(4);
                            org.Icon = _reader.IsDBNull(5) ? "" : _reader.GetString(5);
                            org.IconTitle = _reader.GetString(6);
                            org.IconAlt = _reader.GetString(7);
                            org.CategoryId = _reader.GetInt32(8);
                            org.ParentId = _reader.IsDBNull(9) ? 0 : _reader.GetInt32(9);
                            org.RegionId = _reader.IsDBNull(10) ? Guid.Empty : _reader.GetGuid(10);
                            org.MetaTitle = _reader.GetString(11);
                            org.MetaDescriptions = _reader.GetString(12);
                            org.MetaNoFollow = _reader.GetBoolean(13);
                            org.MetaNoIndex = _reader.GetBoolean(14);
                            org.CreatedAt = _reader.GetDateTime(15);
                            org.UpdatedAt = _reader.GetDateTime(16);
                            org.IsActive = _reader.GetBoolean(17);
                            org.Hits = _reader.GetInt32(18);
                            org.LastVisitedAt = _reader.IsDBNull(19) ? null : (DateTime?)_reader.GetDateTime(19);
                            org.MetaKeywords = _reader.GetString(20);

                            org.CategoryAlias = _getOrgCategoryAliasById(org.CategoryId);
                            //return org;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки организации (alias=\"" + alias + "\")!\n" + ex.ToString();
                    log.Error(LastError);
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
            if (org == null) return null;
            
            Dictionary<int, int> points = _getOrgsPointsCount(new int[] { org.Id });
            if (points != null && points.Count > 0)
            {
                org.PointsCount = points.First().Value;
            }
            Dictionary<int, int> reviews = _getOrgsReviewsCount(new int[] { org.Id });
            if (reviews != null && reviews.Count > 0)
            {
                org.ReviewsCount = reviews.First().Value;
            }
            return org;
        }
        public VM_OrgItem GetOrgItem(int id)
        {
            string SQLQuery = _sqlGetOrgItem(id);
            VM_OrgItem org = new VM_OrgItem();
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
                        LastError = "Ошибка во время загрузки организации (id=\"" + id.ToString() + "\")!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }

                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            org.Id = _reader.GetInt32(0);
                            org.Alias = _reader.GetString(1);
                            org.Title = _reader.GetString(2);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки организации (id=\"" + id.ToString() + "\")!\n" + ex.ToString();
                    log.Error(LastError);
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
            return org;
        }
        public List<VM_OrgItem> GetOrgItems()
        {
            string SQLQuery = _sqlGetOrgItems();
            List<VM_OrgItem> orgs = new List<VM_OrgItem>();            
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
                        LastError = "Ошибка во время загрузки организаций!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }

                    if (_reader.HasRows)
                    {
                        while (_reader.Read())
                        {
                            orgs.Add(new VM_OrgItem(){
                            Id = _reader.GetInt32(0),
                            Alias = _reader.GetString(1),
                            Title = _reader.GetString(2)
                        });
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки организаций!\n" + ex.ToString();
                    log.Error(LastError);
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
            return orgs;
        }        
        public bool CreateOrg(ref VM_Org org, HttpPostedFileBase icon)
        {
            //Формирование ссылки
            string alias = GenerateAlias(org.Title, org.RegionId == Guid.Empty ? "" : "Воронежская область");
            if (String.IsNullOrEmpty(alias)) return false;            
            org.Alias = alias;
            
            //Сохранение иконки
            org.Icon = _saveOrgIcon(icon, alias);
            
            //Формирование запроса
            string SQLQuery = _sqlCreateOrg(org);            
            SqlCommand _command = null;            
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object objId = _command.ExecuteScalar();
                    if (objId == null)
                    {
                        LastError = "Ошибка во время создания организации!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return false;
                    }
                    //Новый Id
                    org.Id = Convert.ToInt32(objId.ToString());
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время создания организации!\n" + ex.ToString();
                    log.Error(LastError);
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
        public bool UpdateOrg(int id, VM_Org org, HttpPostedFileBase icon)
        {            
            //Сохранение иконки
            if(icon != null)
                org.Icon = _saveOrgIcon(icon, org.Alias);

            //Формирование запроса
            string SQLQuery = _sqlUpdateOrg(id, org);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время обновления организации!\nСервер вернул ответ " + i.ToString() + ".";
                        log.Error(LastError);
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время обновления организации!\n" + ex.ToString();
                    log.Error(LastError);
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
        public bool DeleteOrg(int id)
        {
            string SQLQuery = _sqlDeleteOrg(id);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время удаления организации!\nСервер вернул ответ " + i.ToString() + ".";
                        log.Error(LastError);
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время удаления организации!\n" + ex.ToString();
                    log.Error(LastError);
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
        public bool SetOrgActive(int id, bool action)
        {
            //Формирование запроса
            string SQLQuery = _sqlSetOrgActive(id, action);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время создания организации!\nСервер вернул ответ " + i.ToString() + ".";
                        log.Error(LastError);
                        return false;
                    }                    
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время установки активности организации!\n" + ex.ToString();
                    log.Error(LastError);
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

        #region ОФИСЫ И БАНКОМАТЫ
        public VM_OrgPoints GetOrgPoints(int orgId, VM_OrgsPointsFilters filter, int page = 1)
        {
            VM_OrgPoints orgPoints = new VM_OrgPoints();
            orgPoints.Org = GetOrg(orgId);
            orgPoints.Filters.Assign(filter);
            orgPoints.PagingInfo.SetData(page, _getOrgPointsTotalCount(orgId, filter));
            if (orgPoints.PagingInfo.TotalItems == -1) return null;
            orgPoints.PagingInfo.CurrentPage = page;
            orgPoints.Items = _getOrgPoints(orgId, filter, orgPoints.PagingInfo.GetNumberFrom(), orgPoints.PagingInfo.GetNumberTo());
            return orgPoints;
        }
        public VM_OrgPoints GetOrgPoints(string orgAlias, string type, Guid regionId, int page = 1)
        {
            VM_OrgPoints orgPoints = new VM_OrgPoints();
            orgPoints.Org = GetOrg(orgAlias);
            if (orgPoints.Org == null) return null;
            switch(type)
            {
                case "office":
                    orgPoints.Filters.PointType = EnumOrgPointType.Office;
                    break;
                case "bankomaty":
                    orgPoints.Filters.PointType = EnumOrgPointType.ATM;
                    break;
                default:
                    orgPoints.Filters.PointType = EnumOrgPointType.Office;
                    break;
            }
            orgPoints.Filters.IsActive = EnumBoolType.True;
            orgPoints.PagingInfo.ItemsPerPage = 30;
            orgPoints.PagingInfo.SetData(page, _getOrgPointsTotalCount(orgPoints.Org.Id, orgPoints.Filters));
            if (orgPoints.PagingInfo.TotalItems == -1) return null;
            orgPoints.Items = _getOrgPoints(orgPoints.Org.Id, orgPoints.Filters, regionId, orgPoints.PagingInfo.GetNumberFrom(), orgPoints.PagingInfo.GetNumberTo());
            return orgPoints;
        }
        public List<VM_OrgPoint> GetPoints(int orgId, Guid regionId)
        {
            string SQLQuery = _sqlGetOrgPoints(orgId, regionId);
            List<VM_OrgPoint> points = new List<VM_OrgPoint>();
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
                        LastError = "Ошибка во время загрузки офисов и банкоматов!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_OrgPoint orgPoint = null;
                        while (_reader.Read())
                        {
                            orgPoint = new VM_OrgPoint();
                            orgPoint.Id = _reader.GetInt32(0);
                            orgPoint.OrgId = _reader.GetInt32(1);
                            orgPoint.PointType = (EnumOrgPointType)_reader.GetInt32(2);
                            orgPoint.Phones = _reader.GetString(3);
                            orgPoint.Address = _reader.GetString(4);
                            orgPoint.AddressDopInfo = _reader.GetString(5);
                            orgPoint.Schedule = _reader.GetString(6);
                            orgPoint.DopInfo = _reader.GetString(7);
                            orgPoint.Title = _reader.GetString(8);
                            orgPoint.Alias = _reader.GetString(9);
                            orgPoint.RegionId = _reader.GetGuid(10);
                            orgPoint.IsActive = _reader.GetBoolean(11);
                            points.Add(orgPoint);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки офисов и банкоматов!\n" + ex.ToString();
                    log.Error(LastError);
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
            return points;
        }
        public VM_OrgPoint GetOrgPoint(int id)
        {
            string SQLQuery = _sqlGetOrgPoint(id);
            VM_OrgPoint orgPoint = new VM_OrgPoint();
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
                        LastError = "Ошибка во время загрузки офиса/банкомата (id=\"" + id.ToString() + "\")!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }

                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            orgPoint.Id = _reader.GetInt32(0);                            
                            orgPoint.OrgId = _reader.GetInt32(1);
                            orgPoint.PointType = (EnumOrgPointType)_reader.GetInt32(2);
                            orgPoint.Phones= _reader.GetString(3);
                            orgPoint.Address= _reader.GetString(4);
                            orgPoint.AddressDopInfo= _reader.GetString(5);
                            orgPoint.Schedule= _reader.GetString(6);
                            orgPoint.DopInfo= _reader.GetString(7);
                            orgPoint.Title = _reader.GetString(8);
                            orgPoint.Alias = _reader.GetString(9);
                            orgPoint.RegionId = _reader.GetGuid(10);
                            orgPoint.IsActive = _reader.GetBoolean(11);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки офиса/банкомата (id=\"" + id.ToString() + "\")!\n" + ex.ToString();
                    log.Error(LastError);
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
            return orgPoint;
        }
        public bool CreateOrgPoint(ref VM_OrgPoint orgPoint)
        {
            //Формирование ссылки
            string alias = GeneratePointAlias(orgPoint.Title, orgPoint.RegionId == Guid.Empty ? "" : "Воронежская область");
            if (String.IsNullOrEmpty(alias)) return false;
            orgPoint.Alias = alias;
            
            //Формирование запроса
            string SQLQuery = _sqlCreateOrgPoint(orgPoint);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object objId = _command.ExecuteScalar();
                    if (objId == null)
                    {
                        LastError = "Ошибка во время создания офиса/банкомата!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return false;
                    }
                    //Новый Id
                    orgPoint.Id = Convert.ToInt32(objId.ToString());
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время создания офиса/банкомата!\n" + ex.ToString();
                    log.Error(LastError);
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
        public bool UpdateOrgPoint(int id, VM_OrgPoint org)
        {            
            //Формирование запроса
            string SQLQuery = _sqlUpdateOrgPoint(id, org);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время обновления офиса/банкомата!\nСервер вернул ответ " + i.ToString() + ".";
                        log.Error(LastError);
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время обновления офиса/банкомата!\n" + ex.ToString();
                    log.Error(LastError);
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
        public bool DeleteOrgPoint(int id)
        {
            //Формирование запроса
            string SQLQuery = _sqlDeleteOrgPoint(id);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время удаления офиса/банкомата!\nСервер вернул ответ " + i.ToString() + ".";
                        log.Error(LastError);
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время удаления офиса/банкомата!\n" + ex.ToString();
                    log.Error(LastError);
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
        public bool SetOrgPointActive(int id, bool action)
        {
            //Формирование запроса
            string SQLQuery = _sqlSetOrgPointActive(id, action);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время установки активности офиса/банкомата!\nСервер вернул ответ " + i.ToString() + ".";
                        log.Error(LastError);
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время установки активности офиса/банкомата!\n" + ex.ToString();
                    log.Error(LastError);
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

        #region ОТЗЫВЫ
        //Отзывы
        public bool CreateReview(VM_ReviewResponse review)
        {
            VM_Review r = new VM_Review();
            r.Confirmed = EnumConfirmStatus.NotConfirmed;
            r.IsActive = true;
            r.OrgId = review.OrgId;
            r.Rating = review.Rating;
            r.ReviewText = review.ReviewText;
            r.UserId = 3;
            r.UserName = review.UserName;            
            //Формирование запроса
            string SQLQuery = _sqlCreateReview(r);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object objId = _command.ExecuteScalar();
                    if (objId == null)
                    {
                        LastError = "Ошибка во время создания отзыва!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return false;
                    }                    
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время создания отзыва!\n" + ex.ToString();
                    log.Error(LastError);
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
        public VM_OrgReviews GetOrgReviews(int? orgId, VM_OrgsReviewsFilters filter, int page = 1)
        {
            VM_OrgReviews orgReviews = new VM_OrgReviews();
            orgReviews.Org = orgId == null ? null : GetOrg((int)orgId);
            orgReviews.Filters.Assign(filter);
            orgReviews.PagingInfo.SetData(page, _getOrgReviewsTotalCount(orgId, filter));
            if (orgReviews.PagingInfo.TotalItems == -1) return null;
            orgReviews.PagingInfo.CurrentPage = page;
            orgReviews.Items = _getOrgReviews(orgId, filter, orgReviews.PagingInfo.GetNumberFrom(), orgReviews.PagingInfo.GetNumberTo());
            return orgReviews;
        }
        public List<VM_Review> GetLastReview(int? org_id, int? categoryId, int recCount)
        {
            string SQLQuery = _sqlGetLastReview(org_id, categoryId, recCount);
            List<VM_Review> reviews = new List<VM_Review>();
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
                        LastError = "Ошибка во время загрузки последних отзывов!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_Review r = null;
                        while (_reader.Read())
                        {
                            r = new VM_Review();
                            r.Id = _reader.GetInt32(0);
                            r.OrgId = _reader.GetInt32(1);
                            r.UserId = _reader.GetInt32(2);
                            r.UserName = _reader.GetString(3);
                            r.ReviewText = _reader.GetString(4);
                            r.CreatedAt = _reader.GetDateTime(5);
                            r.Rating = _reader.GetInt32(6);
                            r.IsActive = _reader.GetBoolean(7);
                            r.Confirmed = (EnumConfirmStatus)_reader.GetInt32(8);
                            r.ReviewTitle = _reader.GetString(9);
                            r.Org = new VM_OrgItem();
                            reviews.Add(r);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки последних отзывов!\n" + ex.ToString();
                    log.Error(LastError);
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
            if (reviews != null && reviews.Count > 0)
            {
                int[] ids = (from r in reviews
                             where r.OrgId > 0
                             select r.OrgId).Distinct().ToArray<int>();
                List<VM_OrgItem> orgs = _getOrgs(ids);
                if (orgs != null && orgs.Count > 0)
                {
                    VM_OrgItem org = null;
                    foreach (VM_Review review in reviews)
                    {
                        org = orgs.FirstOrDefault(r => r.Id == review.OrgId);
                        if (org != null)
                        {
                            review.Org.Assign(org);
                            if (review.Org.Category == null)
                                review.Org.Category = new VM_OrgCategoryItem();
                            review.Org.Category.Alias = _getOrgCategoryAliasById(review.Org.Category.Id);
                        }
                    }
                }
            }
            return reviews;
        }
        public VM_OrgReviews GetOrgReviews(string orgAlias, int page)
        {
            VM_OrgReviews orgReviews = new VM_OrgReviews();
            orgReviews.Org = GetOrg(orgAlias);
            if (orgReviews.Org == null)
                return null;
            orgReviews.PagingInfo.ItemsPerPage = 10;
            orgReviews.PagingInfo.SetData(page, _getOrgReviewsTotalCount(orgReviews.Org.Id));
            if (orgReviews.PagingInfo.TotalItems == -1) return null;
            orgReviews.PagingInfo.CurrentPage = page;
            orgReviews.Items = GetReviews(orgReviews.Org.Id, orgReviews.PagingInfo.GetNumberFrom(), orgReviews.PagingInfo.GetNumberTo());
            return orgReviews;
        }
        public List<VM_Review> GetReviews(int orgId, int offset, int limit)
        {
            string SQLQuery = _sqlGetReviews(orgId, offset, limit);
            List<VM_Review> reviews = new List<VM_Review>();
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
                        LastError = "Ошибка во время загрузки отзывов!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_Review r = null;
                        while (_reader.Read())
                        {
                            r = new VM_Review();
                            r.Id = _reader.GetInt32(1);
                            r.OrgId = _reader.GetInt32(2);
                            r.UserId = _reader.GetInt32(3);
                            r.UserName = _reader.GetString(4);
                            //r.ReviewText = DbHelper.GetPartUseTags(_reader.GetString(5), 500);
                            r.ReviewText = _reader.GetString(5);
                            r.CreatedAt = _reader.GetDateTime(6);
                            r.Rating = _reader.GetInt32(7);
                            r.IsActive = _reader.GetBoolean(8);
                            r.Confirmed = (EnumConfirmStatus)_reader.GetInt32(9);
                            r.ReviewTitle = _reader.GetString(10);
                            reviews.Add(r);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки отзывов!\n" + ex.ToString();
                    log.Error(LastError);
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
            return reviews;
        }
        public VM_OrgReview GetOrgReview(string orgAlias, int review_id)
        {
            VM_OrgReview orgReview = new VM_OrgReview();
            orgReview.Org = GetOrg(orgAlias);
            if (orgReview.Org == null)
                return null;            
            orgReview.Review = GetReview(review_id, true);
            return orgReview;
        }
        public VM_Review GetReview(int review_id, bool onlyPublished = false)
        {
            string SQLQuery = _sqlGetReview(review_id, onlyPublished);            
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
                        //LastError = "Ошибка во время загрузки последних отзывов!\nСервер вернул ответ NULL.";
                        LastError = String.Format("Ошибка во время загрузки отзыва (id={0})!\nСервер вернул ответ NULL", review_id);
                        log.Error(LastError);
                        return null;
                    }
                    if (_reader.HasRows)
                    {                        
                        if (_reader.Read())
                        {
                            VM_Review review = new VM_Review();
                            review.Id = _reader.GetInt32(0);
                            review.OrgId = _reader.GetInt32(1);
                            review.UserId = _reader.GetInt32(2);
                            review.UserName = _reader.GetString(3);
                            review.ReviewText = _reader.GetString(4);
                            review.CreatedAt = _reader.GetDateTime(5);
                            review.Rating = _reader.GetInt32(6);
                            review.IsActive = _reader.GetBoolean(7);
                            review.Confirmed = (EnumConfirmStatus)_reader.GetInt32(8);
                            review.ReviewTitle = _reader.GetString(9);
                            return review;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = String.Format("Ошибка во время загрузки отзыва (id={0})!\n{1}", review_id, ex.ToString());
                    log.Error(LastError);
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
        public bool CreateOrgReview(ref VM_Review review)
        {            
            //Формирование запроса
            string SQLQuery = _sqlCreateOrgReview(review);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object objId = _command.ExecuteScalar();
                    if (objId == null)
                    {
                        LastError = "Ошибка во время создания отзыва!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return false;
                    }
                    //Новый Id
                    review.Id = Convert.ToInt32(objId.ToString());
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время создания отзыва!\n" + ex.ToString();
                    log.Error(LastError);
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
        public bool UpdateOrgReview(int id, VM_Review review)
        {
            //Формирование запроса
            string SQLQuery = _sqlUpdateOrgReview(id, review);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время обновления отзыва!\nСервер вернул ответ " + i.ToString() + ".";
                        log.Error(LastError);
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время обновления отзыва!\n" + ex.ToString();
                    log.Error(LastError);
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
        public bool DeleteOrgReview(int id)
        {
            //Формирование запроса
            string SQLQuery = _sqlDeleteOrgReview(id);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время удаления отзыва!\nСервер вернул ответ " + i.ToString() + ".";
                        log.Error(LastError);
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время удаления отзыва!\n" + ex.ToString();
                    log.Error(LastError);
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
        public bool SetOrgReviewActive(int id, bool action)
        {
            //Формирование запроса
            string SQLQuery = _sqlSetOrgReviewActive(id, action);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время установки активности отзыва!\nСервер вернул ответ " + i.ToString() + ".";
                        log.Error(LastError);
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время установки активности отзыва!\n" + ex.ToString();
                    log.Error(LastError);
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
        public bool SetReviewComfirmed(int id, bool action)
        {
            //Формирование запроса
            string SQLQuery = _sqlSetOrgReviewComfirmation(id, action);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    int i = _command.ExecuteNonQuery();
                    if (i < 0)
                    {
                        LastError = "Ошибка во время установки статуса подтверждения отзыва!\nСервер вернул ответ " + i.ToString() + ".";
                        log.Error(LastError);
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время установки статуса подтверждения отзыва!\n" + ex.ToString();
                    log.Error(LastError);
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
        public VM_OrgCategory GetOrgCategory(string alias)
        {
            string SQLQuery = _sqlGetOrgCategoryByAlias(alias);
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
                        LastError = String.Format("Ошибка во время загрузки категории организаций ({0})!\nСервер вернул ответ NULL.", alias);
                        log.Error(LastError);
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        if(_reader.Read())
                        {
                            VM_OrgCategory cat = new VM_OrgCategory()
                            {
                                Id = _reader.GetInt32(0),
                                Alias = _reader.GetString(1),
                                Title = _reader.GetString(2),
                                Description = _reader.GetString(3),
                                Icon = _reader.IsDBNull(4) ? String.Empty : _reader.GetString(4),
                                MetaTitle = _reader.GetString(5),
                                MetaDescriptions = _reader.GetString(6),
                                MetaKeywords = _reader.GetString(7),
                                MetaNoFollow = _reader.GetBoolean(8),
                                MetaNoIndex = _reader.GetBoolean(9),
                                IsActive = _reader.GetBoolean(10)
                            };
                            return cat;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = String.Format("Ошибка во время загрузки категории организаций ({0})!\n{1}", alias, ex.ToString());
                    log.Error(LastError);
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
        public List<VM_OrgCategoryItem> GetOrgCategories()
        {
            string SQLQuery = _sqlGetOrgCategories();
            List<VM_OrgCategoryItem> cats = new List<VM_OrgCategoryItem>();
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
                        LastError = "Ошибка во время загрузки категорий организаций!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }

                    if (_reader.HasRows)
                    {
                        while (_reader.Read())
                        {
                            cats.Add(new VM_OrgCategoryItem()
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
                    LastError = "Ошибка во время загрузки категорий организаций!\n" + ex.ToString();
                    log.Error(LastError);
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
        #endregion

        #region ЧАСТНЫЕ МЕТОДЫ

        #region ПОИСК
        //Поиск организаций
        private int _getOrgsTotalCount(VM_OrgsFilters filter)
        {
            string SQLQuery = _sqlGetOrgsTotalCount(filter);           
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object count = _command.ExecuteScalar();
                    if (count == null)
                    {
                        LastError = "Ошибка во время определения общего числа организаций!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return -1;
                    }
                    return (int)count;                    
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время определения общего числа организаций!\n" + ex.ToString();
                    log.Error(LastError);
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
        private int _getOrgsTotalCount(int catId, string letter)
        {
            string SQLQuery = _sqlGetOrgsTotalCount(catId, letter);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object count = _command.ExecuteScalar();
                    if (count == null)
                    {
                        LastError = "Ошибка во время определения общего числа организаций!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return -1;
                    }
                    return (int)count;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время определения общего числа организаций!\n" + ex.ToString();
                    log.Error(LastError);
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
        private List<VM_OrgItem> _getOrgs(VM_OrgsFilters filter, int offset, int limit)
        {
            string SQLQuery = _sqlGetOrgs(filter, offset, limit);
            List<VM_OrgItem> items = new List<VM_OrgItem>();
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
                        LastError = "Ошибка во время загрузки организаций!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }                                        
                    if (_reader.HasRows)
                    {
                        VM_OrgItem item = null;
                        while (_reader.Read())
                        {
                            item = new VM_OrgItem()
                            {
                                Id = _reader.GetInt32(1),
                                Alias = _reader.GetString(2),
                                Title = _reader.GetString(3),
                                IsActive = _reader.GetBoolean(4),                                
                                Region = _reader.IsDBNull(6) ? "РФ" : GetRegionName(_reader.GetGuid(6))
                            };
                            if(!_reader.IsDBNull(5))
                            {
                                item.Parent = new VM_OrgItem();
                                item.Parent.Id = _reader.GetInt32(5);
                            }
                            items.Add(item);
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки организаций!\n" + ex.ToString();
                    log.Error(LastError);
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
                int[] parentIds = (from i in items
                                   where i.Parent != null
                                   select i.Parent.Id).Distinct().ToArray<int>();
                int[] ids = (from i in items                                   
                                   select i.Id).Distinct().ToArray<int>();
                if (parentIds != null && parentIds.Length > 0)
                {
                    List<VM_OrgItem> parents = _getOrgs(parentIds);
                    if (parents != null && parents.Count > 0)
                    {
                        VM_OrgItem item = null;
                        foreach (VM_OrgItem par in parents)
                        {
                            item = items.FirstOrDefault(i => i.Parent != null && i.Parent.Id == par.Id);
                            if (item != null)
                                item.Parent.Assign(par);
                        }
                    }
                }
                if (ids != null && ids.Length > 0)
                {
                    Dictionary<int, int> points = _getOrgsPointsCount(ids);
                    if (points != null && points.Count > 0)
                    {
                        VM_OrgItem item = null;
                        foreach (var p in points)
                        {
                            item = items.FirstOrDefault(i => i.Id == p.Key);
                            if (item != null)
                                item.PointsCount = p.Value;
                        }
                    }
                }
            }
            return items;
        }
        private List<VM_OrgItem> _getOrgs(int catId, string letter, int offset, int limit)
        {
            string SQLQuery = _sqlGetOrgs(catId, letter, offset, limit);
            List<VM_OrgItem> items = new List<VM_OrgItem>();
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
                        LastError = "Ошибка во время загрузки организаций!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_OrgItem item = null;
                        while (_reader.Read())
                        {
                            item = new VM_OrgItem();
                            item.Id = _reader.GetInt32(1);
                            item.Alias = _reader.GetString(2);
                            item.Title = _reader.GetString(3);
                            item.IsActive = _reader.GetBoolean(4);
                            if (!_reader.IsDBNull(5))
                            {
                                item.Parent = new VM_OrgItem();
                                item.Parent.Id = _reader.GetInt32(5);
                            }
                            item.Region = _reader.IsDBNull(6) ? "РФ" : GetRegionName(_reader.GetGuid(6));
                            if (item.Category == null)
                                item.Category = new VM_OrgCategoryItem();
                            item.Category.Id = _reader.GetInt32(7);
                            item.Category.Alias = _getOrgCategoryAliasById(_reader.GetInt32(7));
                            item.Icon = _reader.IsDBNull(8) ? String.Empty : _reader.GetString(8);
                            items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки организаций!\n" + ex.ToString();
                    log.Error(LastError);
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
                    Dictionary<int, int> points = _getOrgsPointsCount(ids);
                    if (points != null && points.Count > 0)
                    {
                        VM_OrgItem item = null;
                        foreach (var p in points)
                        {
                            item = items.FirstOrDefault(i => i.Id == p.Key);
                            if (item != null)
                                item.PointsCount = p.Value;
                        }
                    }
                    Dictionary<int, int> reviews = _getOrgsReviewsCount(ids);
                    if (reviews != null && reviews.Count > 0)
                    {
                        VM_OrgItem item = null;
                        foreach (var r in reviews)
                        {
                            item = items.FirstOrDefault(i => i.Id == r.Key);
                            if (item != null)
                                item.ReviewsCount = r.Value;
                        }
                    }
                }
            }
            return items;
        }
        private List<VM_OrgItem> _getOrgs(int[] ids)
        {
            string SQLQuery = _sqlGetOrgs(ids);
            List<VM_OrgItem> items = new List<VM_OrgItem>();
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
                        LastError = "Ошибка во время загрузки организаций!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_OrgItem item = null;
                        while (_reader.Read())
                        {
                            item = new VM_OrgItem();
                            item.Category = new VM_OrgCategoryItem();
                            item.Id = _reader.GetInt32(0);
                            item.Alias = _reader.GetString(1);
                            item.Title = _reader.GetString(2);
                            item.IsActive = _reader.GetBoolean(3);
                            if (!_reader.IsDBNull(4))
                            {
                                item.Parent = new VM_OrgItem();
                                item.Parent.Id = _reader.GetInt32(4);
                            }
                            item.Region = _reader.IsDBNull(5) ? "РФ" : GetRegionName(_reader.GetGuid(5));
                            item.Category.Id = _reader.GetInt32(6);
                            items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки организаций!\n" + ex.ToString();
                    log.Error(LastError);
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
        private Dictionary<int, int> _getOrgsPointsCount(int[] ids)
        {
            string SQLQuery = _sqlGetOrgsPointsCount(ids);
            Dictionary<int, int> points = new Dictionary<int, int>();
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
                        LastError = "Ошибка во время загрузки количества офисов!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }
                    if (_reader.HasRows)
                    {                        
                        while (_reader.Read())
                        {                            
                            points.Add(_reader.GetInt32(0), _reader.GetInt32(1));
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки количества офисов!\n" + ex.ToString();
                    log.Error(LastError);
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
            return points;
        }
        private Dictionary<int, int> _getOrgsReviewsCount(int[] ids)
        {
            string SQLQuery = _sqlGetOrgsReviewsCount(ids);
            Dictionary<int, int> reviews = new Dictionary<int, int>();
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
                        LastError = "Ошибка во время загрузки количества отзывов!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        while (_reader.Read())
                        {
                            reviews.Add(_reader.GetInt32(0), _reader.GetInt32(1));
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки количества отзывов!\n" + ex.ToString();
                    log.Error(LastError);
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
            return reviews;
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
                    log.Error(LastError);
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
        //Поиск офисов и банкоматов
        private int _getOrgPointsTotalCount(int orgId, VM_OrgsPointsFilters filter)
        {
            string SQLQuery = _sqlGetOrgPointsTotalCount(orgId, filter);

            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object count = _command.ExecuteScalar();
                    if (count == null)
                    {
                        LastError = "Ошибка во время определения общего числа офисов/банкоматов!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return -1;
                    }
                    return (int)count;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время определения общего числа офисов/банкоматов!\n" + ex.ToString();
                    log.Error(LastError);
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
        private List<VM_OrgPoint> _getOrgPoints(int orgId, VM_OrgsPointsFilters filter, int offset, int limit)
        {
            string SQLQuery = _sqlGetOrgPoints(orgId, filter, offset, limit);
            List<VM_OrgPoint> items = new List<VM_OrgPoint>();
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
                        LastError = "Ошибка во время загрузки офисов/банкоматов!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_OrgPoint item = null;
                        while (_reader.Read())
                        {
                            item = new VM_OrgPoint()
                            {
                                Id = _reader.GetInt32(1),                            
                                OrgId = _reader.GetInt32(2),
                                PointType = (EnumOrgPointType)_reader.GetInt32(3),
                                Phones = _reader.GetString(4),
                                Address = _reader.GetString(5),
                                AddressDopInfo = _reader.GetString(6),
                                Schedule = _reader.GetString(7),
                                DopInfo = _reader.GetString(8),
                                Title = _reader.GetString(9),
                                Alias = _reader.GetString(10),
                                RegionId = _reader.GetGuid(11),
                                IsActive = _reader.GetBoolean(12)
                            };
                            if (item.PointType != EnumOrgPointType.None)
                                item.PointTypeAsString = GetPointTypeAsString(item.PointType);
                            if (item.RegionId != Guid.Empty)
                                item.Region = GetRegionName(item.RegionId);
                            items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки офисов/банкоматов!\n" + ex.ToString();
                    log.Error(LastError);
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
        private List<VM_OrgPoint> _getOrgPoints(int orgId, VM_OrgsPointsFilters filter, Guid regionId, int offset, int limit)
        {
            string SQLQuery = _sqlGetOrgPoints(orgId, filter, offset, limit);
            List<VM_OrgPoint> items = new List<VM_OrgPoint>();
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
                        LastError = "Ошибка во время загрузки офисов/банкоматов!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_OrgPoint item = null;
                        while (_reader.Read())
                        {
                            item = new VM_OrgPoint()
                            {
                                Id = _reader.GetInt32(1),
                                OrgId = _reader.GetInt32(2),
                                PointType = (EnumOrgPointType)_reader.GetInt32(3),
                                Phones = _reader.GetString(4),
                                Address = _reader.GetString(5),
                                AddressDopInfo = _reader.GetString(6),
                                Schedule = _reader.GetString(7),
                                DopInfo = _reader.GetString(8),
                                Title = _reader.GetString(9),
                                Alias = _reader.GetString(10),
                                RegionId = _reader.GetGuid(11),
                                IsActive = _reader.GetBoolean(12)
                            };
                            if (item.PointType != EnumOrgPointType.None)
                                item.PointTypeAsString = GetPointTypeAsString(item.PointType);
                            if (item.RegionId != Guid.Empty)
                                item.Region = GetRegionName(item.RegionId);
                            items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки офисов/банкоматов!\n" + ex.ToString();
                    log.Error(LastError);
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
        private bool _dbPointAliasExists(string alias)
        {
            string SQLQuery = _sqlGetPointAliasExists(alias);
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
                    LastError = "Ошибка во время проверки ссылки офиса/банкомата на существование!\n" + ex.ToString();
                    log.Error(LastError);
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
        //Отзывы
        private int _getOrgReviewsTotalCount(int? orgId, VM_OrgsReviewsFilters filter)
        {
            string SQLQuery = _sqlGetOrgReviewsTotalCount(orgId, filter);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object count = _command.ExecuteScalar();
                    if (count == null)
                    {
                        LastError = "Ошибка во время определения общего числа отзывов!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return -1;
                    }
                    return (int)count;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время определения общего числа отзывов!\n" + ex.ToString();
                    log.Error(LastError);
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
        private List<VM_Review> _getOrgReviews(int? orgId, VM_OrgsReviewsFilters filter, int offset, int limit)
        {
            string SQLQuery = _sqlGetOrgReviews(orgId, filter, offset, limit);
            List<VM_Review> items = new List<VM_Review>();
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
                        LastError = "Ошибка во время загрузки отзывов!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return null;
                    }
                    if (_reader.HasRows)
                    {
                        VM_Review r = null;
                        while (_reader.Read())
                        {
                            r = new VM_Review();
                            r.Id = _reader.GetInt32(1);
                            r.OrgId = _reader.GetInt32(2);
                            r.UserId = _reader.GetInt32(3);
                            r.UserName = _reader.GetString(4);
                            r.ReviewText = _reader.GetString(5);
                            r.CreatedAt = _reader.GetDateTime(6);
                            r.Rating = _reader.GetInt32(7);
                            r.IsActive = _reader.GetBoolean(8);
                            r.Confirmed = (EnumConfirmStatus)_reader.GetInt32(9);
                            r.ReviewTitle = _reader.GetString(10);                            
                            items.Add(r);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки отзывов!\n" + ex.ToString();
                    log.Error(LastError);
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
                int[] ids = (from r in items
                             where r.OrgId > 0
                             select r.OrgId).Distinct().ToArray<int>();
                List<VM_OrgItem> orgs = _getOrgs(ids);
                if (orgs != null && orgs.Count > 0)
                {
                    VM_OrgItem org = null;
                    foreach (VM_Review review in items)
                    {
                        org = orgs.FirstOrDefault(r => r.Id == review.OrgId);
                        if (org != null)
                        {
                            if (review.Org == null)
                                review.Org = new VM_OrgItem();
                            review.Org.Assign(org);
                            if (review.Org.Category == null)
                                review.Org.Category = new VM_OrgCategoryItem();
                            review.Org.Category.Alias = _getOrgCategoryAliasById(review.Org.Category.Id);
                        }
                    }
                }
            }

            return items;
        }        
        private int _getOrgReviewsTotalCount(int org_id)
        {
            string SQLQuery = _sqlGetOrgReviewsTotalCount(org_id);
            SqlCommand _command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    _command = new SqlCommand(SQLQuery, GlobalParams.GetConnection());
                    object count = _command.ExecuteScalar();
                    if (count == null)
                    {
                        LastError = "Ошибка во время определения общего числа отзывов организации!\nСервер вернул ответ NULL.";
                        log.Error(LastError);
                        return -1;
                    }
                    return (int)count;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время определения общего числа отзывов организации!\n" + ex.ToString();
                    log.Error(LastError);
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
        #endregion

        #region ЗАПРОСЫ
        //Организации
        private string _sqlGetOrgsTotalCount(VM_OrgsFilters filter)
        {
            string SqlConds = _sqlGetFilter(filter);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.COUNT + "(*)";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Orgs.TABLENAME;
            if (!String.IsNullOrEmpty(SqlConds))
            {
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += SqlConds;
            }
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetOrgsTotalCount(int catId, string letter)
        {
            string SqlCond = String.Empty;
            SqlCond += " " + DbStruct.SE.AND + " ";
            SqlCond += DbStruct.Orgs.FIELDS.IsActive;
            SqlCond += " =1";
            if (catId != -1)
            {
                SqlCond += " " + DbStruct.SE.AND + " ";
                SqlCond += DbStruct.Orgs.FIELDS.CategoryId;
                SqlCond += " = " + catId.ToString();
            }
            if (!String.IsNullOrEmpty(letter))
            {
                SqlCond += " " + DbStruct.SE.AND + " ";
                SqlCond += "Upper(Substring(";
                SqlCond += DbStruct.Orgs.FIELDS.Title;
                SqlCond += ",1,1))";
                SqlCond += " = '" + letter + "'";
            }
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.COUNT + "(*)";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Orgs.TABLENAME;
            if (!String.IsNullOrEmpty(SqlCond))
            {
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += SqlCond.Substring(5);
            }
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetOrgs(int catId, string letter, int offset, int limit)
        {
            string SqlCond = String.Empty;
            SqlCond += " " + DbStruct.SE.AND + " ";
            SqlCond += DbStruct.Orgs.FIELDS.IsActive;
            SqlCond += " =1";
            if (catId != -1)
            {
                SqlCond += " " + DbStruct.SE.AND + " ";
                SqlCond += DbStruct.Orgs.FIELDS.CategoryId;
                SqlCond += " = " + catId.ToString();
            }
            if (!String.IsNullOrEmpty(letter))
            {
                SqlCond += " " + DbStruct.SE.AND + " ";
                SqlCond += "Upper(Substring(";
                SqlCond += DbStruct.Orgs.FIELDS.Title;
                SqlCond += ",1,1))";
                SqlCond += " = '" + letter + "'";
            }

            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " t1.* " + DbStruct.SE.FROM + "(";
            SqlQuery += DbStruct.SE.SELECT + " ROW_NUMBER() OVER(";
            SqlQuery += DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Title;
            SqlQuery += ") AS RowNumber, ";
            SqlQuery += DbStruct.Orgs.FIELDS.Id + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Title + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.ParentId + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.RegionId + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.CategoryId + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Icon;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Orgs.TABLENAME;
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
        private string _sqlGetOrgs(VM_OrgsFilters filter, int offset, int limit)
        {
            string SqlConds = _sqlGetFilter(filter);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " t1.* " + DbStruct.SE.FROM + "(";
            SqlQuery += DbStruct.SE.SELECT + " ROW_NUMBER() OVER(";            
            SqlQuery += DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Title;
            SqlQuery += ") AS RowNumber, ";            
            SqlQuery += DbStruct.Orgs.FIELDS.Id + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Title + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.ParentId + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.RegionId;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Orgs.TABLENAME;                        
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
        private string _sqlGetFilter(VM_OrgsFilters filter)
        {
            string SqlConds = String.Empty;
            if (filter.IsActive != EnumBoolType.None)
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.Orgs.FIELDS.IsActive;
                if (filter.IsActive == EnumBoolType.True)
                    SqlConds += " = 1";
                else
                    SqlConds += " = 0";
            }
            if (!String.IsNullOrEmpty(filter.Title))
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.Orgs.FIELDS.Title;
                SqlConds += " " + DbStruct.SE.LIKE + " ";
                SqlConds += "'%" + DbStruct.SQLRealEscapeString(filter.Title) + "%'";
            }
            if (filter.CategoryId > 0)
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.Orgs.FIELDS.CategoryId;
                SqlConds += " = " + filter.CategoryId.ToString();
            }
            if (filter.RegionId != Guid.Empty)
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.Orgs.FIELDS.RegionId;
                SqlConds += " = '" + filter.RegionId.ToString() + "'";
            }
            return String.IsNullOrEmpty(SqlConds) ? String.Empty : SqlConds.Substring(5);
        }
        private string _sqlGetOrgs(int[] ids)
        {
            if (ids.Length == 0) return String.Empty;
            string strIds = String.Empty;
            foreach (int id in ids)
                strIds += "," + id.ToString();
            strIds = strIds.Substring(1);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Id + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Title + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.ParentId + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.RegionId + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.CategoryId;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Orgs.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Id;
            SqlQuery += " " + DbStruct.SE.IN + "(" + strIds + ")";
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Title;
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetOrgsPointsCount(int[] ids)
        {
            if (ids.Length == 0) return String.Empty;
            string strIds = String.Empty;
            foreach (int id in ids)
                strIds += "," + id.ToString();
            strIds = strIds.Substring(1);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.OrgId + ",";
            SqlQuery += " " + DbStruct.SE.COUNT + "(";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Id;
            SqlQuery += ")";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.OrgPoints.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.OrgId;
            SqlQuery += " " + DbStruct.SE.IN + "(" + strIds + ")";
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.IsActive + " = 1";
            SqlQuery += " " + DbStruct.SE.GROUP_BY + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.OrgId;
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetOrgsReviewsCount(int[] ids)
        {
            if (ids.Length == 0) return String.Empty;
            string strIds = String.Empty;
            foreach (int id in ids)
                strIds += "," + id.ToString();
            strIds = strIds.Substring(1);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.OrgId + ",";
            SqlQuery += " " + DbStruct.SE.COUNT + "(";
            SqlQuery += DbStruct.Reviews.FIELDS.Id;
            SqlQuery += ")";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Reviews.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.OrgId;
            SqlQuery += " " + DbStruct.SE.IN + "(" + strIds + ")";
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.IsActive + " = 1";
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.ReviewState + " = 1";
            SqlQuery += " " + DbStruct.SE.GROUP_BY + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.OrgId;
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetOrg(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";            
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Orgs.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetOrg(string alias)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Orgs.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Alias;
            SqlQuery += " = '" + alias + "'";
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.IsActive;
            SqlQuery += " = 1";
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetOrgItem(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Id + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Title;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Orgs.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetOrgItems()
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Id + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Title;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Orgs.TABLENAME;
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Title;            
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlCreateOrg(VM_Org org)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.INSERT + " ";
            SqlQuery += DbStruct.Orgs.TABLENAME;
            SqlQuery += "(";            
            SqlQuery += DbStruct.Orgs.FIELDS.Alias + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Title + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.CategoryId + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.ChangedAt + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Comment + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.CreatedAt + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Descriptions + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Hits + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Icon + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.IconAlt + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.IconTitle + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.LastVisitedAt + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.MetaDescriptions + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.MetaKeywords + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.MetaNoFollow + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.MetaNoIndex + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.MetaTitle + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.ParentId + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.RegionId;            
            SqlQuery += ")";
            SqlQuery += " " + DbStruct.SE.VALUES + "(";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.Alias) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.Title) + "',";
            SqlQuery += org.CategoryId.ToString() + ",";
            SqlQuery += DbStruct.SE.GETDATE + ",";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.Comment) + "',";
            SqlQuery += DbStruct.SE.GETDATE + ",";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.Descriptions) + "',";
            SqlQuery += "0,";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.Icon) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.IconAlt) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.IconTitle) + "',";
            SqlQuery += org.IsActive ? "1," : "0,";
            SqlQuery += DbStruct.SE.NULL + ",";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.MetaDescriptions) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.MetaKeywords) + "',";
            SqlQuery += org.MetaNoFollow ? "1," : "0,";
            SqlQuery += org.MetaNoIndex ? "1," : "0,";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.MetaTitle) + "',";
            SqlQuery += (org.ParentId == 0 ? DbStruct.SE.NULL : org.ParentId.ToString()) + ",";
            SqlQuery += (org.RegionId == Guid.Empty ? DbStruct.SE.NULL : "'" + org.RegionId.ToString() + "'");
            SqlQuery += ")";            
            SqlQuery += ";";
            SqlQuery += DbStruct.SE.SELECT_SCOPE_IDENTITY;
            return SqlQuery;
        }
        private string _sqlUpdateOrg(int id, VM_Org org)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.Orgs.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";
            //SqlQuery += DbStruct.Orgs.FIELDS.Alias + " = ";
            //SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.Alias) + "',";
            SqlQuery += DbStruct.Orgs.FIELDS.Title + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.Title) + "',";
            SqlQuery += DbStruct.Orgs.FIELDS.CategoryId + " = ";
            SqlQuery += org.CategoryId.ToString() + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.ChangedAt + " = ";
            SqlQuery += DbStruct.SE.GETDATE + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.Comment + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.Comment) + "',";            
            SqlQuery += DbStruct.Orgs.FIELDS.Descriptions + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.Descriptions) + "',";
            SqlQuery += DbStruct.Orgs.FIELDS.Hits + " = ";
            SqlQuery += "0,";
            SqlQuery += DbStruct.Orgs.FIELDS.Icon + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.Icon) + "',";
            SqlQuery += DbStruct.Orgs.FIELDS.IconAlt + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.IconAlt) + "',";
            SqlQuery += DbStruct.Orgs.FIELDS.IconTitle + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.IconTitle) + "',";
            SqlQuery += DbStruct.Orgs.FIELDS.IsActive + " = ";
            SqlQuery += org.IsActive ? "1," : "0,";            
            SqlQuery += DbStruct.Orgs.FIELDS.MetaDescriptions + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.MetaDescriptions) + "',";
            SqlQuery += DbStruct.Orgs.FIELDS.MetaKeywords + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.MetaKeywords) + "',";
            SqlQuery += DbStruct.Orgs.FIELDS.MetaNoFollow + " = ";
            SqlQuery += org.MetaNoFollow ? "1," : "0,";
            SqlQuery += DbStruct.Orgs.FIELDS.MetaNoIndex + " = ";
            SqlQuery += org.MetaNoIndex ? "1," : "0,";
            SqlQuery += DbStruct.Orgs.FIELDS.MetaTitle + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(org.MetaTitle) + "',";
            SqlQuery += DbStruct.Orgs.FIELDS.ParentId + " = ";
            SqlQuery += (org.ParentId == 0 ? DbStruct.SE.NULL : org.ParentId.ToString()) + ",";
            SqlQuery += DbStruct.Orgs.FIELDS.RegionId + " = ";
            SqlQuery += (org.RegionId == Guid.Empty ? DbStruct.SE.NULL : "'" + org.RegionId.ToString() + "'");            
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlDeleteOrg(int id)
        {
            string SqlQuery = String.Empty;
            //Удаляем офисы
            SqlQuery += DbStruct.SE.DELETE + " ";
            SqlQuery += DbStruct.OrgPoints.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.OrgId;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            //Удаляем отзывы
            SqlQuery += DbStruct.SE.DELETE + " ";
            SqlQuery += DbStruct.Reviews.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.OrgId;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            //Удаляем организацию
            SqlQuery += DbStruct.SE.DELETE + " ";
            SqlQuery += DbStruct.Orgs.TABLENAME;                        
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlSetOrgActive(int id, bool action)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.Orgs.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";            
            SqlQuery += DbStruct.Orgs.FIELDS.IsActive + " = ";
            SqlQuery += action ? "1" : "0";            
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetAliasExists(string alias)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Alias;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Orgs.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Orgs.FIELDS.Alias;
            SqlQuery += " = '" + DbStruct.SQLRealEscapeString(alias) + "'";
            SqlQuery += ";";
            return SqlQuery;
        }
        //Офисы и банкоматы
        private string _sqlGetOrgPointsTotalCount(int orgId, VM_OrgsPointsFilters filter)
        {
            string SqlConds = _sqlGetPointFilter(orgId, filter);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.COUNT + "(*)";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.OrgPoints.TABLENAME;
            if (!String.IsNullOrEmpty(SqlConds))
            {
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += SqlConds;
            }
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetOrgPoints(int orgId, VM_OrgsPointsFilters filter, int offset, int limit)
        {
            string SqlConds = _sqlGetPointFilter(orgId, filter);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " t1.* " + DbStruct.SE.FROM + "(";
            SqlQuery += DbStruct.SE.SELECT + " ROW_NUMBER() OVER(";
            SqlQuery += DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Title;
            SqlQuery += ") AS RowNumber, *";            
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.OrgPoints.TABLENAME;
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
        private string _sqlGetPointFilter(int orgId, VM_OrgsPointsFilters filter)
        {
            string SqlConds = String.Empty;
            SqlConds += " " + DbStruct.SE.AND + " ";
            SqlConds += DbStruct.OrgPoints.FIELDS.OrgId;
            SqlConds += " = " + orgId.ToString();
            if (filter.IsActive != EnumBoolType.None)
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.OrgPoints.FIELDS.IsActive;
                if (filter.IsActive == EnumBoolType.True)
                    SqlConds += " = 1";
                else
                    SqlConds += " = 0";
            }
            if (!String.IsNullOrEmpty(filter.Title))
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.OrgPoints.FIELDS.Title;
                SqlConds += " " + DbStruct.SE.LIKE + " ";
                SqlConds += "'%" + DbStruct.SQLRealEscapeString(filter.Title) + "%'";
            }
            if (!String.IsNullOrEmpty(filter.Address))
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.OrgPoints.FIELDS.Address;
                SqlConds += " " + DbStruct.SE.LIKE + " ";
                SqlConds += "'%" + DbStruct.SQLRealEscapeString(filter.Address) + "%'";
            }
            if (filter.PointType != EnumOrgPointType.None)
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.OrgPoints.FIELDS.PointType;
                SqlConds += " = " + ((int)filter.PointType).ToString();
            }
            if (filter.RegionId != Guid.Empty)
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.OrgPoints.FIELDS.RegionId;
                SqlConds += " = '" + filter.RegionId.ToString() + "'";
            }
            return String.IsNullOrEmpty(SqlConds) ? String.Empty : SqlConds.Substring(5);
        }        
        private string _sqlGetOrgPoint(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.OrgPoints.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetOrgPoints(int orgId, Guid RegionUd)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.OrgPoints.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.OrgId;
            SqlQuery += " = " + orgId.ToString();
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.IsActive;
            SqlQuery += " =1";
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlCreateOrgPoint(VM_OrgPoint orgPoint)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.INSERT + " ";
            SqlQuery += DbStruct.OrgPoints.TABLENAME;
            SqlQuery += "(";
            SqlQuery += DbStruct.OrgPoints.FIELDS.OrgId + ",";            
            SqlQuery += DbStruct.OrgPoints.FIELDS.Title + ",";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Alias + ",";
            SqlQuery += DbStruct.OrgPoints.FIELDS.RegionId + ",";
            SqlQuery += DbStruct.OrgPoints.FIELDS.PointType + ",";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Address + ",";
            SqlQuery += DbStruct.OrgPoints.FIELDS.AddressDopInfo + ",";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Phones + ",";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Schedule + ",";
            SqlQuery += DbStruct.OrgPoints.FIELDS.DopInfo + ",";
            SqlQuery += DbStruct.OrgPoints.FIELDS.IsActive;            
            SqlQuery += ")";
            SqlQuery += " " + DbStruct.SE.VALUES + "(";
            SqlQuery += orgPoint.OrgId.ToString() + ",";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(orgPoint.Title) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(orgPoint.Alias) + "',";
            SqlQuery += "'" + orgPoint.RegionId.ToString() + "',";
            SqlQuery += ((int)orgPoint.PointType).ToString() + ",";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(orgPoint.Address) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(orgPoint.AddressDopInfo) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(orgPoint.Phones) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(orgPoint.Schedule) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(orgPoint.DopInfo) + "',";            
            SqlQuery += orgPoint.IsActive ? "1" : "0";            
            SqlQuery += ")";
            SqlQuery += ";";
            SqlQuery += DbStruct.SE.SELECT_SCOPE_IDENTITY;
            return SqlQuery;
        }
        private string _sqlUpdateOrgPoint(int id, VM_OrgPoint orgPoint)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.OrgPoints.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.OrgId + " = ";
            SqlQuery += orgPoint.OrgId.ToString() + ",";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Title + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(orgPoint.Title) + "',";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Alias + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(orgPoint.Alias) + "',";
            SqlQuery += DbStruct.OrgPoints.FIELDS.RegionId + " = ";
            SqlQuery += "'" + orgPoint.RegionId.ToString() + "',";
            SqlQuery += DbStruct.OrgPoints.FIELDS.PointType + " = ";
            SqlQuery += ((int)orgPoint.PointType).ToString() + ",";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Address + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(orgPoint.Address) + "',";
            SqlQuery += DbStruct.OrgPoints.FIELDS.AddressDopInfo + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(orgPoint.AddressDopInfo) + "',";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Phones + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(orgPoint.Phones) + "',";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Schedule + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(orgPoint.Schedule) + "',";
            SqlQuery += DbStruct.OrgPoints.FIELDS.DopInfo + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(orgPoint.DopInfo) + "',";
            SqlQuery += DbStruct.OrgPoints.FIELDS.IsActive + " = ";                       
            SqlQuery += orgPoint.IsActive ? "1" : "0";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlDeleteOrgPoint(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.DELETE + " ";
            SqlQuery += DbStruct.OrgPoints.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlSetOrgPointActive(int id, bool action)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.OrgPoints.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.IsActive + " = ";
            SqlQuery += action ? "1" : "0";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetPointAliasExists(string alias)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Alias;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.OrgPoints.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.OrgPoints.FIELDS.Alias;
            SqlQuery += " = '" + DbStruct.SQLRealEscapeString(alias) + "'";
            SqlQuery += ";";
            return SqlQuery;
        }
        //Отзывы
        private string _sqlCreateReview(VM_Review review)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.INSERT + " ";
            SqlQuery += DbStruct.Reviews.TABLENAME;
            SqlQuery += "(";
            SqlQuery += DbStruct.Reviews.FIELDS.IsActive + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.OrgId + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.UserId + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.UserName + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.ReviewTitle + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.ReviewText + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.Rating + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.ReviewState + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.CreatedAt;            
            SqlQuery += ")";
            SqlQuery += " " + DbStruct.SE.VALUES + "(";
            SqlQuery += (review.IsActive ? "1" : "0") + ",";
            SqlQuery += review.OrgId + ",";
            SqlQuery += review.UserId + ",";
            SqlQuery += "'" + review.UserName.ToString() + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(review.ReviewTitle) + "',";            
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(review.ReviewText) + "',";
            SqlQuery += review.Rating.ToString() + ",";
            SqlQuery += ((int)review.Confirmed).ToString() + ",";
            SqlQuery += DbStruct.SE.GETDATE;
            SqlQuery += ")";
            SqlQuery += ";";
            SqlQuery += DbStruct.SE.SELECT_SCOPE_IDENTITY;
            return SqlQuery;
        }
        private string _sqlGetOrgReviewsTotalCount(int? orgId, VM_OrgsReviewsFilters filter)
        {
            string SqlConds = _sqlGetReviewFilter(orgId, filter);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.COUNT + "(*)";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Reviews.TABLENAME;
            if (!String.IsNullOrEmpty(SqlConds))
            {
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += SqlConds;
            }
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetOrgReviews(int? orgId, VM_OrgsReviewsFilters filter, int offset, int limit)
        {
            string SqlConds = _sqlGetReviewFilter(orgId, filter);
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " t1.* " + DbStruct.SE.FROM + "(";
            SqlQuery += DbStruct.SE.SELECT + " ROW_NUMBER() OVER(";
            SqlQuery += DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ") AS RowNumber, *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Reviews.TABLENAME;
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
        private string _sqlGetReviewFilter(int? orgId, VM_OrgsReviewsFilters filter)
        {
            string SqlConds = String.Empty;
            if (orgId != null)
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.Reviews.FIELDS.OrgId;
                SqlConds += " = " + orgId.ToString();
            }
            if (filter.IsActive != EnumBoolType.None)
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.Reviews.FIELDS.IsActive;
                if (filter.IsActive == EnumBoolType.True)
                    SqlConds += " = 1";
                else
                    SqlConds += " = 0";
            }
            if (!String.IsNullOrEmpty(filter.ReviewText))
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.Reviews.FIELDS.ReviewText;
                SqlConds += " " + DbStruct.SE.LIKE + " ";
                SqlConds += "'%" + DbStruct.SQLRealEscapeString(filter.ReviewText) + "%'";
            }
            if (filter.Confirmed != EnumBoolType.None)
            {
                SqlConds += " " + DbStruct.SE.AND + " ";
                SqlConds += DbStruct.Reviews.FIELDS.ReviewState;
                if (filter.Confirmed == EnumBoolType.True)
                    SqlConds += " = 1";
                else
                    SqlConds += " = 0";
            }
            return String.IsNullOrEmpty(SqlConds) ? String.Empty : SqlConds.Substring(5);
        }        
        private string _sqlGetLastReview(int? org_id, int? categoryId, int recCount)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.TOP + " " + recCount.ToString() + " *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Reviews.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";            
            SqlQuery += DbStruct.Reviews.FIELDS.IsActive;
            SqlQuery += " = 1";
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.ReviewState;
            SqlQuery += " = 1"; 
            if (org_id != null)
            {
                SqlQuery += " " + DbStruct.SE.AND + " ";
                SqlQuery += DbStruct.Reviews.FIELDS.OrgId;
                SqlQuery += " = " + org_id.ToString();
            }
            else if (categoryId != null)
            {
                SqlQuery += " " + DbStruct.SE.AND + " ";
                SqlQuery += DbStruct.Reviews.FIELDS.OrgId;
                SqlQuery += " " + DbStruct.SE.IN + " (";
                SqlQuery += DbStruct.SE.SELECT + " ";
                SqlQuery += DbStruct.Orgs.FIELDS.Id;
                SqlQuery += " " + DbStruct.SE.FROM + " ";
                SqlQuery += DbStruct.Orgs.TABLENAME;
                SqlQuery += " " + DbStruct.SE.WHERE + " ";
                SqlQuery += DbStruct.Orgs.FIELDS.CategoryId;
                SqlQuery += " = " + categoryId.ToString();
                SqlQuery += ")";
            }
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetOrgReviewsTotalCount(int org_id)
        {            
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.SE.COUNT + "(*)";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Reviews.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.OrgId;
            SqlQuery += " = " + org_id.ToString();
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.IsActive;
            SqlQuery += " = 1";
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.ReviewState;
            SqlQuery += " = 1"; 
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetReviews(int org_id, int offset, int limit)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " t1.* " + DbStruct.SE.FROM + "(";
            SqlQuery += DbStruct.SE.SELECT + " ROW_NUMBER() OVER(";
            SqlQuery += DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.CreatedAt;
            SqlQuery += " " + DbStruct.SE.DESC;
            SqlQuery += ") AS RowNumber, *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Reviews.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.OrgId;
            SqlQuery += " = " + org_id.ToString();
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.IsActive;
            SqlQuery += " = 1";
            SqlQuery += " " + DbStruct.SE.AND + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.ReviewState;
            SqlQuery += " = 1";
            SqlQuery += ") AS t1";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += "t1.RowNumber BETWEEN {0} AND {1}";
            SqlQuery += ";";
            return String.Format(SqlQuery, offset.ToString(), limit.ToString());
        }
        private string _sqlGetReview(int review_id, bool onlyPublished = false)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";            
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Reviews.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.Id;
            SqlQuery += " = " + review_id.ToString();
            if (onlyPublished)
            {
                SqlQuery += " " + DbStruct.SE.AND + " ";
                SqlQuery += DbStruct.Reviews.FIELDS.IsActive;
                SqlQuery += " = 1";
                SqlQuery += " " + DbStruct.SE.AND + " ";
                SqlQuery += DbStruct.Reviews.FIELDS.ReviewState;
                SqlQuery += " = 1";
            }
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlCreateOrgReview(VM_Review review)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.INSERT + " ";
            SqlQuery += DbStruct.Reviews.TABLENAME;
            SqlQuery += "(";
            SqlQuery += DbStruct.Reviews.FIELDS.OrgId + ",";            
            SqlQuery += DbStruct.Reviews.FIELDS.ReviewText + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.ReviewTitle + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.UserId + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.UserName + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.Rating + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.CreatedAt + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.ReviewState + ",";            
            SqlQuery += DbStruct.Reviews.FIELDS.IsActive;
            SqlQuery += ")";
            SqlQuery += " " + DbStruct.SE.VALUES + "(";
            SqlQuery += review.OrgId.ToString() + ",";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(review.ReviewText) + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(review.ReviewTitle) + "',";
            SqlQuery += "'" + review.UserId.ToString() + "',";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(review.UserName) + "',";
            SqlQuery += review.Rating.ToString() + ",";
            SqlQuery += DbStruct.SE.GETDATE + ",";
            SqlQuery += ((int)review.Confirmed).ToString() + ",";
            SqlQuery += review.IsActive ? "1" : "0";
            SqlQuery += ")";
            SqlQuery += ";";
            SqlQuery += DbStruct.SE.SELECT_SCOPE_IDENTITY;
            return SqlQuery;
        }
        private string _sqlUpdateOrgReview(int id, VM_Review review)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.Reviews.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";            
            SqlQuery += DbStruct.Reviews.FIELDS.ReviewText + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(review.ReviewText) + "',";
            SqlQuery += DbStruct.Reviews.FIELDS.ReviewTitle + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(review.ReviewTitle) + "',";
            SqlQuery += DbStruct.Reviews.FIELDS.UserId + " = ";
            SqlQuery += "'" + review.UserId.ToString() + "',";
            SqlQuery += DbStruct.Reviews.FIELDS.UserName + " = ";
            SqlQuery += "'" + DbStruct.SQLRealEscapeString(review.UserName) + "',";
            SqlQuery += DbStruct.Reviews.FIELDS.Rating + " = ";
            SqlQuery += review.Rating.ToString() + ",";            
            SqlQuery += DbStruct.Reviews.FIELDS.ReviewState + " = ";
            SqlQuery += ((int)review.Confirmed).ToString() + ",";
            SqlQuery += DbStruct.Reviews.FIELDS.IsActive + " = ";            
            SqlQuery += review.IsActive ? "1" : "0";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlDeleteOrgReview(int id)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.DELETE + " ";
            SqlQuery += DbStruct.Reviews.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.Id;
            SqlQuery += " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlSetOrgReviewActive(int id, bool action)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.Reviews.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.IsActive + " = ";
            SqlQuery += action ? "1" : "0";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlSetOrgReviewComfirmation(int id, bool action)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.UPDATE + " ";
            SqlQuery += DbStruct.Reviews.TABLENAME;
            SqlQuery += " " + DbStruct.SE.SET + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.ReviewState + " = ";
            SqlQuery += action ? "1" : "0";
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Reviews.FIELDS.Id + " = " + id.ToString();
            SqlQuery += ";";
            return SqlQuery;
        }        
        //Категории
        private string _sqlGetOrgCategoryByAlias(string alias)
        { 
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";            
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.OrgCategories.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.OrgCategories.FIELDS.Alias;
            SqlQuery += " = '" + alias + "'";
            SqlQuery += ";";
            return SqlQuery;
        }
        private string _sqlGetOrgCategories()
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.OrgCategories.FIELDS.Id + ",";
            SqlQuery += DbStruct.OrgCategories.FIELDS.Alias + ",";
            SqlQuery += DbStruct.OrgCategories.FIELDS.Title;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.OrgCategories.TABLENAME;
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.OrgCategories.FIELDS.Title;
            SqlQuery += ";";
            return SqlQuery;
        }
        #endregion

        #region ДОПОЛНИТЕЛЬНО
        //Ссылки
        protected string GenerateAlias(string title, string region)
        {
            string result = String.Empty;
            try
            {
                result = TransliterationManager.Front(title + (String.IsNullOrEmpty(region) ? "" : " " + region));
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
                log.Error(LastError);
                return null;
            }
        }
        protected string GeneratePointAlias(string title, string region)
        {
            string result = String.Empty;
            try
            {
                result = TransliterationManager.Front(title + (String.IsNullOrEmpty(region) ? "" : " " + region));
                if (_dbPointAliasExists(result))
                {
                    string res = result;
                    int i = 1;
                    do
                    {
                        res = result + "-" + i.ToString();
                        i++;
                    }
                    while (_dbPointAliasExists(res));
                    result = res;
                }
                return result;
            }
            catch (Exception ex)
            {
                LastError = "Ошибка во время генерации ссылки! " + ex.ToString();
                log.Error(LastError);
                return null;
            }
        }
        //Регионы
        protected string GetRegionName(Guid regionId)
        {
            //object regions = Cache["regions"];
            return "Воронежская область";
        }
        //Типы представительств организаций
        protected string GetPointTypeAsString(EnumOrgPointType pointType)
        {
            switch (pointType)
            { 
                case EnumOrgPointType.Office:
                    return "Офис";
                case EnumOrgPointType.ATM:
                    return "Банкомат";
                default:
                    return "Не определен";
            }
        }
        //Файлы
        private string _saveOrgIcon(HttpPostedFileBase icon, string alias)
        {
            if (icon != null)
            {
                try
                {
                    string iconPath = AppDomain.CurrentDomain.BaseDirectory + _imagePath;
                    string filename = Path.GetFileName(icon.FileName);
                    if (filename != null)
                    {
                        string fileFormat = ".jpg";
                        if (filename.LastIndexOf(".") != -1)
                        {
                            fileFormat = filename.Substring(filename.LastIndexOf("."));
                        }
                        icon.SaveAs(Path.Combine(iconPath, alias + fileFormat));
                        return Path.Combine(_imagePath, alias + fileFormat);
                    }
                    else
                    {
                        //Логирование
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    LastError = ex.ToString();
                    log.Error(LastError);
                    return null;
                }
            }
            else
                return null;
        }
        //Категории
        private int _getOrgCategoryIdByAlias(string alias)
        {
            switch (alias)
            {
                case "banki":
                    return 1;
                case "mfo":
                    return 2;
                case "investicionnie-kompanii":
                    return 3;
                case "strahovie-kompanii":
                    return 5;
                case "lombardy":
                    return 6;
                case "тза":
                    return 7;
                case "kollektorskie-kompanii":
                    return 8;
                case "juridicheskie-kompanii":
                    return 9;
                default:
                    return -1;
            }
        }
        private string _getOrgCategoryAliasById(int id)
        {
            switch (id)
            {
                case 1: return "banki";
                case 2: return "mfo";
                case 3: return "investicionnie-kompanii";
                case 5: return "strahovie-kompanii";
                case 6: return "lombardy";
                case 7: return "тза";
                case 8: return "kollektorskie-kompanii";
                case 9: return "juridicheskie-kompanii";
                default:
                    return "";
            }
        }
        private string _getOrgCategoryNameById(int id)
        {
            switch (id)
            {
                case 1: return "Банки";
                case 2: return "МФО";
                case 3: return "Инвестиционные компании";
                case 5: return "Страховые компании";
                case 6: return "Ломбарды";
                case 7: return "НПФ";
                case 8: return "Коллекторские компании";
                case 9: return "Юридические компании";
                default:
                    return "";
            }
        }        
        #endregion

        #endregion
    }
}