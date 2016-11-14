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
        public DateTime ReportDate { get; set; }
        public double Value { get; set; }

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
            ReportDate = DateTime.MinValue;
            Value = 0.0F;
        }
    }
}