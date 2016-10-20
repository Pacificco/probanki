using Bankiru.Models.DataBase;
using Bankiru.Models.Domain.Users;
using Bankiru.Models.Infrastructure;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Bankiru.Models.Domain.Account
{
    public class AccountManager
    {
        private string _lastError;
        public string LastError { get { return _lastError; } }
        public static readonly ILog log = LogManager.GetLogger(typeof(AccountManager));

        public AccountManager()
        {
            _lastError = String.Empty;
        }

        #region BACKEND
        
        #endregion

        #region FRONTEND
        public int RegisterNewUser(VM_UserRegistration user, string role, string token)
        {            
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.CreateNewUser.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CreateNewUser.Params.NicName, user.NicName.Trim());
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CreateNewUser.Params.Name, user.Name.Trim());
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CreateNewUser.Params.Email, user.Email.Trim());
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CreateNewUser.Params.Password, _getMd5Hash(user.Password.Trim()));
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CreateNewUser.Params.Sex, (int)user.Sex);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CreateNewUser.Params.Subscribed, user.Subscribed);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CreateNewUser.Params.Role, role);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.CreateNewUser.Params.Token, token);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    object objId = command.ExecuteScalar();
                    if(objId == null)
                    {
                        _lastError = "Во время регистрации пользователя произошла ошибка!";
                        log.Error("Во время регистрации пользователя произошла ошибка!");
                        return -1;
                    }
                    int id = Convert.ToInt32(objId.ToString());
                    if (id == 0)
                    {
                        _lastError = "Во время регистрации пользователя произошла ошибка!";
                        return -1;
                    }
                    return id;
                }
                catch (Exception ex)
                {
                    _lastError = "Во время регистрации пользователя произошла ошибка!";
                    log.Error(String.Format("Во время регистрации пользователя произошла ошибка!", ex.ToString()));
                    return -1;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }            
        }
        public bool ChangeUserPassword(int userId, string password)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UserPasswordEdit.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UserPasswordEdit.Params.Id, userId);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UserPasswordEdit.Params.Password, _getMd5Hash(password.Trim()));
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {

                    if (command.ExecuteNonQuery() > 0)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время изменения пароля! " + ex.ToString();
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
        #endregion

        #region ОБЩИЕ МЕТОДЫ
        public VM_User Login(string email, string password)
        {
            VM_User user = _getUser(email);
            if (user == null)
                return null;
            if (_verifyMd5Hash(password, user.Password))
                return user;
            else
                return null;
        }
        public VM_User FindUser(string email)
        {
            return _getUser(email);
        }
        public VM_User FindUser(int id)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UserView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UserView.Params.Id, id);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    VM_User user = new VM_User();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader == null || !reader.HasRows)
                            return null;
                        reader.Read();
                        for (int j = 0; j < reader.FieldCount; j++)
                            user.SetFieldValue(reader.GetName(j), reader.GetValue(j));
                    }
                    return user;
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время загрузки пользователя из базы данных!";
                    log.Error(String.Format("Ошибка во время загрузки пользователя из базы данных! ", ex.ToString()));
                    return null;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
        }
        public VM_User FindActiveUser(string email)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ActiveUserByEmailView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.ActiveUserByEmailView.Params.Email, email);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    VM_User user = new VM_User();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader == null || !reader.HasRows)
                            return null;
                        reader.Read();
                        for (int j = 0; j < reader.FieldCount; j++)
                            user.SetFieldValue(reader.GetName(j), reader.GetValue(j));
                    }
                    return user;
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время загрузки пользователя из базы данных!";
                    log.Error(String.Format("Ошибка во время загрузки пользователя из базы данных! ", ex.ToString()));
                    return null;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
        }
        public VM_User FindActiveUser(int id)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ActiveUserView.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.ActiveUserView.Params.Id, id);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    VM_User user = new VM_User();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader == null || !reader.HasRows)
                            return null;
                        reader.Read();
                        for (int j = 0; j < reader.FieldCount; j++)
                            user.SetFieldValue(reader.GetName(j), reader.GetValue(j));
                    }
                    return user;
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время загрузки пользователя из базы данных!";
                    log.Error(String.Format("Ошибка во время загрузки пользователя из базы данных! ", ex.ToString()));
                    return null;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
        }
        public bool MailExists(string email)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.MailExists.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.MailExists.Params.Email, email.Trim());
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    object exists = command.ExecuteScalar();
                    if (exists == null)
                        return true;

                    return Convert.ToInt32(exists.ToString()) == 1 ? true : false;
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время проверки Email на существование!";
                    log.Error(String.Format("Ошибка во время проверки Email на существование!", ex.ToString()));
                    return true;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
        }
        public bool NicExists(string nic)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.NicExists.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.NicExists.Params.Nic, nic.Trim());
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    object exists = command.ExecuteScalar();
                    if (exists == null)
                        return true;

                    return Convert.ToInt32(exists.ToString()) == 1 ? true : false;
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время проверки Никнейма на существование!";
                    log.Error(String.Format("Ошибка во время проверки Никнейма на существование!", ex.ToString()));
                    return true;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
        }        
        #endregion

        #region СКРЫТЫЕ МЕТОДЫ
        //Пользователи        
        private VM_User _getUser(string email)
        {
            string SQLQuery = _sqlGetUser(email);

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
                        _lastError = "Ошибка во время загрузки пользователя!\nСервер вернул ответ NULL.";
                        return null;
                    }

                    if (_reader.HasRows)
                    {
                        if (_reader.Read())
                        {
                            VM_User u = new VM_User()
                            {
                                Id = _reader.GetInt32(0),
                                Name = _reader.GetString(1),
                                Email = _reader.GetString(2),
                                Password = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                                Sex = (VM_UserSex)_reader.GetInt32(4),
                                LastName = _reader.GetString(5),
                                FatherName = _reader.GetString(6),
                                IsActive = _reader.GetBoolean(7),
                                IsSubscribed = _reader.GetBoolean(8),
                                Nic = _reader.GetString(9),
                                Rols = _reader.IsDBNull(10) ? new string[] { "guest" } : _reader.GetString(10).Split(new char[] { ',' })
                            };
                            return u;
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время загрузки пользователя!\n" + ex.ToString();
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
        private string _sqlGetUser(string email)
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " *";
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Users.TABLENAME;
            SqlQuery += " " + DbStruct.SE.WHERE + " ";
            SqlQuery += DbStruct.Users.FIELDS.Email;
            SqlQuery += " = '" + email + "'";
            SqlQuery += ";";
            return SqlQuery;
        }

        //Вспомагательные методы
        private string _getMd5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }
        private bool _verifyMd5Hash(string input, string hash)
        {
            string hashOfInput = _getMd5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (comparer.Compare(hashOfInput, hash) == 0)
                return true;
            else
                return false;
        }
        #endregion
    }
}