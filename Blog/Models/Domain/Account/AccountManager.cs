using Bankiru.Models.DataBase;
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
        public bool RegisterNewUser(VM_UserRegistration user, string role)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.CreateNewUser.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(DbStruct.PROCEDURES.CreateNewUser.Params.NicName, user.NicName.Trim());
            command.Parameters.Add(DbStruct.PROCEDURES.CreateNewUser.Params.Name, user.Name.Trim());
            command.Parameters.Add(DbStruct.PROCEDURES.CreateNewUser.Params.Email, user.Email.Trim());
            command.Parameters.Add(DbStruct.PROCEDURES.CreateNewUser.Params.Password, _getMd5Hash(user.Password.Trim()));
            command.Parameters.Add(DbStruct.PROCEDURES.CreateNewUser.Params.Sex, (int)user.Sex);
            command.Parameters.Add(DbStruct.PROCEDURES.CreateNewUser.Params.Subscribed, user.Subscribed);
            command.Parameters.Add(DbStruct.PROCEDURES.CreateNewUser.Params.Role, role);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    if (command.ExecuteNonQuery() == 1)
                    {
                        _lastError = "Во время регистрации пользователя произошла ошибка!";
                        log.Error(String.Format("Во время регистрации пользователя произошла ошибка!", ex.ToString()));
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    _lastError = "Во время регистрации пользователя произошла ошибка!";
                    log.Error(String.Format("Во время регистрации пользователя произошла ошибка!", ex.ToString()));
                    return false;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
            return true;
        }
        #endregion

        #region FRONTEND

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
        public VM_User Find(string email)
        {
            return _getUser(email);
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
                                Sex = (EnumSex)_reader.GetInt32(4),
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
                    LastError = "Ошибка во время загрузки пользователя!\n" + ex.ToString();
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