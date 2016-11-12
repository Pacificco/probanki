using Bankiru.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Club
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
        public DateTime? ReportDate { get; set; }
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
        
        public bool SetFieldValue(string fName, object fValue)
        {
            switch (fName)
            {
                case "Id":
                    Id = (int)fValue;
                    break;
                case "SubjectId":
                    Subject = (ForecastSubject)(byte)fValue;
                    break;
                case "IsClosed":
                    IsClosed = (bool)fValue;
                    break;
                case "WinnerId":
                    Winner.Id = fValue == DBNull.Value ? -1 : (int)fValue;
                    break;
                case "WinnerNic":
                    Winner.Nic = fValue == DBNull.Value ? "" : (string)fValue;
                    break;
                case "WinValue":
                    WinValue = fValue == DBNull.Value ? 0.0F : (double)fValue;
                    break;
                case "WinAmount":
                    WinAmount = fValue == DBNull.Value ? 0.0F : (double)fValue;
                    break;
                case "ForecastDate":
                    ForecastDate = (DateTime)fValue;
                    break;
                case "CreateDate":
                    CreateDate = (DateTime)fValue;
                    break;
                case "ReportDate":
                    ReportDate = fValue == DBNull.Value ? null : (DateTime?)fValue;
                    break;
                case "ReportUserId":
                    ReportUserId = fValue == DBNull.Value ? -1 : (int)fValue;
                    break;                
                default:

                    return false;
            }
            return true;
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