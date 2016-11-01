using Bankiru.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Forecasts
{
    public class VM_Forecast
    {
        public int Id { get; set; }
        public ForecastSubject Subject { get; set; }
        public bool IsClosed { get; set; }
        public VM_User Winner { get; set; }
        public double WinValue { get; set; }
        public double WinAmount { get; set; }
        public DateTime ForecastDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ReportDate { get; set; }
        public int ReportUserId { get; set; }

        public VM_Forecast()
        {
            Clear();
        }
        public void Clear()
        { 
            Id = -1;
            Subject = ForecastSubject.Undefined;
            IsClosed = false;
            Winner = null;
            Winner = new VM_User();
            WinValue = 0.0F;
            WinAmount = 0.0F;
            ForecastDate = DateTime.Now;
            CreateDate = DateTime.Now;
            ReportDate = DateTime.Now;
            ReportUserId = -1;
        }
    }

    public enum ForecastSubject
    {
        Undefined = 0,
        USD = 1,
        EUR = 2,
        Oil = 3,
        StockSberBank = 4
    }
}