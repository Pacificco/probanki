using Bankiru.Models.DataBase;
using Bankiru.Models.Infrastructure;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Club
{
    public class ForecastsManager
    {
        private string _lastError;
        public string LastError { get { return _lastError; } }
        public static readonly ILog log = LogManager.GetLogger(typeof(ForecastsManager));

        public ForecastsManager()
        {
            _lastError = "";
        }

        /// <summary>
        /// Создает новые дни прогнозов
        /// </summary>
        /// <param name="forecastDate">Дата прогнозов</param>
        /// <returns>Логическое значение</returns>
        public bool CreateNewForecasts(DateTime forecastDate)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.CreateNewForecasts.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CreateNewForecasts.Params.ForecastDate, forecastDate);
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
                        DbStruct.PROCEDURES.CreateNewForecasts.Name, ex.ToString());
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
        /// <returns>Логическое значение</returns>
        public bool AddUserToForecast(int forecastId, int userId, double value)
        {
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
                    if (command.ExecuteNonQuery() == 1)
                        return false;
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                        DbStruct.PROCEDURES.AddUserToForecast.Name, ex.ToString());
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
        /// Закрывает прогнозы по указанной дате
        /// </summary>
        /// <param name="forecastDate">Дата окончания прогноза</param>
        /// <returns>Логическое значение</returns>
        public bool CloseForecasts(DateTime forecastDate)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.CloseForecasts.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CloseForecasts.Params.ForecastDate, forecastDate);
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
                        DbStruct.PROCEDURES.CloseForecasts.Name, ex.ToString());
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
        public List<VM_Forecast> GetForecasts(bool IsClosed, ForecastSubject subject)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ForecastsView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastsView.Params.IsClosed, IsClosed);
            if (subject == ForecastSubject.Undefined)
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastsView.Params.SubjectId, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.ForecastsView.Params.SubjectId, (byte)subject);
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
    }
}