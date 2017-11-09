using Bankiru.Models.DataBase;
using Bankiru.Models.Infrastructure;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Debtors
{
    /// <summary>
    /// Менеджер по работе с должниками
    /// </summary>
    public class DebtorsManager
    {
        /// <summary>
        /// Количество строк при выводе должников
        /// </summary>
        private int _recPerPage = 10;
        /// <summary>
        /// Сообщение о последней ошибке
        /// </summary>
        private string _lastError;
        /// <summary>
        /// Сообщение о последней ошибке
        /// </summary>
        public string LastError { get { return _lastError; } }
        /// <summary>
        /// Лог
        /// </summary>
        public static readonly ILog log = LogManager.GetLogger(typeof(DebtorsManager));

        /// <summary>
        /// Возвращает список должников
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="page">Страница</param>
        /// <returns>Модель списка должников для отображения на страницах сайта</returns>
        public VM_Debtors GetDebtors(VM_DebtorsFilter filter, int page = 1)
        {
            VM_Debtors debtors = new VM_Debtors();
            
            debtors.Filters.Assign(filter);
            DebtorsFilter sqlfilter = new DebtorsFilter();
            sqlfilter.Assign(filter);

            int totalCount = 0;
            debtors.PagingInfo.ItemsPerPage = _recPerPage;
            debtors.PagingInfo.CurrentPage = page;

            debtors.Items = _getDebtors(sqlfilter, debtors.PagingInfo.GetNumberFrom(), debtors.PagingInfo.GetNumberTo(), out totalCount);
            if (debtors.Items == null)
                return null;
            
            debtors.PagingInfo.SetData(page, totalCount);
            return debtors;
        }
        /// <summary>
        /// Возвращает список должников
        /// </summary>
        /// <param name="debtorId">Идентификатор должника</param>
        /// <returns>Модель должника</returns>
        public Debtor GetDebtor(int debtorId)
        {
            try
            {
                VM_DebtorsFilter filter = new VM_DebtorsFilter();
                filter.DebtorId = debtorId;                
                DebtorsFilter sqlfilter = new DebtorsFilter();
                sqlfilter.Assign(filter);

                int totalCount = 0;
                List<Debtor> Items = _getDebtors(sqlfilter, 1, 1, out totalCount);

                if (Items == null || Items.Count == 0)
                    return null;                

                return Items.First();
            }
            catch(Exception ex)
            {
                _lastError = "Ошибка во время загрузки должника!\n" + ex.ToString();
                log.Error(_lastError);
                return null;
            }            
        }
        /// <summary>
        /// Возвращает список должников по заданному фильтру
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="recNoFrom">Номер первой строки</param>
        /// <param name="recNoTo">Номер последней строки</param>
        /// <param name="totalCount">Общее количество записей, удовлетворяющих фильтру</param>
        /// <returns>Список должников</returns>
        public List<Debtor> _getDebtors(DebtorsFilter filter, int recNoFrom, int recNoTo, out int totalCount)
        {
            totalCount = 0;

            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.DebtorsView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            if (filter.Published == null)
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.Published, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.Published, (bool)filter.Published);
            if (filter.DebtorId == null)
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.DebtorId, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.DebtorId, (int)filter.DebtorId);
            if(filter.CourtDecisionType == EnumCourtDecisionType.Undefind)
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.CourtDecisionTypeId, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.CourtDecisionTypeId, (int)filter.CourtDecisionType);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.DebtAmountRange, filter.DebtAmountRange);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.DebtCreatedRange, filter.DebtCreatedRange);
            if(filter.DebtEssenceType == EnumDebtEssenceType.Undefind)
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.DebtEssenceTypeId, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.DebtEssenceTypeId, (int)filter.DebtEssenceType);
            if (filter.DebtorType == EnumDebtorType.Undefind)
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.DebtorTypeId, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.DebtorTypeId, (int)filter.DebtorType);
            if (filter.DebtSellerType == EnumDebtSellerType.Undefind)
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.DebtSellerTypeId, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.DebtSellerTypeId, filter.DebtSellerType);
            if(filter.RegionId == null)
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.RegionId, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.RegionId, (Guid)filter.RegionId);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.SalePriceRange, filter.SalePriceRange);
            if (filter.OriginalCreditorType == EnumOriginalCreditorType.Undefind)
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.OriginalCreditorTypeId, DBNull.Value);
            else
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.OriginalCreditorTypeId, (int)filter.OriginalCreditorType);

            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.RecFrom, recNoFrom);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsView.Params.RecTo, recNoTo);

            SqlParameter totalCountParameter = new SqlParameter();
            totalCountParameter.ParameterName = "@TotalCount";
            totalCountParameter.DbType = System.Data.DbType.Int32;
            totalCountParameter.Direction = System.Data.ParameterDirection.Output;
            totalCountParameter.Value = 0;
            command.Parameters.Add(totalCountParameter);

            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    List<Debtor> debtors = new List<Debtor>();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            Debtor d = null;
                            while (reader.Read())
                            {
                                d = new Debtor();
                                d.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                                d.DebtorType = (EnumDebtorType)reader.GetInt32(reader.GetOrdinal("DebtorTypeId"));
                                d.Published = reader.GetBoolean(reader.GetOrdinal("Published"));
                                d.OriginalCreditorType = (EnumOriginalCreditorType)reader.GetInt32(reader.GetOrdinal("OriginalCreditorTypeId"));
                                d.RegionId = reader.GetGuid(reader.GetOrdinal("RegionId"));
                                d.Locality = reader.GetString(reader.GetOrdinal("Locality"));
                                d.DebtEssenceType = (EnumDebtEssenceType)reader.GetInt32(reader.GetOrdinal("DebtEssenceTypeId"));
                                d.CourtDecisionType = (EnumCourtDecisionType)reader.GetInt32(reader.GetOrdinal("CourtDecisionTypeId"));
                                d.DebtCreatedDate = reader.GetDateTime(reader.GetOrdinal("DebtCreatedDate"));
                                d.DebtAmount = reader.GetDecimal(reader.GetOrdinal("DebtAmount"));
                                d.SalePrice = reader.GetDecimal(reader.GetOrdinal("SalePrice"));
                                d.DebtSellerType = (EnumDebtSellerType)reader.GetInt32(reader.GetOrdinal("DebtSellerTypeId"));
                                d.ContactPerson = reader.GetString(reader.GetOrdinal("ContactPerson"));
                                d.ContactPhone = reader.GetInt64(reader.GetOrdinal("ContactPhone"));
                                d.DopPhone = reader.IsDBNull(reader.GetOrdinal("DopPhone")) ? null :
                                    (Int64?)(reader.GetInt64(reader.GetOrdinal("DopPhone")));
                                d.Email = reader.GetString(reader.GetOrdinal("Email"));
                                d.Comment = reader.GetString(reader.GetOrdinal("Comment"));
                                d.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                                d.UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"));

                                d.RegionName = reader.GetString(reader.GetOrdinal("RegionName"));
                                d.DebtorTypeName = reader.GetString(reader.GetOrdinal("DebtorTypeName"));
                                d.OriginalCreditorTypeName = reader.GetString(reader.GetOrdinal("OriginalCreditorTypeName"));
                                d.CourtDecisionTypeName = reader.GetString(reader.GetOrdinal("CourtDecisionTypeName"));
                                d.DebtSellerTypeName = reader.GetString(reader.GetOrdinal("DebtSellerTypeName"));
                                d.DebtEssenceTypeName = reader.GetString(reader.GetOrdinal("DebtEssenceTypeName"));
                                debtors.Add(d);
                            }
                        }
                    }
                    totalCount = totalCountParameter.Value == null ? 0 : (int)totalCountParameter.Value;
                    return debtors;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время загрузки должников из базы данных {0}!\n{1}",
                        DbStruct.PROCEDURES.DebtorsView.Name, ex.ToString());
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
        /// Создает/обновляет указанного должника
        /// </summary>
        /// <param name="debtor">Должник</param>
        /// <param name="operation">Тип операции</param>
        /// <returns>Логическое значение</returns>
        public bool EditDebtor(Debtor debtor, int operation)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.DebtorsEdit.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Operation, operation);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Id, debtor.Id);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Published, debtor.Published);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.CourtDecisionTypeId, debtor.CourtDecisionType);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtAmount, debtor.DebtAmount);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtCreatedDate, debtor.DebtCreatedDate);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtEssenceTypeId, debtor.DebtEssenceType);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtorTypeId, debtor.DebtorType);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtSellerTypeId, debtor.DebtSellerType);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.RegionId, debtor.RegionId);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.SalePrice, debtor.SalePrice);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.OriginalCreditorTypeId, debtor.OriginalCreditorType);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Locality, debtor.Locality);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Comment, debtor.Comment);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.ContactPerson, debtor.ContactPerson);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.ContactPhone, debtor.ContactPhone);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DopPhone, debtor.DopPhone);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Email, debtor.Email);

            SqlParameter returnValue = new SqlParameter();
            returnValue.DbType = System.Data.DbType.Int32;
            returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
            returnValue.Value = 1;
            command.Parameters.Add(returnValue);

            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    command.ExecuteNonQuery();
                    if ((int)returnValue.Value == 1)
                    {
                        _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\nОперация {1}.",
                            DbStruct.PROCEDURES.DebtorsEdit.Name, operation);
                        log.Error(_lastError);
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время создания/обновления должника в базе данных {0}!\n{1}",
                        DbStruct.PROCEDURES.DebtorsEdit.Name, ex.ToString());
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
        /// Обновляет активность указанного должника
        /// </summary>
        /// <param name="debtorId">Должник</param>
        /// <param name="published">Активность</param>
        /// <returns>Логическое значение</returns>
        public bool EditDebtorPublished(int debtorId, bool published)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.DebtorsEdit.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Operation, 5);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Id, debtorId);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Published, published);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.CourtDecisionTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtAmount, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtCreatedDate, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtEssenceTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtorTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtSellerTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.RegionId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.SalePrice, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.OriginalCreditorTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Locality, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Comment, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.ContactPerson, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.ContactPhone, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DopPhone, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Email, DBNull.Value);

            SqlParameter returnValue = new SqlParameter();
            returnValue.DbType = System.Data.DbType.Int32;
            returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
            returnValue.Value = 1;
            command.Parameters.Add(returnValue);

            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    command.ExecuteNonQuery();
                    if ((int)returnValue.Value == 1)
                    {
                        _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\nОперация {1}.",
                            DbStruct.PROCEDURES.DebtorsEdit.Name, 5);
                        log.Error(_lastError);
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время обновления активности должника в базе данных {0}!\n{1}",
                        DbStruct.PROCEDURES.DebtorsEdit.Name, ex.ToString());
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
        /// Удаляет указанного должника
        /// </summary>
        /// <param name="debtorId">Должник</param>
        /// <returns>Логическое значение</returns>
        public bool DeleteDebtor(int debtorId)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.DebtorsEdit.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Operation, 3);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Id, debtorId);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Published, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.CourtDecisionTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtAmount, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtCreatedDate, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtEssenceTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtorTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtSellerTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.RegionId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.SalePrice, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.OriginalCreditorTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Locality, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Comment, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.ContactPerson, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.ContactPhone, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DopPhone, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Email, DBNull.Value);

            SqlParameter returnValue = new SqlParameter();
            returnValue.DbType = System.Data.DbType.Int32;
            returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
            returnValue.Value = 1;
            command.Parameters.Add(returnValue);

            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    command.ExecuteNonQuery();
                    if ((int)returnValue.Value == 1)
                    {
                        _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\nОперация {1}.",
                            DbStruct.PROCEDURES.DebtorsEdit.Name, 3);
                        log.Error(_lastError);
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время создания/обновления должника в базе данных {0}!\n{1}",
                        DbStruct.PROCEDURES.DebtorsEdit.Name, ex.ToString());
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
        /// Изменяет активность указанного должника
        /// </summary>
        /// <param name="debtorId">Должник</param>
        /// <param name="activeState">Состояние активности</param>
        /// <returns>Логическое значение</returns>
        public bool SetDebtorActive(int debtorId, bool activeState)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.DebtorsEdit.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Operation, 5);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Id, debtorId);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Published, activeState);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.CourtDecisionTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtAmount, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtCreatedDate, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtEssenceTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtorTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DebtSellerTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.RegionId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.SalePrice, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.OriginalCreditorTypeId, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Locality, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Comment, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.ContactPerson, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.ContactPhone, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.DopPhone, DBNull.Value);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.DebtorsEdit.Params.Email, DBNull.Value);

            SqlParameter returnValue = new SqlParameter();
            returnValue.DbType = System.Data.DbType.Int32;
            returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
            returnValue.Value = 1;
            command.Parameters.Add(returnValue);

            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    command.ExecuteNonQuery();
                    if ((int)returnValue.Value == 1)
                    {
                        _lastError = String.Format("Ошибка во время выполнения хранимой процедуры {0}!\nОперация {1}.",
                            DbStruct.PROCEDURES.DebtorsEdit.Name, 3);
                        log.Error(_lastError);
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    _lastError = String.Format("Ошибка во время создания/обновления должника в базе данных {0}!\n{1}",
                        DbStruct.PROCEDURES.DebtorsEdit.Name, ex.ToString());
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
    }
}