using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Club
{
    public class VM_ForecastHeader
    {
        //public List<VM_ForecastSubject> Subjects { get; set; }
        public List<VM_Forecast> Forecasts { get; set; }
        public int CurItemId { get; set; }

        public VM_ForecastHeader()
        {
            //Subjects = new List<VM_ForecastSubject>();
            Forecasts = new List<VM_Forecast>();
            CurItemId = 0;
        }
    }
}