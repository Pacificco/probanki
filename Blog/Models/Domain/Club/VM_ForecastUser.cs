using Bankiru.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Club
{
    public class VM_ForecastUser
    {
        public VM_User User { get; set; }
        public VM_Forecast Forecast { get; set; }
        public double? Value { get; set; }
        public DateTime? ValueDate { get; set; }

        public VM_ForecastUser()
        {
            User = new VM_User();
            Forecast = new VM_Forecast();
            Clear();
        }
        public void Clear()
        {
            User.Clear();
            Forecast.Clear();
            Value = null;
            ValueDate = null;
        }
    }
}