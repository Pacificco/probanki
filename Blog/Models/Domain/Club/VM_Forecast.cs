using Bankiru.Models.Domain.Users;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Club
{
    public class VM_Forecast
    {
        [HiddenInput]
        public int Id { get; set; }
        public VM_ForecastSubject Subject { get; set; }
        [Display(Name = "Предмет прогноза")]
        [Range(1, 10)]
        public byte SubjectId {
            get { return Subject.Id; }
            set { Subject.Id = value; }
        }
        [Display(Name = "Закрыт")]
        public bool IsClosed { get; set; }
        public VM_User Winner { get; set; }
        [Display(Name = "Значение прогноза")]
        public double WinValue { get; set; }
        [Display(Name = "Сумма выигрыша")]
        public double WinAmount { get; set; }
        [Display(Name = "Дата окончания прогноза")]
        [DataType(DataType.Date)] 
        public DateTime ForecastDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ReportDate { get; set; }
        public int ReportUserId { get; set; }

        public List<VM_ForecastUser> Users { get; set; }

        private string _lastError;
        public string LastError { get { return _lastError; } }
        public static readonly ILog log = LogManager.GetLogger(typeof(ForecastManager));

        public VM_Forecast()
        {
            Subject = new VM_ForecastSubject();
            Users = new List<VM_ForecastUser>();
            Clear();
        }
        public void Clear()
        { 
            Id = -1;
            Subject.Clear();
            IsClosed = false;
            Winner = null;
            Winner = new VM_User();
            WinValue = 0.0F;
            WinAmount = 0.0F;
            ForecastDate = DateTime.Now;
            CreateDate = DateTime.Now;
            ReportDate = DateTime.Now;
            ReportUserId = -1;

            Users.Clear();

            _lastError = String.Empty;
        }
        
        public bool SetFieldValue(string fName, object fValue)
        {
            switch (fName)
            {
                case "Id":
                    Id = (int)fValue;
                    break;
                case "SubjectId":
                    Subject.Id = (byte)fValue;
                    break;
                case "IsClosed":
                    IsClosed = (bool)fValue;
                    break;
                case "Winner":
                    Winner.Id = fValue == DBNull.Value ? -1 : (int)fValue;
                    break;
                case "WinnerNic":
                    Winner.Nic = fValue == DBNull.Value ? "" : (string)fValue;
                    break;
                case "WinValue":
                    WinValue = fValue == DBNull.Value ? 0.0F : (double)fValue;
                    break;
                case "WinAmount":
                    WinAmount = fValue == DBNull.Value ? 0.0f : (double)fValue;
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
                    _lastError = String.Format("Поле {0} не определено в классе {1}!", fName, this.GetType().ToString());
                    log.Error(_lastError);
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