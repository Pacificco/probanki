using Bankiru.Models.DataBase;
using Bankiru.Models.Domain.Club;
using Bankiru.Models.Infrastructure;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Users
{
    public class UserManager
    {
        private string _lastError = "";
        public string LastError { get { return _lastError; } }
        public static readonly ILog log = LogManager.GetLogger(typeof(UserManager));

        public VM_Users GetUsers(VM_UsersFilters filter, int page = 1)
        {
            VM_Users _users = new VM_Users();
            _users.Filters.Assign(filter);
            _users.PagingInfo.SetData(page, _getUsersTotalCount(filter));
            if (_users.PagingInfo.TotalItems == -1) return null;
            _users.PagingInfo.CurrentPage = page;
            _users.Items = _getUsers(filter, _users.PagingInfo.GetNumberFrom(), _users.PagingInfo.GetNumberTo());
            return _users;
        }
        public VM_User GetUser(int userId, bool allInfo = false)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UserView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UserView.Params.Id, userId);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    VM_User user = new VM_User();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                for (int j = 0; j < reader.FieldCount; j++)
                                    user.SetFieldValue(reader.GetName(j), reader.GetValue(j));
                            }
                        }
                    }
                    if (allInfo)
                    {
                        //user.ForecastInfo = GetUserProfiletInfo(userId);
                        //if (user.ForecastInfo == null)
                        //    return null;
                    }
                    return user;
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время загрузки пользователя!\n" + ex.ToString();
                    log.Error(_lastError);
                    return null;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }            
        }
        public VM_UserTariffInfo GetUserTariffInfo(int userId)
        {
            VM_UserTariffInfo tariff = new VM_UserTariffInfo();
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UserTariffInfoView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UserTariffInfoView.Params.UserId, userId);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {                                
                                for (int j = 0; j < reader.FieldCount; j++)
                                    tariff.SetFieldValue(reader.GetName(j), reader.GetValue(j));
                            }
                        }
                    }
                    return tariff;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.UserTariffInfoView.Name,
                        ex.ToString());
                    log.Error(_lastError);
                    return null;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
        }
        public List<VM_ForecastUser> GetUserForecastsForMonth(int userId)
        {
            List<VM_ForecastUser> result = new List<VM_ForecastUser>();
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UserForecastsForMonthView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UserForecastsForMonthView.Params.UserId, userId);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {                            
                            VM_ForecastUser uf = null;
                            while (reader.Read())
                            {
                                uf = new VM_ForecastUser();

                                //Пользователь не принимает участие в текущем прогнозе
                                if (reader.IsDBNull(0))
                                {
                                    uf.Value = null;
                                    uf.ValueDate = null;
                                    uf.User.Clear();
                                }
                                else
                                {
                                    uf.User.Id = reader.GetInt32(0);
                                    uf.User.Nic = reader.GetString(1);
                                    uf.Value = reader.GetDouble(2);
                                    uf.ValueDate = reader.GetDateTime(3);
                                }
                                uf.Forecast.Id = reader.GetInt32(4);
                                uf.Forecast.ForecastDate = reader.GetDateTime(5);
                                uf.Forecast.SubjectId = reader.GetByte(6);
                                //for (int j = 4; j < reader.FieldCount; j++)
                                //    uf.Forecast.SetFieldValue(reader.GetName(j), reader.GetValue(j));
                                result.Add(uf);
                            }
                        }
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.UserForecastsForMonthView.Name,
                        ex.ToString());
                    log.Error(_lastError);
                    return null;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
        }
        public VM_UserProfileInfo GetUserProfiletInfo(int userId)
        {
            VM_UserProfileInfo profile = new VM_UserProfileInfo();

            //Загружаем информацию о пользователе
            if (userId == -1)
            {
                profile.User = null;
            }
            else
            {
                profile.User = GetUser(userId);
                if (profile.User == null)
                    return null;
            }

            //Загружаем информацию о прогнозах
            profile.ForecastsForMonth = GetUserForecastsForMonth(userId);

            //Загружаем предметы прогнозов
            ForecastManager manager = new ForecastManager();
            profile.ForecastSubjects = manager.GetForecastSubjects();

            //Информация о тарифах
            profile.TariffInfo = GetUserTariffInfo(userId);

            return profile;
        }
        public List<VM_UserBalanceHistoryItem> GetUserBalanceHistory(int userId)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UserBalanceHistoryView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UserBalanceHistoryView.Params.UserId, userId);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    List<VM_UserBalanceHistoryItem> history = new List<VM_UserBalanceHistoryItem>();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            VM_UserBalanceHistoryItem historyItem = null;
                            while (reader.Read())
                            {
                                historyItem = new VM_UserBalanceHistoryItem();
                                for (int j = 0; j < reader.FieldCount; j++)
                                    historyItem.SetFieldValue(reader.GetName(j), reader.GetValue(j));
                                history.Add(historyItem);
                            }
                        }
                    }
                    return history;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                         DbStruct.PROCEDURES.UserBalanceHistoryView.Name,
                         ex.ToString());
                    log.Error(_lastError);
                    return null;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            } 
        }
        public bool AddBalance(VM_UserAddBalance info)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.AddBalance.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.AddBalance.Params.UserId, info.UserId);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.AddBalance.Params.TariffId, (byte)info.TariffId);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.AddBalance.Params.Sum, info.Sum);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.AddBalance.Params.PeriodId, info.PeriodId);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.AddBalance.Params.Comment, info.Comment);

            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    if (command.ExecuteNonQuery() == 1)
                    {
                        _lastError = "Ошибка во время пополнения баланса пользователя! Ошибка во время выполнения хранимой процедуры AddBalance.";
                        log.Error(_lastError);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                    //switch (result)
                    //{
                    //    case 2:
                    //        _lastError = "Ошибка во время пополнения баланса пользователя! Пользователь не найден.";
                    //        log.Error(_lastError);
                    //        return false;
                    //    case 1:
                    //        _lastError = "Ошибка во время пополнения баланса пользователя! Ошибка во время выполнения хранимой процедуры AddBalance.";
                    //        log.Error(_lastError);
                    //        return false;
                    //    case -1:
                    //        _lastError = "Ошибка во время пополнения баланса пользователя! Ошибка во время выполнения хранимой процедуры AddBalance.";
                    //        log.Error(_lastError);
                    //        return false;
                    //    case 0:
                    //        return true;
                    //    default:
                    //        return true;
                    //}
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}! {1}",
                        DbStruct.PROCEDURES.AddBalance.Name,
                        ex.ToString());
                    log.Error(_lastError);
                    return false;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            } 
        }

        #region ЧАСТНЫЕ МЕТОДЫ
        private int _getUsersTotalCount(VM_UsersFilters filter)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UsersCount.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            if(filter.IsActive == EnumBoolType.None)
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.IsActive, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.IsActive,
                    filter.IsActive == EnumBoolType.True ? 1 : 0);
            if(String.IsNullOrEmpty(filter.Nic))
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.Nic, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.Nic, filter.Nic);
            if (String.IsNullOrEmpty(filter.Email))
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.Email, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.Email, filter.Email);
            if (String.IsNullOrEmpty(filter.Name))
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.Name, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersCount.Params.Name, filter.Name);
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    object objCount = command.ExecuteScalar();
                    if(objCount == null)
                        return -1;
                    else 
                        return Convert.ToInt32(objCount.ToString());
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время определения общего числа пользователей!\n" + ex.ToString();
                    log.Error(_lastError);
                    return -1;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
        }
        private List<VM_UserItem> _getUsers(VM_UsersFilters filter, int from, int to)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UsersView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            if (filter.IsActive == EnumBoolType.None)
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.IsActive, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.IsActive, 
                    filter.IsActive == EnumBoolType.True ? 1 : 0);
            if (String.IsNullOrEmpty(filter.Nic))
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.Nic, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.Nic, filter.Nic);
            if (String.IsNullOrEmpty(filter.Email))
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.Email, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.Email, filter.Email);
            if (String.IsNullOrEmpty(filter.Name))
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.Name, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.Name, filter.Name);

            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.From, from);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UsersView.Params.To, to);

            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    List<VM_UserItem> items = new List<VM_UserItem>();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            VM_UserItem item = null;
                            while (reader.Read())
                            {
                                item = new VM_UserItem();
                                for (int j = 0; j < reader.FieldCount; j++)
                                    item.SetFieldValue(reader.GetName(j), reader.GetValue(j));
                                    items.Add(item);
                            }
                        }
                    }
                    return items;
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время загрузки пользователей!\n" + ex.ToString();
                    log.Error(_lastError);
                    return null;
                }
                finally
                {                    
                    if (command != null)
                        command.Dispose();
                }
            }
        }
        #endregion
    }
}