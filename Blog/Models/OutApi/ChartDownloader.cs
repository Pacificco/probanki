﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using log4net;
using System.Xml;
using System.Xml.Serialization;
using Currencies;
using System.Globalization;
using System.Data.SqlClient;
using Bankiru.Models.DataBase;
using Bankiru.Models.Infrastructure;
using Bankiru.Models.Domain.Club;

namespace Bankiru.Models.OutApi
{
    public class ChartDownloader : IHttpModule
    {
        static Timer timer;
        long _interval = 7200000; //2 час
        static object _synclock = new object();
        static bool _isDownLoading = false;
        private ChartManager _manager = new ChartManager();
        private List<VM_ForecastSubject> _subjects;

        public static readonly ILog log = LogManager.GetLogger(typeof(ChartDownloader));

        public void Init(HttpApplication app)
        {
            ForecastManager manager = new ForecastManager();
            _subjects = manager.GetForecastSubjects();
            if (_subjects == null || _subjects.Count == 0)
            {
                log.Error(manager.LastError);
                return;
            }
            timer = new Timer(new TimerCallback(_loadQuotes), null, 0, _interval);
        }
        private void _loadCharts(object obj)
        {
            if (_isDownLoading) return;
            _isDownLoading = true;
            try
            {
                List<ChartObject> rows = null;
                foreach (VM_ForecastSubject s in _subjects)
                {
                    if (String.IsNullOrEmpty(s.Ticker))
                        continue;
                    if (String.IsNullOrEmpty(s.SourceType))
                        continue;

                    switch(s.SourceType)
                    {
                        case "cbr":
                            rows = _manager.LoadChartDataFromCBR(s.Ticker);
                            break;
                        case "yahoo":
                            rows = _manager.LoadChartDataFromYahoo(s.Ticker);
                            break;
                    }
                    if (rows == null || rows.Count == 0)
                    {
                        log.Error(String.Format("Ошибка во время загрузки статической информации для графика ({0})!\nНе удалось загрузить данные", s.Name));
                        continue;
                    }
                    if (_deleteFromDataBase(s.Id))
                    {
                        foreach (ChartObject row in rows)
                        {
                            _saveToDataBase(row, s.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Ошибка во время загрузки статической информации для графиков!\n" + ex.ToString());
            }
            finally
            {
                _isDownLoading = false;
            }
        }
        private void _loadQuotes(object obj)
        {
            if (_isDownLoading) return;
            _isDownLoading = true;
            try
            {
                List<ChartObject> rows = null;
                //ChartObject row2 = null;
                foreach (VM_ForecastSubject s in _subjects)
                {
                    if (String.IsNullOrEmpty(s.Ticker))
                        continue;
                    if (String.IsNullOrEmpty(s.SourceType))
                        continue;

                    switch (s.SourceType)
                    {
                        case "cbr":
                            //rows = _manager.LoadQuotesDataFromCBR(s.Ticker, null);
                            rows = _manager.LoadQuotesDataFromCBR(s.Ticker, _getDateForSBR());
                            break;
                        case "yahoo":
                            rows = _manager.LoadQuotesDataFromYahoo(s.Ticker);
                            break;
                    }
                    if (rows == null)
                    {
                        log.Error(String.Format("Ошибка во время загрузки статической информации для графика ({0})!\nНе удалось загрузить данные", s.Name));
                        continue;
                    }
                    foreach(var row in rows)
                        _saveQoutesToDataBase(row, s.Id);
                    //if (row2 != null)
                    //    _saveQoutesToDataBase(row2, s.Id);
                }
            }
            catch (Exception ex)
            {
                log.Error("Ошибка во время загрузки статической информации для графиков!\n" + ex.ToString());
            }
            finally
            {
                _isDownLoading = false;
            }
        }
        private bool _saveToDataBase(ChartObject row, int subjectId)
        {
            try
            {
                SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ChartsDataInsert.Name, GlobalParams.GetConnection());
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandTimeout = 15;
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.ChartsDataInsert.Params.SubjectId, subjectId);
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.ChartsDataInsert.Params.ChartValue, row.Open);
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.ChartsDataInsert.Params.ChartDate, row.Date);
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
                        log.Error(String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                            DbStruct.PROCEDURES.ChartsDataInsert.Name, ex.ToString()));
                        return false;
                    }
                    finally
                    {
                        if (command != null)
                            command.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                    DbStruct.PROCEDURES.ChartsDataInsert.Name, ex.ToString()));
                return false;
            }
        }
        private bool _saveQoutesToDataBase(ChartObject row, int subjectId)
        {
            try
            {
                SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.QuotesDataEdit.Name, GlobalParams.GetConnection());
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandTimeout = 15;
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.QuotesDataEdit.Params.SubjectId, subjectId);
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.QuotesDataEdit.Params.ChartValue, row.Close);
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.QuotesDataEdit.Params.ChartDate, row.Date);
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
                        log.Error(String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                            DbStruct.PROCEDURES.QuotesDataEdit.Name, ex.ToString()));
                        return false;
                    }
                    finally
                    {
                        if (command != null)
                            command.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                    DbStruct.PROCEDURES.QuotesDataEdit.Name, ex.ToString()));
                return false;
            }
        }
        private bool _deleteFromDataBase(int subjectId)
        {
            try
            {
                SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ChartsDataDelete.Name, GlobalParams.GetConnection());
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandTimeout = 15;
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.ChartsDataDelete.Params.SubjectId, subjectId);
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
                        log.Error(String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                            DbStruct.PROCEDURES.ChartsDataDelete.Name, ex.ToString()));
                        return false;
                    }
                    finally
                    {
                        if (command != null)
                            command.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                    DbStruct.PROCEDURES.ChartsDataDelete.Name, ex.ToString()));
                return false;
            }
        }
        private DateTime _getDateForSBR()
        {
            switch(DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                    return DateTime.Now.AddDays(1);
                case DayOfWeek.Friday:
                    return DateTime.Now.AddDays(3);
                case DayOfWeek.Saturday:
                    return DateTime.Now.AddDays(2);
                case DayOfWeek.Sunday:
                    return DateTime.Now.AddDays(1);
                default:
                    return DateTime.Now.AddDays(1);
            }
        }

        public void Dispose()
        {

        }
    }
}