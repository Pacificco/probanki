using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChartsApi
{
    public class ChartManager
    {
        private const string _baseUrl = "http://chartapi.finance.yahoo.com/instrument/1.0/SBER.ME/chartdata;type=quote;range=3m/csv/";
        public static readonly ILog log = LogManager.GetLogger(typeof(ChartManager));

        private string _lastError;
        public string LastError { get; set; }

        public List<ChartObject> LoadChartData(string subject)
        {
            try
            {
                string csvData;

                using (WebClient web = new WebClient())
                {
                    csvData = web.DownloadString(_baseUrl);
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

                        obj.Date = Convert.ToDateTime(cols[0]);
                        obj.close = Convert.ToDecimal(cols[1]);
                        obj.high = Convert.ToDecimal(cols[2]);
                        obj.low = Convert.ToDecimal(cols[3]);
                        obj.open = Convert.ToDecimal(cols[4]);
                        obj.volume = Convert.ToDecimal(cols[5]);

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
    }
}
