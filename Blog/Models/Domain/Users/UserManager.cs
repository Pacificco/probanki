using Bankiru.Models.DataBase;
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