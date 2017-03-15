using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Club
{
    public class VM_TariffPaymentEmail
    {
        public string UserName { get; set; }
        public string Tariff { get; set; }
        public string Period { get; set; }
        public double Sum { get; set; }
    }
}