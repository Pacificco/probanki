﻿using Bankiru.Models.Domain;
using Bankiru.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bankiru.Models.DataBase
{
    /// <summary>
    /// Класс представляет собой структуру базы данных
    /// </summary>
    public static class DbStruct
    {
        /// <summary>
        /// Текст последней ошибки
        /// </summary>
        public static string LastError = String.Empty;
        /// <summary>
        /// Массив запрещенных символов для применения в SQL запросах
        /// </summary>
        public static char[] EscapeCharArray = new char[] { '\\', '`', '\'', '"' };

        #region ТРАНЗАКЦИИ
        /// <summary>
        /// Открывает новую транзакцию с указанным имененм
        /// </summary>
        /// <param name="transName">Имя транзакции</param>
        /// <returns>Логическое значение</returns>
        public static bool BeginTransaction(string transName)
        {
            SqlCommand command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    command = new SqlCommand(SE.BEGIN_TRAN + " " + transName + ";", GlobalParams.GetConnection());
                    command.CommandTimeout = 30;
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время открытия новой транзакции (" + transName + ")!\n" + ex.ToString();
                    return false;
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }
        /// <summary>
        /// Закрывает транзакцию с указанным имененм
        /// </summary>
        /// <param name="transName">Имя транзакции</param>
        /// <returns>Логическое значение</returns>
        public static bool CommitTransaction(string transName)
        {
            SqlCommand command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    command = new SqlCommand(SE.COMMIT_TRAN + " " + transName + ";", GlobalParams.GetConnection());
                    command.CommandTimeout = 30;
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время закрытия транзакции (" + transName + ")!\n" + ex.ToString();
                    return false;
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }

        }
        /// <summary>
        /// Выполняет откат транзакции с указанным имененм
        /// </summary>
        /// <param name="transName">Имя транзакции</param>
        /// <returns>Логическое значение</returns>
        public static bool RollBackTransaction(string transName)
        {
            SqlCommand command = null;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    command = new SqlCommand(SE.ROLLBACK_TRAN + " " + transName + ";", GlobalParams.GetConnection());
                    command.CommandTimeout = 30;
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время отката транзакции (" + transName + ")!\n" + ex.ToString();
                    return false;
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }

        }
        #endregion

        #region МЕТОДЫ
        /// <summary>
        /// Экранирует специальные символы в строках для использования в выражениях SQL
        /// </summary>
        /// <param name="value">Строка</param>
        /// <returns>Строка</returns>
        public static string SQLRealEscapeString(string value)
        {
            if (String.IsNullOrEmpty(value)) return "";
            string result = FixString(value, "'", "''");            
            return result;
        }
        private static string FixString(string SourceString, string StringToReplace, string StringReplacement)
        {
            SourceString = SourceString.Replace(StringToReplace, StringReplacement);
            return (SourceString);
        }
        /// <summary>
        /// Возвращает дату и время в формате, использующемся в SQL-запросах
        /// </summary>
        /// <param name="Value">Дата и время</param>
        /// /// <param name="dFormat">Возвращаемый формат дата и время</param>
        /// <returns>Строка</returns>
        public static string FormatSQLDateTime(DateTime Value, SQLDateTimeFormat dFormat)
        {
            string Result = String.Empty;
            switch (dFormat)
            {
                case SQLDateTimeFormat.sdfDateTime:
                    Result += Value.Year.ToString();
                    Result += Value.Month.ToString().Length == 1 ? "0" + Value.Month.ToString() : Value.Month.ToString();
                    Result += Value.Day.ToString().Length == 1 ? "0" + Value.Day.ToString() : Value.Day.ToString();
                    Result += " ";

                    Result += Value.Hour.ToString().Length == 1 ? "0" + Value.Hour.ToString() : Value.Hour.ToString();
                    Result += ":";
                    Result += Value.Minute.ToString().Length == 1 ? "0" + Value.Minute.ToString() : Value.Minute.ToString();
                    Result += ":";
                    Result += Value.Second.ToString().Length == 1 ? "0" + Value.Second.ToString() : Value.Second.ToString();

                    Result = Result.Trim();
                    break;
                case SQLDateTimeFormat.sdfDate:
                    Result += Value.Year.ToString();
                    Result += Value.Month.ToString().Length == 1 ? "0" + Value.Month.ToString() : Value.Month.ToString();
                    Result += Value.Day.ToString().Length == 1 ? "0" + Value.Day.ToString() : Value.Day.ToString();
                    break;
                case SQLDateTimeFormat.sdfTime:
                    Result += Value.Hour.ToString().Length == 1 ? "0" + Value.Hour.ToString() : Value.Hour.ToString();
                    Result += ":";
                    Result += Value.Minute.ToString().Length == 1 ? "0" + Value.Minute.ToString() : Value.Minute.ToString();
                    Result += ":";
                    Result += Value.Second.ToString().Length == 1 ? "0" + Value.Second.ToString() : Value.Second.ToString();
                    break;
                case SQLDateTimeFormat.sdfDateTime_TimeMin:
                    Result += Value.Year.ToString();
                    Result += Value.Month.ToString().Length == 1 ? "0" + Value.Month.ToString() : Value.Month.ToString();
                    Result += Value.Day.ToString().Length == 1 ? "0" + Value.Day.ToString() : Value.Day.ToString();
                    Result += " ";
                    Result += "00:00:00";
                    break;
                case SQLDateTimeFormat.sdfDateTime_TimeMax:
                    Result += Value.Year.ToString();
                    Result += Value.Month.ToString().Length == 1 ? "0" + Value.Month.ToString() : Value.Month.ToString();
                    Result += Value.Day.ToString().Length == 1 ? "0" + Value.Day.ToString() : Value.Day.ToString();
                    Result += " ";
                    Result += "23:59:59";
                    break;
            }

            return "'" + Result + "'";
        }
        #endregion

        #region ТАБЛИЦЫ
        /// <summary>
        /// Таблица "Статьи"
        /// </summary>
        public static class Articles
        {
            /// <summary>
            /// Имя таблицы
            /// </summary>
            public const string TABLENAME = "Articles";
            /// <summary>
            /// Псевдоним таблицы
            /// </summary>
            public const string ALIAS = "a";
            /// <summary>
            /// Поля таблицы
            /// </summary>
            public static class FIELDS
            {
                public const string Id = "Id";
                public const string Title = "Title";
                public const string Alias = "Alias";
                public const string SubTitle = "SubTitle";
                public const string TextPrev = "TextPrev";
                public const string CategoryId = "CategoryId";
                public const string IsActive = "IsActive";
                public const string IsCentral = "IsCentral";
                public const string CentralNumber = "CentralNumber";
                public const string MetaTitle = "MetaTitle";
                public const string MetaKeys = "MetaKeys";
                public const string MetaDesc = "MetaDesc";
                public const string MetaNoIndex = "MetaNoIndex";
                public const string MetaNoFollow = "MetaNoFollow";
                public const string ChangedAt = "ChangedAt";
                public const string CreatedAt = "CreatedAt";
                public const string UserId = "UserId";
                public const string TextFull = "TextFull";
                public const string OtherUser = "OtherUser";
                public const string PublishedAt = "PublishedAt";
                public const string Hits = "Hits";

            }
        }
        /// <summary>
        /// Таблица "Организации"
        /// </summary>
        public static class Orgs
        {
            /// <summary>
            /// Имя таблицы
            /// </summary>
            public const string TABLENAME = "Orgs";
            /// <summary>
            /// Псевдоним таблицы
            /// </summary>
            public const string ALIAS = "o";
            /// <summary>
            /// Поля таблицы
            /// </summary>
            public static class FIELDS
            {
                public const string Id = "Id";
                public const string Alias = "Alias";
                public const string Title = "Title";
                public const string Descriptions = "Descriptions";
                public const string Comment = "Comment";
                public const string Icon = "Icon";
                public const string IconTitle = "IconTitle";
                public const string IconAlt = "IconAlt";
                public const string CategoryId = "CategoryId";
                public const string ParentId = "ParentId";
                public const string RegionId = "RegionId";
                public const string MetaTitle = "MetaTitle";
                public const string MetaDescriptions = "MetaDescriptions";
                public const string MetaNoFollow = "MetaNoFollow";
                public const string MetaNoIndex = "MetaNoIndex";
                public const string CreatedAt = "CreatedAt";
                public const string ChangedAt = "ChangedAt";
                public const string IsActive = "IsActive";
                public const string Hits = "Hits";
                public const string LastVisitedAt = "LastVisitedAt";
                public const string MetaKeywords = "MetaKeywords";
            }
        }
        /// <summary>
        /// Таблица "Офисы и банкоматы"
        /// </summary>
        public static class OrgPoints
        {
            /// <summary>
            /// Имя таблицы
            /// </summary>
            public const string TABLENAME = "OrgPoints";
            /// <summary>
            /// Псевдоним таблицы
            /// </summary>
            public const string ALIAS = "o_p";
            /// <summary>
            /// Поля таблицы
            /// </summary>
            public static class FIELDS
            {
                public const string Id = "Id";
                public const string Alias = "Alias";
                public const string Title = "Title";
                public const string RegionId = "RegionId";
                public const string OrgId = "OrgId";
                public const string PointType = "PointType";
                public const string Phones = "Phones";
                public const string Address = "Address";
                public const string AddressDopInfo = "AddressDopInfo";
                public const string Schedule = "Schedule";
                public const string DopInfo = "DopInfo";
                public const string IsActive = "IsActive";
            }
        }
        /// <summary>
        /// Таблица "Категории организаций"
        /// </summary>
        public static class OrgCategories
        {
            /// <summary>
            /// Имя таблицы
            /// </summary>
            public const string TABLENAME = "OrgCategories";
            /// <summary>
            /// Псевдоним таблицы
            /// </summary>
            public const string ALIAS = "o_cats";
            /// <summary>
            /// Поля таблицы
            /// </summary>
            public static class FIELDS
            {
                public const string Id = "Id";
                public const string Alias = "Alias";
                public const string Title = "Title";
                public const string Descriptions = "Descriptions";
                public const string Icon = "Icon";
                public const string MetaTitle = "MetaTitle";
                public const string MetaDescriptions = "MetaDescriptions";
                public const string MetaKeywords = "MetaKeywords";
                public const string MetaNoFollow = "MetaNoFollow";
                public const string MetaNoIndex = "MetaNoIndex";
                public const string IsActive = "IsActive";

            }
        }
        /// <summary>
        /// Таблица "Категории"
        /// </summary>
        public static class Categories
        {
            /// <summary>
            /// Имя таблицы
            /// </summary>
            public const string TABLENAME = "Categories";
            /// <summary>
            /// Псевдоним таблицы
            /// </summary>
            public const string ALIAS = "cats";
            /// <summary>
            /// Поля таблицы
            /// </summary>
            public static class FIELDS
            {
                public const string Id = "Id";
                public const string Title = "Name";
                public const string Alias = "Alias";
                public const string ParentId = "ParentId";
                public const string OrderNum = "OrderNum";
                public const string IsActive = "IsActive";
                public const string CDescription = "CDescription";
            }
        }
        /// <summary>
        /// Таблица "Комментарии"
        /// </summary>
        public static class Comments
        {
            /// <summary>
            /// Имя таблицы
            /// </summary>
            public const string TABLENAME = "Comments";
            /// <summary>
            /// Псевдоним таблицы
            /// </summary>
            public const string ALIAS = "com";
            /// <summary>
            /// Поля таблицы
            /// </summary>
            public static class FIELDS
            {
                public const string Id = "Id";
                public const string Text = "CommentText";
                public const string UserId = "UserId";
                public const string UserName = "UserName";
                public const string ParentId = "ParentId";
                public const string CreatedAt = "CreatedAt";
                public const string ArticleId = "ArticleId";
                public const string IsActive = "IsActive";
                public const string Confirmed = "Confirmed";
                public const string LikeCount = "LikeCount";
                public const string DisLikeCount = "DisLikeCount";
            }
        }
        /// <summary>
        /// Таблица "Отзывы"
        /// </summary>
        public static class Reviews
        {
            /// <summary>
            /// Имя таблицы
            /// </summary>
            public const string TABLENAME = "Reviews";
            /// <summary>
            /// Псевдоним таблицы
            /// </summary>
            public const string ALIAS = "r";
            /// <summary>
            /// Поля таблицы
            /// </summary>
            public static class FIELDS
            {
                public const string Id = "Id";
                public const string OrgId = "OrgId";
                public const string UserId = "UserId";
                public const string UserName = "UserName";
                public const string ReviewTitle = "ReviewTitle";
                public const string ReviewText = "ReviewText";
                public const string CreatedAt = "CreatedAt";
                public const string Rating = "Rating";
                public const string IsActive = "IsActive";
                public const string ReviewState = "ReviewState";
            }
        }
        /// <summary>
        /// Таблица "Пользователи"
        /// </summary>
        public static class Users
        {
            /// <summary>
            /// Имя таблицы
            /// </summary>
            public const string TABLENAME = "Users";
            /// <summary>
            /// Псевдоним таблицы
            /// </summary>
            public const string ALIAS = "u";
            /// <summary>
            /// Поля таблицы
            /// </summary>
            public static class FIELDS
            {
                public const string Id = "Id";
                public const string Name = "Name";
                public const string Email = "Email";
                public const string Password = "Password";
                public const string Sex = "Sex";
                public const string LastName = "LastName";
                public const string FatherName = "FatherName";
                public const string IsActive = "IsActive";
                public const string IsSubscribed = "IsSubscribed";
                public const string Nic = "Nic";
            }
        }
        /// <summary>
        /// Таблица "Регионы"
        /// </summary>
        public static class Regions
        {
            /// <summary>
            /// Имя таблицы
            /// </summary>
            public const string TABLENAME = "Regions";
            /// <summary>
            /// Псевдоним таблицы
            /// </summary>
            public const string ALIAS = "regs";
            /// <summary>
            /// Поля таблицы
            /// </summary>
            public static class FIELDS
            {
                public const string AoId = "AoId";
                public const string AoGuid = "AoGuid";
                public const string ParentGuid = "ParentGuid";
                public const string AoLevel = "AoLevel";
                public const string LiveStatus = "LiveStatus";
                public const string CurrStatus = "CurrStatus";
                public const string ActStatus = "ActStatus";
                public const string RegionCode = "RegionCode";
                public const string AutoCode = "AutoCode";
                public const string AreaCode = "AreaCode";
                public const string CityCode = "CityCode";
                public const string CtarCode = "CtarCode";
                public const string PlaceCode = "PlaceCode";
                public const string StreetCode = "StreetCode";
                public const string ExtrCode = "ExtrCode";
                public const string SextCode = "SextCode";
                public const string CentStatus = "CentStatus";
                public const string PostalCode = "PostalCode";
                public const string OffName = "OffName";
                public const string FormalName = "FormalName";
                public const string ShortName = "ShortName";
            }
        }
        /// <summary>
        /// Таблица "Новости"
        /// </summary>
        public static class News
        {
            /// <summary>
            /// Имя таблицы
            /// </summary>
            public const string TABLENAME = "News";
            /// <summary>
            /// Псевдоним таблицы
            /// </summary>
            public const string ALIAS = "n";
            /// <summary>
            /// Поля таблицы
            /// </summary>
            public static class FIELDS
            {
                public const string Id = "Id";
                public const string Author = "Author";
                public const string SourceUrl = "SourceUrl";
                public const string CreatedAt = "CreatedAt";
                public const string Title = "Title";
                public const string Alias = "Alias";
                public const string NewsText = "NewsText";
                public const string MetaTitle = "MetaTitle";
                public const string MetaDescription = "MetaDescription";
                public const string MetaKeywords = "MetaKeywords";
                public const string MetaNofollow = "MetaNofollow";
                public const string MetaNoindex = "MetaNoindex";
                public const string IsActive = "IsActive";
                public const string PublishedAt = "PublishedAt";
                public const string NewsUrl = "NewsUrl";
                public const string PictureUrl = "PictureUrl";
            }
        }
        #endregion

        /// <summary>
        /// Конструкции и выражения
        /// </summary>
        public static class SE
        {
            /// <summary>
            /// DISTINCT
            /// </summary>
            public const string DISTINCT = "DISTINCT";
            /// <summary>
            /// SELECT
            /// </summary>
            public const string SELECT = "SELECT";
            /// <summary>
            /// UNION
            /// </summary>
            public const string UNION = "UNION";
            /// <summary>
            /// INSERT
            /// </summary>
            public const string INSERT = "INSERT INTO";
            /// <summary>
            /// VALUES
            /// </summary>
            public const string VALUES = "VALUES";
            /// <summary>
            /// UPDATE
            /// </summary>
            public const string UPDATE = "UPDATE";
            /// <summary>
            /// DELETE
            /// </summary>
            public const string DELETE = "DELETE FROM";
            /// <summary>
            /// SET
            /// </summary>
            public const string SET = "SET";
            /// <summary>
            /// FROM
            /// </summary>
            public const string FROM = "FROM";
            /// <summary>
            /// WHERE
            /// </summary>
            public const string WHERE = "WHERE";
            /// <summary>
            /// ORDER BY
            /// </summary>
            public const string ORDER_BY = "ORDER BY";
            /// <summary>
            /// GROUP BY
            /// </summary>
            public const string GROUP_BY = "GROUP BY";
            /// <summary>
            /// HAVING
            /// </summary>
            public const string HAVING = "HAVING";
            /// <summary>
            /// AND
            /// </summary>
            public const string AND = "AND";
            /// <summary>
            /// OR
            /// </summary>
            public const string OR = "OR";
            /// <summary>
            /// LIKE
            /// </summary>
            public const string LIKE = "LIKE";
            /// <summary>
            /// JOIN
            /// </summary>
            public const string JOIN = "JOIN";
            /// <summary>
            /// RIGHT JOIN
            /// </summary>
            public const string RIGHT_JOIN = "RIGHT JOIN";
            /// <summary>
            /// LEFT JOIN
            /// </summary>
            public const string LEFT_JOIN = "LEFT JOIN";
            /// <summary>
            /// ON
            /// </summary>
            public const string ON = "ON";
            /// <summary>
            /// COUNT
            /// </summary>
            public const string COUNT = "COUNT";
            /// <summary>
            /// SUM
            /// </summary>
            public const string SUM = "SUM";
            /// <summary>
            /// MAX
            /// </summary>
            public const string MAX = "MAX";
            /// <summary>
            /// MIN
            /// </summary>
            public const string MIN = "MIN";
            /// <summary>
            /// AVG
            /// </summary>
            public const string AVG = "AVG";
            /// <summary>
            /// TOP
            /// </summary>
            public const string TOP = "TOP";
            /// <summary>
            /// TOP
            /// </summary>
            public const string DESC = "DESC";

            /// <summary>
            /// NULL
            /// </summary>
            public const string NULL = "NULL";
            /// <summary>
            /// IN
            /// </summary>
            public const string IN = "IN";
            /// <summary>
            /// NOT IN
            /// </summary>
            public const string NOT_IN = "NOT IN";
            /// <summary>
            /// NOT EXISTS
            /// </summary>
            public const string NOT_EXISTS = "NOT EXISTS";

            /// <summary>
            /// IS NOT NULL
            /// </summary>
            public const string IS_NOT_NULL = "IS NOT NULL";
            /// <summary>
            /// IS NULL
            /// </summary>
            public const string IS_NULL = "IS NULL";

            /// <summary>
            /// IF
            /// </summary>
            public const string IF = "IF";
            /// <summary>
            /// BEGIN
            /// </summary>
            public const string BEGIN = "BEGIN";
            /// <summary>
            /// END
            /// </summary>
            public const string END = "END";
            /// <summary>
            /// EXISTS
            /// </summary>
            public const string EXISTS = "EXISTS";
            /// <summary>
            /// IF NOT EXISTS
            /// </summary>
            public const string IF_NOT_EXISTS = "IF NOT EXISTS";
            /// <summary>
            /// CASE
            /// </summary>
            public const string CASE = "CASE";
            /// <summary>
            /// WHEN
            /// </summary>
            public const string WHEN = "WHEN";
            /// <summary>
            /// THEN
            /// </summary>
            public const string THEN = "THEN";
            /// <summary>
            /// ELSE
            /// </summary>
            public const string ELSE = "ELSE";

            /// <summary>
            /// GETDATE()
            /// </summary>
            public const string GETDATE = "GETDATE()";
            /// <summary>
            /// NEWID()
            /// </summary>
            public const string NEWID = "NEWID()";

            /// <summary>
            /// BEGIN TRAN
            /// </summary>
            public const string BEGIN_TRAN = "BEGIN TRAN";
            /// <summary>
            /// COMMIT TRAN
            /// </summary>
            public const string COMMIT_TRAN = "COMMIT TRAN";
            /// <summary>
            /// ROLLBACK TRAN
            /// </summary>
            public const string ROLLBACK_TRAN = "ROLLBACK TRAN";

            /// <summary>
            /// SCOPEVIDENTITY
            /// </summary>
            public const string SCOPE_IDENTITY = "SCOPE_IDENTITY()";
            /// <summary>
            /// SELECT SCOPEVIDENTITY
            /// </summary>
            public const string SELECT_SCOPE_IDENTITY = "SELECT SCOPE_IDENTITY();";

            /// <summary>
            /// EXECUTE
            /// </summary>
            public const string EXECUTE = "EXECUTE";
        }
        /// <summary>
        /// Хранимые процедуры
        /// </summary>
        public static class PROCEDURES
        {
            #region ПОЛЬЗОВАТЕЛИ (Регистрация / Авторизация / Восстановление пароля)
            /// <summary>
            /// Создает нового пользователя в базе данных
            /// </summary>
            public static class CreateNewUser
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "CreateNewUser";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    /// <summary>
                    /// Никнейм
                    /// </summary>
                    public static string NicName = "@NicName";
                    /// <summary>
                    /// Имя
                    /// </summary>
                    public static string Name = "@Name";
                    /// <summary>
                    /// Email
                    /// </summary>
                    public static string Email = "@Email";
                    /// <summary>
                    /// Пароль
                    /// </summary>
                    public static string Password = "@Password";
                    /// <summary>
                    /// Пол
                    /// </summary>
                    public static string Sex = "@Sex";
                    /// <summary>
                    /// Подписка
                    /// </summary>
                    public static string Subscribed = "@Subscribed";
                    /// <summary>
                    /// Роль
                    /// </summary>
                    public static string Role = "@Role";
                    /// <summary>
                    /// Token
                    /// </summary>
                    public static string Token = "@Token";
                }
            }
            /// <summary>
            /// Проверяет существование Email в базе данных
            /// </summary>
            public static class MailExists
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "MailExists";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string UserId = "@UserId";
                    public static string Email = "@Email";
                }
            }
            /// <summary>
            /// Проверяет существование Никнейма в базе данных
            /// </summary>
            public static class NicExists
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "NicExists";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string UserId = "@UserId";
                    public static string Nic = "@Nic";                    
                }
            }
            /// <summary>
            /// Обновляет состояние подтверждения Email в базе данных
            /// </summary>
            public static class UserEmailConfirmedEdit
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "UserEmailConfirmedEdit";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string Id = "@Id";
                    public static string Confirmed = "@Confirmed";
                }
            }
            /// <summary>
            /// Обновляет Token
            /// </summary>
            public static class UserTokenEdit
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "UserTokenEdit";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string Id = "@Id";
                    public static string Token = "@Token";
                }
            }
            /// <summary>
            /// Изменяет пароль
            /// </summary>
            public static class UserPasswordEdit
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "UserPasswordEdit";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string Id = "@Id";
                    public static string Password = "@Password";
                }
            }
            #endregion

            #region ПОЛЬЗОВАТЕЛИ
            /// <summary>
            /// Возвращает пользователя по идентификатору
            /// </summary>
            public static class UserView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "UserView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string Id = "@Id";
                }
            }
            /// <summary>
            /// Возвращает информацию о тарифах пользователя из базы данных по идентификатору
            /// </summary>
            public static class UserTariffInfoView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "UserTariffInfoView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string UserId = "@UserId";
                }
            }
            /// <summary>
            /// Возвращает информацию о прогнозах пользователя по идентификатору
            /// </summary>
            public static class UserForecastsForMonthView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "UserForecastsForMonthView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string UserId = "@UserId";
                }
            }
            /// <summary>
            /// Возвращает список прогнозов пользователя/пользователей
            /// </summary>
            public static class UserForecastsView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "UserForecastsView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string UserId = "@UserId";
                    public static string SubjectId = "@SubjectId";
                }
            }
            /// <summary>
            /// Возвращает историю изменения баланса пользователя по идентификатору
            /// </summary>
            public static class UserBalanceHistoryView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "UserBalanceHistoryView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string UserId = "@UserId";
                }
            }
            /// <summary>
            /// Возвращает активного пользователя по идентификатору
            /// </summary>
            public static class ActiveUserView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "ActiveUserView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string Id = "@Id";
                }
            }
            /// <summary>
            /// Возвращает активного пользователя по Email
            /// </summary>
            public static class ActiveUserByEmailView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "ActiveUserByEmailView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string Email = "@Email";
                }
            }
            #endregion

            #region ПОЛЬЗОВАТЕЛИ (АДМИНКА)
            /// <summary>
            /// Возвращает количество пользователей по фильтру
            /// </summary>
            public static class UsersCount
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "UsersCount";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string Nic = "@Nic";
                    public static string Email = "@Email";
                    public static string Name = "@Name";
                    public static string IsActive = "@IsActive";
                }
            }
            /// <summary>
            /// Возвращает количество пользователей по фильтру
            /// </summary>
            public static class UsersView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "UsersView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string Nic = "@Nic";
                    public static string Email = "@Email";
                    public static string Name = "@Name";
                    public static string IsActive = "@IsActive";
                    public static string From = "@From";
                    public static string To = "@To";
                }
            }
            /// <summary>
            /// Выполняет пополнение баланса пользователя
            /// </summary>
            public static class AddBalance
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "AddBalance";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string UserId = "@UserId";
                    public static string TariffId = "@TariffId";
                    public static string Sum = "@Sum";
                    public static string PeriodId = "@PeriodId";
                    public static string Comment = "@Comment";
                }
            }
            /// <summary>
            /// Создает/редактирует заявку на продление подписки пользователя
            /// </summary>
            public static class UsersTariffRequestsEdit
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "UsersTariffRequestsEdit";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    /// <summary>
                    /// Тип операции: 
                    /// 1 - Создаем заявку, 
                    /// 2 - Проставляем оплату,
                    /// 3 - Отменяем заявку,
                    /// 4 - Архивируем заявку,
                    /// 5 - Удаляем заявку
                    /// </summary>
                    public static string Operation = "@Operation";
                    public static string Id = "@Id";
                    public static string UserId = "@UserId";
                    public static string TariffId = "@TariffId";
                    public static string PeriodId = "@PeriodId";
                    public static string Sum = "@Sum";
                    public static string IsPaid = "@IsPaid";
                    public static string PaidDate = "@PaidDate";
                    public static string PaymentInfo = "@PaymentInfo";                    
                    public static string Comment = "@Comment";
                }
            }
            /// <summary>
            /// Возвращает заявки пользователя на подписки
            /// </summary>
            public static class UsersTariffRequestsView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "UsersTariffRequestsView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string UserId = "@UserId";
                    public static string IsArchive = "@IsArchive";
                }
            }
            /// <summary>
            /// Подтверждает поступление средств для указанного пользователя
            /// </summary>
            public static class PaymentConfirmed
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "PaymentConfirmed";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string UserId = "@UserId";                    
                }
            }
            /// <summary>
            /// Подтверждает отправку письма пользователя при продлении подписки
            /// </summary>
            public static class PaymentEmailSend
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "PaymentEmailSend";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string UserId = "@UserId";
                }
            }
            /// <summary>
            /// Обновляет данные пользователя в базе данных
            /// </summary>
            public static class UpdateUserProfile
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "UpdateUserProfile";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string UserId = "@UserId";
                    public static string Name = "@Name";
                    public static string Email = "@Email";
                    public static string Sex = "@Sex";
                    public static string LastName = "@LastName";
                    public static string FatherName = "@FatherName";
                    public static string IsSubscribed = "@IsSubscribed";
                    public static string Nic = "@Nic";
                }
            }
            /// <summary>
            /// Удаляет пользователя из базы данных
            /// </summary>
            public static class UserDelete
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "UserDelete";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string UserId = "@UserId";                    
                }
            }
            #endregion

            #region ПРОГНОЗЫ
            /// <summary>
            /// Создает новый прогнозы
            /// </summary>
            public static class CreateForecast
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "CreateForecast";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string ForecastDate = "@ForecastDate";
                    public static string UserId = "@UserId";
                    public static string SubjectId = "@SubjectId";
                    public static string WinAmount = "@WinAmount";                    
                }
            }
            /// <summary>
            /// Обновляет прогнозы
            /// </summary>
            public static class EditForecast
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "EditForecast";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string Id = "@Id";
                    public static string SubjectId = "@SubjectId";
                    public static string ForecastDate = "@ForecastDate";
                    public static string WinAmount = "@WinAmount";
                    public static string UserId = "@UserId";
                }
            }
            /// <summary>
            /// Удаляет прогнозы
            /// </summary>
            public static class DeleteForecast
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "DeleteForecast";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string Id = "@Id";
                }
            }
            /// <summary>
            /// Выполняет проверку участвует ли пользователь в прогнозе
            /// </summary>
            public static class ForecastHasUser
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "ForecastHasUser";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string ForecastId = "@ForecastId";
                    public static string UserId = "@UserId";
                }
            }
            /// <summary>
            /// Добавляет участника к прогнозу
            /// </summary>
            public static class AddUserToForecast
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "AddUserToForecast";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string ForecastId = "@ForecastId";
                    public static string UserId = "@UserId";
                    public static string ForecastValue = "@ForecastValue";
                }
            }
            /// <summary>
            /// Закрывает прогнозы по указанной дате
            /// </summary>
            public static class CloseForecast
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "CloseForecast";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string ForecastId = "@ForecastId";
                    public static string FactValue = "@FactValue";
                    public static string NewForecastDate = "@NewForecastDate";
                }
            }
            /// <summary>
            /// Возвращает прогноз
            /// </summary>
            public static class ForecastView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "ForecastView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string Id = "@Id";
                }
            }
            /// <summary>
            /// Возвращает пользователей, принявших участие в прогнозе
            /// </summary>
            public static class ForecastUsers
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "ForecastUsers";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string Id = "@Id";
                }
            }
            /// <summary>
            /// Возвращает список прогнозов
            /// </summary>
            public static class ForecastsView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "ForecastsView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string IsClosed = "@IsClosed";
                    public static string SubjectId = "@SubjectId";
                }
            }
            /// <summary>
            /// Возвращает список прогнозов
            /// </summary>
            public static class ForecastsList
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "ForecastsList";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string IsClosed = "@IsClosed";
                    public static string SubjectId = "@SubjectId";
                    public static string RowBegin = "@RowBegin";
                    public static string RowEnd = "@RowEnd";
                    //out
                    public static string RowTotalCount = "@RowTotalCount";
                }
            }
            /// <summary>
            /// Возвращает пользователей текущих прогнозов
            /// </summary>
            public static class CurrentForecastsUsersView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "CurrentForecastsUsersView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string Id1 = "@Id1";
                    public static string Id2 = "@Id2";
                    public static string Id3 = "@Id3";
                    public static string Id4 = "@Id4";
                }
            }
            /// <summary>
            /// Возвращает список последних победителей
            /// </summary>
            public static class LastForecastWinnersView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "LastForecastWinnersView";
            }
            /// <summary>
            /// Возвращает членов клуба
            /// </summary>
            public static class ClubTopUsersView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "ClubTopUsersView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string UsersCount = "@UsersCount";
                }
            }
            /// <summary>
            /// Возвращает состояние пользователя при добавлении в указанный прогноз
            /// </summary>
            public static class AddUserToForecastStateView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "AddUserToForecastStateView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string UserId = "@UserId";
                    public static string ForecastId = "@ForecastId";
                }
            }
            #endregion

            #region ПРЕДМЕТ ПРОГНОЗА
            /// <summary>
            /// Возвращает предмет прогноза по псевдониму
            /// </summary>
            public static class ForecastSubjectView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "ForecastSubjectView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string SubjectAlias = "@SubjectAlias";
                }
            }
            /// <summary>
            /// Возвращает предметы прогнозов
            /// </summary>
            public static class ForecastSubjectsView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "ForecastSubjectsView";
            }
            #endregion

            #region ГРАФИКИ
            /// <summary>
            /// Создает запись в таблице графиков
            /// </summary>
            public static class ChartsDataInsert{
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "ChartsDataInsert";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string SubjectId = "@SubjectId";
                    public static string ChartDate = "@ChartDate";
                    public static string ChartValue = "@ChartValue";
                }
            }
            /// <summary>
            /// Создает или обновляет запись в таблице графиков
            /// </summary>
            public static class QuotesDataEdit
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "QuotesDataEdit";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string SubjectId = "@SubjectId";
                    public static string ChartDate = "@ChartDate";
                    public static string ChartValue = "@ChartValue";
                }
            }
            /// <summary>
            /// Удаляет данные графиков
            /// </summary>
            public static class ChartsDataDelete
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "ChartsDataDelete";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string SubjectId = "@SubjectId";
                }
            }
            /// <summary>
            /// Загружает данные графиков
            /// </summary>
            public static class ChartsDataView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "ChartsDataView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    public static string SubjectId = "@SubjectId";
                }
            }
            #endregion

            #region ДОЛЖНИКИ
            /// <summary>
            /// Процедура возвращает список должников согласно заданным фильтрам
            /// </summary>
            public static class DebtorsView
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "DebtorsView";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    /// <summary>
                    /// Публикация
                    /// </summary>
                    public static string Published = "@Published";
                    /// <summary>
                    /// Идентификатор должника
                    /// </summary>
                    public static string DebtorId = "@DebtorId";
                    /// <summary>
                    /// Регион
                    /// </summary>
                    public static string RegionId = "@RegionId";
                    /// <summary>
                    /// Дата образования долга
                    /// </summary>
                    public static string DebtCreatedRange = "@DebtCreatedRange";
                    /// <summary>
                    /// Сумма долга
                    /// </summary>
                    public static string DebtAmountRange = "@DebtAmountRange";
                    /// <summary>
                    /// Цена продажи
                    /// </summary>
                    public static string SalePriceRange = "@SalePriceRange";
                    /// <summary>
                    /// Решение суда
                    /// </summary>
                    public static string CourtDecisionTypeId = "@CourtDecisionTypeId";
                    /// <summary>
                    /// Лицо (тип должника)
                    /// </summary>
                    public static string DebtorTypeId = "@DebtorTypeId";
                    /// <summary>
                    /// Сущность долга
                    /// </summary>
                    public static string DebtEssenceTypeId = "@DebtEssenceTypeId";
                    /// <summary>
                    /// Первоначальный кредитор
                    /// </summary>
                    public static string OriginalCreditorTypeId = "@OriginalCreditorTypeId";
                    /// <summary>
                    /// Продавец долга
                    /// </summary>
                    public static string DebtSellerTypeId = "@DebtSellerTypeId";
                    /// <summary>
                    /// Количество выводимых записей
                    /// </summary>
                    public static string RecFrom = "@RecFrom";
                    /// <summary>
                    /// Номер записи, начиная с которой нужно вывести записи
                    /// </summary>
                    public static string RecTo = "@RecTo";
                }
            }
            /// <summary>
            /// Процедура редактирует записи в таблице должников
            /// </summary>
            public static class DebtorsEdit
            {
                /// <summary>
                /// Название хранимой процедуры
                /// </summary>
                public static string Name = "DebtorsEdit";
                /// <summary>
                /// Параметры хранимой процедуры                 
                /// </summary>
                public static class Params
                {
                    /// <summary>
                    /// Тип операции: 1 - Создать, 2 - Изменить, 3 - Удалить, 4 - Архивировать, 5 - Активность
                    /// </summary>
                    public static string Operation = "@Operation";
                    /// <summary>
                    /// Идентификатор должника
                    /// </summary>
                    public static string Id = "@Id";
                    /// <summary>
                    /// Тип должника
                    /// </summary>
                    public static string DebtorTypeId = "@DebtorTypeId";
                    /// <summary>
                    /// Опубликован
                    /// </summary>
                    public static string Published = "@Published";
                    /// <summary>
                    /// Первоначальный кредитор
                    /// </summary>
                    public static string OriginalCreditorTypeId = "@OriginalCreditorTypeId";
                    /// <summary>
                    /// Регион должника
                    /// </summary>
                    public static string RegionId = "@RegionId";
                    /// <summary>
                    /// Населенный пункт
                    /// </summary>
                    public static string Locality = "@Locality";
                    /// <summary>
                    /// Сущность долга
                    /// </summary>
                    public static string DebtEssenceTypeId = "@DebtEssenceTypeId";
                    /// <summary>
                    /// Решение суда
                    /// </summary>
                    public static string CourtDecisionTypeId = "@CourtDecisionTypeId";
                    /// <summary>
                    /// Дата образования долга
                    /// </summary>
                    public static string DebtCreatedDate = "@DebtCreatedDate";
                    /// <summary>
                    /// Сумма долга
                    /// </summary>
                    public static string DebtAmount = "@DebtAmount";
                    /// <summary>
                    /// Сумма долга
                    /// </summary>
                    public static string SalePrice = "@SalePrice";
                    /// <summary>
                    /// Продавец долга
                    /// </summary>
                    public static string DebtSellerTypeId = "@DebtSellerTypeId";
                    /// <summary>
                    /// Контактное лицо
                    /// </summary>
                    public static string ContactPerson = "@ContactPerson";
                    /// <summary>
                    /// Контактный телефон
                    /// </summary>
                    public static string ContactPhone = "@ContactPhone";
                    /// <summary>
                    /// Дополнительный контактный телефон
                    /// </summary>
                    public static string DopPhone = "@DopPhone";
                    /// <summary>
                    /// Email
                    /// </summary>
                    public static string Email = "@Email";
                    /// <summary>
                    /// Дополнительный комментарий
                    /// </summary>
                    public static string Comment = "@Comment";
                }
            }
            #endregion
        }
    }
}