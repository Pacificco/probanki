using Bankiru.Models.DataBase;
using Bankiru.Models.Infrastructure;
using Bankiru.Models.OutApi.CBR;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Bankiru.Models.OutApi
{
    public class ChartManager
    {        
        public static readonly ILog log = LogManager.GetLogger(typeof(ChartManager));
        private static NumberFormatInfo _numberFormatInfo = new NumberFormatInfo() { NumberDecimalSeparator = "." };

        private string _lastError;
        public string LastError { get; set; }

        #region YAHOO CHARTS
        public List<ChartObject> LoadChartDataFromYahoo(string subject)
        {
            try
            {
                string csvData;
                string baseUrl = "http://chartapi.finance.yahoo.com/instrument/1.0/{0}/chartdata;type=quote;range=3m/csv/";
                using (WebClient web = new WebClient())
                {
                    csvData = web.DownloadString(String.Format(baseUrl, subject));
                }                
                
                return _parseYahooData(csvData);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
                return null;
            }
        }
        private List<ChartObject> _parseYahooData(string csvData)
        {
            try
            {
                List<ChartObject> result = new List<ChartObject>();
                string[] rows = csvData.Replace("\r", "").Split('\n');

                bool rowFinded = false;
                foreach (string row in rows)
                {
                    if (String.IsNullOrEmpty(row)) continue;
                    if (row.IndexOf("volume") == 0)
                    {
                        rowFinded = true;
                        continue;
                    }

                    if (rowFinded)
                    {
                        string[] cols = row.Split(',');

                        ChartObject obj = new ChartObject();

                        obj.Date = Convert.ToDateTime(String.Format("{0}{1}{2}{3}-{4}{5}-{6}{7} 00:00:00"
                            , cols[0][0]
                            , cols[0][1]
                            , cols[0][2]
                            , cols[0][3]
                            , cols[0][4]
                            , cols[0][5]
                            , cols[0][6]
                            , cols[0][7]));
                        obj.Close = Convert.ToDecimal(cols[1], _numberFormatInfo);
                        obj.High = Convert.ToDecimal(cols[2], _numberFormatInfo);
                        obj.Low = Convert.ToDecimal(cols[3], _numberFormatInfo);
                        obj.Open = Convert.ToDecimal(cols[4], _numberFormatInfo);
                        obj.Volume = Convert.ToDecimal(cols[5], _numberFormatInfo);

                        result.Add(obj);
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
                return null;
            }
        }        
        #endregion

        #region CBR CHARTS
        public List<ChartObject> LoadChartDataFromCBR(string subject)
        {
            try
            {
                XML_CBRAnswer xmlAnswer = null;
                DateTime dateTo = DateTime.Now.ToUniversalTime();
                DateTime dateFrom = DateTime.Now.ToUniversalTime().AddMonths(-3);
                WebResponseCBRAnswer answer = WebRequestsCBR.GetCurrentCourses(dateFrom, dateTo, subject);
                if (answer.HttpCode == 200) //успешный  запрос
                {
                    using (XmlReader xmlReader = new XmlTextReader(answer.Stream))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(XML_CBRAnswer));
                        xmlAnswer = (XML_CBRAnswer)xmlSerializer.Deserialize(xmlReader);
                    }
                }
                else
                {
                    log.Error("Не удалось загрузить динамику курса валют!");
                    return null;
                }
                if (xmlAnswer == null)
                    return null;
                return _parseCBRData(xmlAnswer);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return null;
            }
        }
        private List<ChartObject> _parseCBRData(XML_CBRAnswer xmlData)
        {
            try
            {
                List<ChartObject> result = new List<ChartObject>();
                if (xmlData.CurrencyItems != null && xmlData.CurrencyItems.Length > 0)
                {
                    ChartObject chartItem = null;
                    foreach (var item in xmlData.CurrencyItems)
                    {
                        chartItem = new ChartObject();
                        chartItem.Date = DateTime.Parse(item.Date);
                        chartItem.Open = Convert.ToDecimal(item.Value);
                        result.Add(chartItem);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return null;
            }
        }        
        #endregion

        public List<ChartObject> LoadChartDataFromDb(int subjectId)
        {
            try
            {
                List<ChartObject> result = new List<ChartObject>();
                SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.ChartsDataView.Name, GlobalParams.GetConnection());
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandTimeout = 15;
                command.Parameters.AddWithValue(DbStruct.PROCEDURES.ChartsDataView.Params.SubjectId, subjectId);
                lock (GlobalParams._DBAccessLock)
                {
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                ChartObject obj = null;
                                while (reader.Read())
                                {
                                    obj = new ChartObject();
                                    obj.SubjectId = reader.GetByte(reader.GetOrdinal("SubjectId"));
                                    obj.Close = reader.GetDecimal(reader.GetOrdinal("ChartValue"));
                                    obj.Date = reader.GetDateTime(reader.GetOrdinal("ChartDate"));
                                    result.Add(obj);
                                }
                            }
                        }
                        return result;
                    }
                    catch (Exception ex)
                    {
                        log.Error(String.Format("Ошибка во время выполнения хранимой процедуры {0}!\n{1}",
                            DbStruct.PROCEDURES.ChartsDataView.Name, ex.ToString()));
                        return null;
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
                    DbStruct.PROCEDURES.ChartsDataView.Name, ex.ToString()));
                return null;
            }
        }
    }
}
