using Bankiru.Models.DataBase;
using Bankiru.Models.Domain;
using Bankiru.Models.Domain.Orgs;
using Bankiru.Models.Domain.Other;
using Bankiru.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Helpers
{
    public static class DbHelper
    {
        public static string LastError = "";

        public static List<VM_Region> GetRegions()
        {
            string SQLQuery = _sqlGetRegions();
            List<VM_Region> regions = new List<VM_Region>();
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
                        LastError = "Ошибка во время загрузки регионов!\nСервер вернул ответ NULL.";
                        return null;
                    }

                    if (_reader.HasRows)
                    {
                        while (_reader.Read())
                        {
                            regions.Add(new VM_Region()
                            {
                                AoGuid = _reader.GetGuid(0),
                                FormalName = _reader.GetString(1),
                                ShortName = _reader.GetString(2)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    LastError = "Ошибка во время загрузки регионов!\n" + ex.ToString();
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
            regions.Sort((a, b) => {
                if (a.ShortName == "г" & b.ShortName == "г")
                    return a.FormalName.CompareTo(b.FormalName);
                if (a.ShortName == "г")
                    return -1;
                if (b.ShortName == "г")
                    return 1;
                return a.FormalName.CompareTo(b.FormalName);
            });
            return regions;
        }
        private static string _sqlGetRegions()
        {
            string SqlQuery = String.Empty;
            SqlQuery += DbStruct.SE.SELECT + " ";
            SqlQuery += DbStruct.Regions.FIELDS.AoGuid + ",";
            SqlQuery += DbStruct.Regions.FIELDS.FormalName + ",";
            SqlQuery += DbStruct.Regions.FIELDS.ShortName;
            SqlQuery += " " + DbStruct.SE.FROM + " ";
            SqlQuery += DbStruct.Regions.TABLENAME;
            SqlQuery += " " + DbStruct.SE.ORDER_BY + " ";
            SqlQuery += DbStruct.Regions.FIELDS.FormalName;
            SqlQuery += ";";
            return SqlQuery;
        }
        public static string GetPartUseTags(string text, int length, string tag = "</p>")
        {
            //return text;
            string result = text;
            if (!String.IsNullOrEmpty(text) && text.Length > length)
            {
                int i = text.IndexOf(tag);
                if (i != -1)
                {
                    do
                    {
                        if (i < length)
                            result = text.Substring(0, i + tag.Length);
                        else
                            break;
                        i = text.IndexOf(tag, i + tag.Length - 1);
                    }
                    while (text.Length < length & i < length & i != -1);
                    if (String.IsNullOrEmpty(text))
                        result = text;
                    else
                        result = result.Insert(result.Length - tag.Length, ".....");
                }
            }
            if (result.IndexOf("<p>") == -1)
                return "<p>" + result + "</p>";
            else
                return result;
        }
        /// <summary>
        /// Возвращает дату и время в формате, использующемся в SQL-запросах
        /// </summary>
        /// <param name="Value">Дата и время</param>
        /// /// <param name="dFormat">Возвращаемый формат дата и время</param>
        /// <returns>Строка</returns>
        public static string FormatSQLDateTime(DateTime Value, SQLDateFormat dFormat)
        {
            string Result = String.Empty;
            switch (dFormat)
            {
                case SQLDateFormat.sdfDateTime:
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
                case SQLDateFormat.sdfDate:
                    Result += Value.Year.ToString();
                    Result += Value.Month.ToString().Length == 1 ? "0" + Value.Month.ToString() : Value.Month.ToString();
                    Result += Value.Day.ToString().Length == 1 ? "0" + Value.Day.ToString() : Value.Day.ToString();
                    break;
                case SQLDateFormat.sdfTime:
                    Result += Value.Hour.ToString().Length == 1 ? "0" + Value.Hour.ToString() : Value.Hour.ToString();
                    Result += ":";
                    Result += Value.Minute.ToString().Length == 1 ? "0" + Value.Minute.ToString() : Value.Minute.ToString();
                    Result += ":";
                    Result += Value.Second.ToString().Length == 1 ? "0" + Value.Second.ToString() : Value.Second.ToString();
                    break;
                case SQLDateFormat.sdfDateTime_TimeMin:
                    Result += Value.Year.ToString();
                    Result += Value.Month.ToString().Length == 1 ? "0" + Value.Month.ToString() : Value.Month.ToString();
                    Result += Value.Day.ToString().Length == 1 ? "0" + Value.Day.ToString() : Value.Day.ToString();
                    Result += " ";
                    Result += "00:00:00";
                    break;
                case SQLDateFormat.sdfDateTime_TimeMax:
                    Result += Value.Year.ToString();
                    Result += Value.Month.ToString().Length == 1 ? "0" + Value.Month.ToString() : Value.Month.ToString();
                    Result += Value.Day.ToString().Length == 1 ? "0" + Value.Day.ToString() : Value.Day.ToString();
                    Result += " ";
                    Result += "23:59:59";
                    break;
            }

            return "'" + Result + "'";
        }
    }
}