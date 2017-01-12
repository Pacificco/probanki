using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Users
{
    public class VM_UserForecastInfo
    {
        #region ПОЛЯ и СВОЙСТВАv
        public int UserId { get; set; }
        public double Balance { get; set; }
        public VM_ForecastTariff Tariff { get; set; }
        public byte ForecastTries { get; set; }
        public DateTime? ForecastBeginDate { get; set; }
        public DateTime? ForecastEndDate { get; set; }
        public bool IsTariffLetterSent { get; set; }
        public DateTime ReportDate { get; set; }
        public int UserReportId { get; set; }
        public int ForecastTriesOnThisMonth { get; set; }
        #endregion

        public VM_UserForecastInfo()
        {
            Clear();
        }
        public void Clear()
        {
            UserId = -1;
            Balance = 0.0F;
            Tariff = VM_ForecastTariff.Undefined;
            ForecastTries = 0;
            ForecastBeginDate = null;
            ForecastEndDate = null;
            IsTariffLetterSent = false;
            ReportDate = DateTime.Now;
            UserReportId = -1;
            ForecastTriesOnThisMonth = 0;
        }

        public bool SetFieldValue(string fName, object fValue)
        {
            switch (fName)
            {
                case "UserId":
                    UserId = (int)fValue;
                    break;
                case "TariffId":
                    Tariff = (VM_ForecastTariff)(byte)fValue;
                    break;
                case "ForecastTries":
                    ForecastTries = (byte)fValue;
                    break;
                case "ForecastBeginDate":
                    ForecastBeginDate = fValue == DBNull.Value ? null : (DateTime?)fValue;
                    break;
                case "ForecastEndDate":
                    ForecastEndDate = fValue == DBNull.Value ? null : (DateTime?)fValue;
                    break;
                case "IsTariffLetterSent":
                    IsTariffLetterSent = (bool)fValue;
                    break;
                case "ReportDate":
                    ReportDate = (DateTime)fValue;
                    break;
                case "UserReportId":
                    UserReportId = (int)fValue;
                    break;
                case "Balance":
                    Balance = (double)fValue;
                    break;
                case "forecastTryCount":
                    ForecastTriesOnThisMonth = (int)fValue;
                    break;
                default:

                    return false;
            }
            return true;
        }
    }
}