using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Infrastructure
{
    /// <summary>
    /// Класс представляет собой контейнер для хранения глобальный конфигураций и настроек
    /// </summary>
    public static class GlobalParams
    {
        #region SQL
        /// <summary>
        /// Подключение к серверу
        /// </summary>
        private static SqlConnection _SqlConnection = null;
        /// <summary>
        /// Ссылка на текущий экземпляр блокировки
        /// </summary>
        public static object _DBAccessLock = new object();
        /// <summary>
        /// Ошибка подключения к базе данных
        /// </summary>
        public static string _connectionError = "";
        /// <summary>
        /// Подключается к базе данных
        /// </summary>
        public static SqlConnection GetConnection()
        {
            try
            {
                if (_SqlConnection == null || _SqlConnection.State != System.Data.ConnectionState.Open)
                {
                    _SqlConnection = new SqlConnection("Data Source=ДОМ\\SQLEXPRESS; Initial Catalog=Bankiru; User ID=UserInvest; Password=1q2w3e");
                    //_SqlConnection = new SqlConnection("Data Source=localhost;Integrated Security=False;User ID=u0201459_probanki_user;Connect Timeout=15;Encrypt=False;Packet Size=4096;Password=Qce7%0w4");



                    //_SqlConnection = new SqlConnection("Data Source=probanki.mssql.somee.com; Initial Catalog=probanki; User ID=pcliskiweb_SQLLogin_1; Password=88xnxg87bk");
                    _SqlConnection.Open();
                    if (_SqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        return _SqlConnection;
                    }
                    else
                    {
                        _connectionError = "Не удалось подключиться к базе данных!";
                        return null;
                    }
                }
                else
                {
                    return _SqlConnection;
                }
            }
            catch (Exception e)
            {
                _connectionError = e.ToString();
                return null;
            }
        }        
        #endregion

        #region ВРЕМЯ
        /// <summary>
        /// Впеменная зона
        /// </summary>
        public static TimeZoneInfo MoscowTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
        #endregion
    }
}