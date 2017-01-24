﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Users
{
    public class VM_UserTariffInfo
    {
        public int UserId { get; set; }
        public double Balance { get; set; }
        public VM_ForecastTariff Tariff { get; set; }
        public byte ForecastTries { get; set; }
        public DateTime? TariffBeginDate { get; set; }
        public DateTime? TariffEndDate { get; set; }
        public bool IsTariffLetterSent { get; set; }
        public DateTime ReportDate { get; set; }
        public int ReportUserId { get; set; }

        public VM_UserTariffInfo()
        {
            Clear();
        }
        public void Clear()
        {
            UserId = -1;
            Balance = 0.0F;
            Tariff = VM_ForecastTariff.Undefined;
            ForecastTries = 0;
            TariffBeginDate = null;
            TariffEndDate = null;
            IsTariffLetterSent = false;
            ReportDate = DateTime.Now;
            ReportUserId = -1;
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
                    TariffBeginDate = fValue == DBNull.Value ? null : (DateTime?)fValue;
                    break;
                case "ForecastEndDate":
                    TariffEndDate = fValue == DBNull.Value ? null : (DateTime?)fValue;
                    break;
                case "IsTariffLetterSent":
                    IsTariffLetterSent = (bool)fValue;
                    break;
                case "ReportDate":
                    ReportDate = (DateTime)fValue;
                    break;
                case "UserReportId":
                    ReportUserId = (int)fValue;
                    break;
                case "Balance":
                    Balance = (double)fValue;
                    break;                
                default:

                    return false;
            }
            return true;
        }
    }
}