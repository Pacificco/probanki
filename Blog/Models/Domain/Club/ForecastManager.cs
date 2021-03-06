﻿using Bankiru.Models.DataBase;
using Bankiru.Models.Domain.Users;
using Bankiru.Models.Helpers;
using Bankiru.Models.Infrastructure;
using Bankiru.Models.Security;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Club
{
    public class ForecastManager
    {
        private string _lastError;
        public string LastError { get { return _lastError; } }
        public static readonly ILog log = LogManager.GetLogger(typeof(ForecastManager));

        public ForecastManager()
        {
            _lastError = "";
        }

        #region ПРОГНОЗЫ
        /// <summary>
        /// Создает новые дни прогнозов
        /// </summary>
        /// <param name="forecastDate">Дата прогнозов</param>
        /// <returns>Логическое значение</returns>
        public bool CreateForecasts(VM_Forecast forecast)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.CreateForecast.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CreateForecast.Params.ForecastDate,
                new DateTime(forecast.ForecastDate.Year, forecast.ForecastDate.Month, forecast.ForecastDate.Day, 23, 59, 59, 0));
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CreateForecast.Params.SubjectId, forecast.Subject.Id);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CreateForecast.Params.WinAmount, forecast.WinAmount);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CreateForecast.Params.UserId, 2);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    if(command.ExecuteNonQuery() == 1)                    
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.CreateForecast.Name, ex.ToString());
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
        /// <summary>
        /// Обновляет прогноз в базе данных
        /// </summary>
        /// <param name="id">Идентификатор прогноза</param>
        /// <param name="forecast">Прогноз</param>
        /// <returns>Логическое значение</returns>
        public bool UpdateForecast(int id, VM_Forecast forecast)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.EditForecast.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.EditForecast.Params.ForecastDate,
                new DateTime(forecast.ForecastDate.Year, forecast.ForecastDate.Month, forecast.ForecastDate.Day, 23, 59, 59, 0));
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.EditForecast.Params.SubjectId, forecast.Subject.Id);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.EditForecast.Params.WinAmount, forecast.WinAmount);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.EditForecast.Params.UserId, 2);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.EditForecast.Params.Id, id);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    if (command.ExecuteNonQuery() == 1)
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.EditForecast.Name, ex.ToString());
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
        /// <summary>
        /// Удаялет прогноз
        /// </summary>
        /// <param name="id">Идентификатор прогноза</param>
        /// <returns>Логическое значение</returns>
        public bool DeleteForecast(int id)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.DeleteForecast.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DeleteForecast.Params.Id, id);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    if (command.ExecuteNonQuery() == 1)
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.DeleteForecast.Name, ex.ToString());
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
        /// <summary>
        /// Выполняет проверку участвует ли пользователь в прогнозе
        /// </summary>
        /// <param name="forecastId">Идентификатор прогноза</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Логическое значение</returns>
        public bool? ForecastHasUser(int forecastId, int userId)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ForecastHasUser.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastHasUser.Params.ForecastId, forecastId);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastHasUser.Params.UserId, userId);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    if (command.ExecuteNonQuery() == 1)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.ForecastHasUser.Name, ex.ToString());
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
        /// <summary>
        /// Добавляет пользователя к участникам прогноза
        /// </summary>
        /// <param name="forecastId">Идентификатор прогноза</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="value">Значение</param>
        /// <param name="disableState">Статус запрета</param>
        /// <returns>Логическое значение</returns>
        public bool AddUserToForecast(int forecastId, int userId, double value, out EnumDisableAddUserToForecast disableState)
        {
            disableState = EnumDisableAddUserToForecast.Undefined;

            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.AddUserToForecast.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.AddUserToForecast.Params.ForecastId, forecastId);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.AddUserToForecast.Params.UserId, userId);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.AddUserToForecast.Params.ForecastValue, value);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    VM_UserForecastState fs = null;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                fs = new VM_UserForecastState();
                                fs.AddEnable = reader.GetBoolean(0);
                                fs.DisableState = (EnumDisableAddUserToForecast)reader.GetInt32(1);                                
                            }
                        }
                    }
                    if (fs == null)
                        return false;
                    if (fs.AddEnable)
                        return true;
                    disableState = fs.DisableState;
                    return false;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!",
                        DbStruct.PROCEDURES.AddUserToForecast.Name);
                    log.Error(String.Format("Ошибка во время выполнения хранимой процедуры {0}!",
                        DbStruct.PROCEDURES.AddUserToForecast.Name, ex.ToString()));
                    return false;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
        }
        /// <summary>
        /// Закрывает прогнозы по указанной дате
        /// </summary>
        /// <param name="forecastDate">Дата окончания прогноза</param>
        /// <returns>Логическое значение</returns>
        public bool CloseForecast(int forecastId, double factValue, DateTime newForecastDate)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.CloseForecast.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CloseForecast.Params.FactValue, factValue);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CloseForecast.Params.ForecastId, forecastId);
            DateTime forecastDate = new DateTime(newForecastDate.Year, newForecastDate.Month, newForecastDate.Day, 23, 59, 59);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CloseForecast.Params.NewForecastDate, forecastDate);
                //DbHelper.FormatSQLDateTime(newForecastDate, SQLDateFormat.sdfDateTime_TimeMax));
            SqlParameter returnValue = new SqlParameter();
            returnValue.DbType = DbType.Int32;
            returnValue.Direction = ParameterDirection.ReturnValue;
            returnValue.Value = 1;
            command.Parameters.Add(returnValue);
            command.CommandTimeout = 30;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    command.ExecuteNonQuery();
                    if ((int)returnValue.Value == 1)
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.CloseForecast.Name, ex.ToString());
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
        /// <summary>
        /// Возвращает список прогнозов
        /// </summary>
        /// <param name="IsClosed">Закрытые</param>
        /// <param name="subject">Предмет прогноза</param>
        /// <returns>Список прогнозов</returns>
        public List<VM_Forecast> GetCurrentForecasts(bool loadUsers, string subjectAlias = "all")
        {
            VM_ForecastSubject subject = null;
            if (subjectAlias == "all")
            {
                subject = new VM_ForecastSubject();
            }
            else
            {
                subject = _getForecastSubject(subjectAlias);
                if (subject == null)
                {
                    return new List<VM_Forecast>();
                }
            }

            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ForecastsView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastsView.Params.IsClosed, false);
            if (subject.Id <= 0)
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastsView.Params.SubjectId, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastsView.Params.SubjectId, subject.Id);
            command.CommandTimeout = 15;

            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    List<VM_Forecast> forecasts = new List<VM_Forecast>();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            VM_Forecast f = null;
                            while (reader.Read())
                            {
                                f = new VM_Forecast();
                                for (int j = 0; j < reader.FieldCount; j++)
                                    f.SetFieldValue(reader.GetName(j), reader.GetValue(j));
                                forecasts.Add(f);
                            }
                        }
                    }

                    if (forecasts.Count > 0)
                    {
                        //Предмет прогноза
                        List<VM_ForecastSubject> subjects = _getForecastSubjects();
                        if (subjects == null)
                            return null;
                        foreach (VM_Forecast f in forecasts)
                            f.Subject.Assign(subjects.FirstOrDefault(s => s.Id == f.Subject.Id));

                        //Пользователи
                        if (loadUsers)
                        {
                            int[] ids = (from f in forecasts select f.Id).ToArray<int>();
                            List<VM_ForecastUser> users = _getCurrentForecastUsers(ids);
                            if (users != null && users.Count > 0)
                            {
                                List<VM_ForecastUser> fUsers = null;
                                foreach (VM_Forecast f in forecasts)
                                {
                                    fUsers = users.FindAll(u => u.Forecast.Id == f.Id);
                                    if (fUsers != null)
                                        foreach (VM_ForecastUser u in fUsers)
                                            f.Users.Add(u);
                                }
                            }
                        }
                    }
                    return forecasts;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.ForecastsView.Name, ex.ToString());
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
        /// <summary>
        /// Возвращает прогноз по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор прогноза</param>
        /// <returns>Прогноз</returns>
        public VM_Forecast GetForecast(int id)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ForecastView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastView.Params.Id, id);
            command.CommandTimeout = 15;
            VM_Forecast forecast = new VM_Forecast();
            lock (GlobalParams._DBAccessLock)
            {
                try
                {                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            if (reader.Read())
                            {                                
                                for (int j = 0; j < reader.FieldCount; j++)
                                    forecast.SetFieldValue(reader.GetName(j), reader.GetValue(j));                                
                            }
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.ForecastView.Name, ex.ToString());
                    log.Error(_lastError);
                    return null;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
                forecast.Users = GetUsersByForecastsId(id);
                return forecast;
            }
        }
        /// <summary>
        /// Возвращает пользователей, принявших участие в прогнозе
        /// </summary>
        /// <param name="id">Идентификатор прогноза</param>
        /// <returns>Список пользователей</returns>
        public List<VM_ForecastUser> GetUsersByForecastsId(int id)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ForecastUsers.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastUsers.Params.Id, id);
            command.CommandTimeout = 15;
            List<VM_ForecastUser> users = new List<VM_ForecastUser>();
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
                                uf.Value = reader.GetDouble(0);
                                uf.ValueDate = reader.GetDateTime(1);
                                for (int j = 2; j < reader.FieldCount; j++)
                                    uf.User.SetFieldValue(reader.GetName(j), reader.GetValue(j));
                                users.Add(uf);
                            }
                        }
                    }
                    return users;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.ForecastUsers.Name, ex.ToString());
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
        /// <summary>
        /// Возвращает прогнозы по фильтру
        /// </summary>
        /// <param name="id">Фильтр</param>
        /// <returns>Прогнозы</returns>
        public VM_Forecasts GetForecasts(VM_ForecastsFilters filter, int page)
        {
            VM_Forecasts forecasts = new VM_Forecasts();
            forecasts.Filters.Assign(filter);
            forecasts.PagingInfo.ItemsPerPage = 20;
            forecasts.PagingInfo.CurrentPage = page;
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ForecastsList.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            if (filter.IsArchive == EnumBoolType.None)
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastsList.Params.IsClosed, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastsList.Params.IsClosed, filter.IsArchive == EnumBoolType.True ? true : false);
            if (filter.SubjectId > 0)
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastsList.Params.SubjectId, filter.SubjectId);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastsList.Params.SubjectId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastsList.Params.RowBegin, forecasts.PagingInfo.GetNumberFrom());
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastsList.Params.RowEnd, forecasts.PagingInfo.GetNumberTo());

            SqlParameter rowCount = command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastsList.Params.RowTotalCount, SqlDbType.Int);
            rowCount.Direction = ParameterDirection.InputOutput;
            
            command.CommandTimeout = 15;                        
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            VM_Forecast f = null;
                            while (reader.Read())
                            {
                                f = new VM_Forecast();
                                for (int j = 0; j < reader.FieldCount; j++)
                                    f.SetFieldValue(reader.GetName(j), reader.GetValue(j));
                                f.Subject.Name = reader.GetString(reader.GetOrdinal("SubjectName"));
                                forecasts.Items.Add(f);
                            }
                        }
                    }
                    forecasts.PagingInfo.SetData(page, Convert.IsDBNull(rowCount.Value) ? -1 : (int)rowCount.Value);
                    return forecasts;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.ForecastsList.Name, ex.ToString());
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
        /// <summary>
        /// Возвращает список последних победителей
        /// </summary>
        /// <returns>Список прогнозов и победителей</returns>
        public List<VM_ForecastUser> GetLastForecastWinners()
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.LastForecastWinnersView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;            
            command.CommandTimeout = 15;
            List<VM_ForecastUser> result = new List<VM_ForecastUser>();
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

                                // Прогноз                              
                                for (int j = 3; j < reader.FieldCount; j++)
                                    uf.Forecast.SetFieldValue(reader.GetName(j), reader.GetValue(j));

                                // Победитель
                                if (reader.IsDBNull(0))
                                {
                                    uf.Forecast.Winner.Clear();
                                }
                                else
                                {
                                    //uf.Forecast.Winner.Id = reader.GetInt32(0);
                                    uf.Forecast.Winner.Nic = reader.GetString(0);
                                    uf.Forecast.Winner.Avatar = reader.GetString(1);
                                    uf.Forecast.Winner.Rang = reader.GetString(2);
                                }                                
                                result.Add(uf);
                            }
                        }
                    }
                    if (result.Count > 0)
                    {
                        //Предмет прогноза
                        ForecastManager manager = new ForecastManager();
                        List<VM_ForecastSubject> subjects = manager._getForecastSubjects();
                        if (subjects == null)
                            return null;
                        foreach (VM_ForecastUser f in result)
                            f.Forecast.Subject.Assign(subjects.FirstOrDefault(s => s.Id == f.Forecast.Subject.Id));
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.UserForecastsView.Name,
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
        #endregion

        #region ПОЛЬЗОВАТЕЛИ
        /// <summary>
        /// Возвращает список пользователей
        /// </summary>
        /// <returns>Список пользователей</returns>
        public List<VM_User> GetUsers(int limit)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ClubTopUsersView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.ClubTopUsersView.Params.UsersCount, limit);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    List<VM_User> users = new List<VM_User>();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            VM_User u = null;
                            while (reader.Read())
                            {
                                u = new VM_User();
                                u.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                                u.Nic = reader.GetString(reader.GetOrdinal("Nic"));
                                u.Avatar = reader.IsDBNull(reader.GetOrdinal("Avatar")) ?
                                    "" : reader.GetString(reader.GetOrdinal("Avatar"));
                                u.ForecastCount = reader.IsDBNull(reader.GetOrdinal("ForecastCount")) ?
                                    0 : reader.GetInt32(reader.GetOrdinal("ForecastCount"));
                                u.WinCount = reader.IsDBNull(reader.GetOrdinal("WinCount")) ?
                                    0 : reader.GetInt32(reader.GetOrdinal("WinCount"));
                                users.Add(u);
                            }
                        }
                    }
                    return users;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.ClubTopUsersView.Name, ex.ToString());
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
        /// <summary>
        /// Возвращает состояние пользователя при добавлении в указанный прогноз
        /// </summary>
        /// <param name="forecastId">Идентификатор прогноза</param>
        /// <returns>Состояние пользователя при добавлении в указанный прогноз</returns>
        public VM_UserForecastState GetUserForecastState(int forecastId, int userId)
        {        
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.AddUserToForecastStateView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.AddUserToForecastStateView.Params.UserId, userId);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.AddUserToForecastStateView.Params.ForecastId, forecastId);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            VM_UserForecastState fs = new VM_UserForecastState();
                            if (reader.Read())
                            {
                                fs.AddEnable = reader.GetBoolean(0);
                                fs.DisableState = (EnumDisableAddUserToForecast)reader.GetInt32(1);
                                return fs;
                            }
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.AddUserToForecastStateView.Name, ex.ToString());
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

        #region ПРЕДМЕТ ПРОГНОЗОВ
        public List<VM_ForecastSubject> GetForecastSubjects()
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ForecastSubjectsView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    List<VM_ForecastSubject> forecasts = new List<VM_ForecastSubject>();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            VM_ForecastSubject fs = null;
                            while (reader.Read())
                            {
                                fs = new VM_ForecastSubject();
                                fs.Id = reader.GetByte(reader.GetOrdinal("Id"));
                                fs.Icon = reader.GetString(reader.GetOrdinal("Icon"));
                                fs.IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                                fs.Name = reader.GetString(reader.GetOrdinal("Name"));
                                fs.Alias = reader.GetString(reader.GetOrdinal("Alias"));
                                fs.Description = reader.GetString(reader.GetOrdinal("Descriptions"));
                                fs.MetaTitle = reader.GetString(reader.GetOrdinal("MetaTitle"));
                                fs.MetaDescriptions = reader.GetString(reader.GetOrdinal("MetaDescriptions"));
                                fs.MetaKeywords = reader.GetString(reader.GetOrdinal("MetaKeywords"));
                                fs.MetaNoFollow = reader.GetBoolean(reader.GetOrdinal("MetaNoFollow"));
                                fs.MetaNoIndex = reader.GetBoolean(reader.GetOrdinal("MetaNoIndex"));
                                fs.Ticker = reader.GetString(reader.GetOrdinal("Ticker"));
                                fs.SourceType = reader.GetString(reader.GetOrdinal("SourceType"));
                                forecasts.Add(fs);
                            }
                        }
                    }
                    return forecasts;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.ForecastSubjectsView.Name, ex.ToString());
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

        #region ВСПОМАГАТЕЛЬНЫЕ МЕТОДЫ
        public VM_ForecastSubject _getForecastSubject(string subjectAlias)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ForecastSubjectView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastSubjectView.Params.SubjectAlias, subjectAlias);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader == null)
                        {
                            _lastError = String.Format("Ошибка во время загрузки предмета прогноза по псевдониму ({0})!\nСервер вернул Null.", subjectAlias);
                            log.Error(LastError);
                            return null;
                        }
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                VM_ForecastSubject forecast = new VM_ForecastSubject()
                                {
                                    Id = reader.GetByte(0),
                                    Alias = reader.GetString(1),
                                    Name = reader.GetString(2),
                                    Description = reader.GetString(3),
                                    Icon = reader.IsDBNull(4) ? String.Empty : reader.GetString(4),
                                    MetaTitle = reader.GetString(5),
                                    MetaDescriptions = reader.GetString(6),
                                    MetaKeywords = reader.GetString(7),
                                    MetaNoFollow = reader.GetBoolean(8),
                                    MetaNoIndex = reader.GetBoolean(9),
                                    IsActive = reader.GetBoolean(10)
                                };
                                return forecast;
                            }
                        }
                        else
                        {
                            _lastError = String.Format("Ошибка во время загрузки предмета прогноза по псевдониму ({0})!\nСервер вернул пустой ответ.", subjectAlias);
                            log.Error(LastError);
                            return null;
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время загрузки предмета прогноза по псевдониму ({0})!\n{1}", subjectAlias, ex.ToString());
                    log.Error(LastError);
                    return null;
                }
                finally
                {                    
                    if (command != null)
                        command.Dispose();
                }
            }            
        }
        public List<VM_ForecastSubject> _getForecastSubjects()
        {
            List<VM_ForecastSubject> subjects = new List<VM_ForecastSubject>();
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ForecastSubjectsView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;            
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader == null)
                        {
                            _lastError = "Ошибка во время загрузки предметов прогнозов!\nСервер вернул Null.";
                            log.Error(LastError);
                            return null;
                        }
                        if (reader.HasRows)
                        {
                            VM_ForecastSubject subject = null;
                            while (reader.Read())
                            {
                                subject = new VM_ForecastSubject()
                                {
                                    Id = reader.GetByte(0),
                                    Name = reader.GetString(1),
                                    Alias = reader.GetString(2),
                                    Description = reader.GetString(3),
                                    Icon = reader.IsDBNull(4) ? String.Empty : reader.GetString(4),
                                    MetaTitle = reader.GetString(5),
                                    MetaDescriptions = reader.GetString(6),
                                    MetaKeywords = reader.GetString(7),
                                    MetaNoFollow = reader.GetBoolean(8),
                                    MetaNoIndex = reader.GetBoolean(9),
                                    IsActive = reader.GetBoolean(10)
                                };
                                subjects.Add(subject);
                            }
                        }
                        else
                        {
                            _lastError = "Ошибка во время загрузки предметов прогнозов!\nСервер вернул пустой ответ.";
                            log.Error(LastError);
                            return null;
                        }
                    }
                    return subjects;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время загрузки предметов прогнозов!\n{0}", ex.ToString());
                    log.Error(LastError);
                    return null;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
        }
        private List<VM_ForecastUser> _getCurrentForecastUsers(int[] forecasIds)
        {
            if (forecasIds == null || forecasIds.Length != 4)
                return null;

            List<VM_ForecastUser> users = new List<VM_ForecastUser>();
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.CurrentForecastsUsersView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CurrentForecastsUsersView.Params.Id1, forecasIds[0]);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CurrentForecastsUsersView.Params.Id2, forecasIds[1]);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CurrentForecastsUsersView.Params.Id3, forecasIds[2]);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CurrentForecastsUsersView.Params.Id4, forecasIds[3]);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader == null)
                        {
                            _lastError = "Ошибка во время загрузки пользователей текущих прогнозов!\nСервер вернул Null.";
                            log.Error(LastError);
                            return null;
                        }
                        if (reader.HasRows)
                        {
                            VM_ForecastUser user = null;
                            while (reader.Read())
                            {
                                user = new VM_ForecastUser();
                                user.User.Id = reader.GetInt32(reader.GetOrdinal("UserId"));
                                user.User.Nic = reader.GetString(reader.GetOrdinal("Nic"));
                                user.User.Avatar = reader.IsDBNull(reader.GetOrdinal("Avatar")) ? String.Empty :
                                    reader.GetString(reader.GetOrdinal("Avatar"));
                                user.Forecast.Id = reader.GetInt32(reader.GetOrdinal("ForecastId"));
                                user.ValueDate = reader.IsDBNull(reader.GetOrdinal("ReportDate")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("ReportDate"));
                                user.Value = reader.GetDouble(reader.GetOrdinal("Value"));                                                                
                                users.Add(user);
                            }
                        }
                        else
                        {
                            _lastError = "Ошибка во время загрузки пользователей текущих прогнозов!\nСервер вернул пустой ответ.";
                            log.Error(LastError);
                            return null;
                        }
                    }
                    return users;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время загрузки пользователей текущих прогнозов!\n{0}", ex.ToString());
                    log.Error(LastError);
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