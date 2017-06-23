using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Users
{
    public class VM_UserTariffRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public byte TariffId { get; set; }
        public string PeriodId { get; set; }
        public double Sum { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidDate { get; set; }
        public string PaymentInfo { get; set; }
        public bool IsArchive { get; set; }
        public DateTime? ArchiveDate { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Returned { get; set; }
        public DateTime? ReturnedDate { get; set; }

        public string TariffName { get; set; }
        public string PeriodName { get; set; }

        public VM_UserTariffRequest()
        {
            Clear();
        }
        public void Clear()
        {
            Id = 0;
            UserId = 0;
            TariffId = 0;
            PeriodId = String.Empty;
            Sum = 0.0F;
            IsPaid = false;
            PaidDate = null;
            PaymentInfo = String.Empty;
            IsArchive = false;
            ArchiveDate = null;
            CreateDate = DateTime.Now;
            Returned = false;
            ReturnedDate = null;

            TariffName = String.Empty;
            PeriodName = String.Empty;
        }
    }
}