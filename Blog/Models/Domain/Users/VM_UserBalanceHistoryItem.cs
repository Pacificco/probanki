using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Users
{
    public class VM_UserBalanceHistoryItem
    {
        #region ПОЛЯ и СВОЙСТВА
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Sum { get; set; }
        /// <summary>
        /// Операция: 1 - пополнение, 2 - списание
        /// </summary>
        public byte Operation { get; set; }
        public DateTime ReportDate { get; set; }
        public int ReportUserId { get; set; }
        public string Comment { get; set; }
        #endregion

        public VM_UserBalanceHistoryItem()
        {
            Clear();
        }
        public void Clear()
        {
            Id = -1;
            UserId = -1;
            Sum = 0.0F;
            Operation = 0;
            ReportDate = DateTime.Now;
            ReportUserId = -1;
            Comment = String.Empty;
        }

        public bool SetFieldValue(string fName, object fValue)
        {
            switch (fName)
            {
                case "Id":
                    Id = (int)fValue;
                    break;
                case "UserId":
                    UserId = (int)fValue;
                    break;
                case "Sum":
                    Sum = (double)fValue;
                    break;
                case "Operation":
                    Operation = (byte)fValue;
                    break;
                case "ReportDate":
                    ReportDate = (DateTime)fValue;
                    break;
                case "ReportUserId":
                    ReportUserId = (int)fValue;
                    break;
                case "Comment":
                    Comment = (string)fValue;
                    break;                
                default:

                    return false;
            }
            return true;
        }
    }
}