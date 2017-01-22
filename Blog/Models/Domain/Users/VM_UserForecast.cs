using Bankiru.Models.Domain.Club;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Users
{
    public class VM_UserForecast
    {
        public VM_User User { get; set; }
        public VM_Forecast Forecast { get; set; }
        public double? Value { get; set; }
        public DateTime? ValueDate { get; set; }

        public VM_UserForecast()
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