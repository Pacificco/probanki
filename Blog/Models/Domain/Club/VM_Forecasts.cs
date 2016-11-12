using Bankiru.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Club
{
    public class VM_Forecasts
    {
        public VM_ForecastSubject CurrentSubject { get; set; }
        public VM_ForecastsFilters Filters { get; set; }
        public List<VM_Forecast> Items { get; set; }
        public VM_PagingInfo PagingInfo { get; set; }

        public VM_Forecasts()
        {
            CurrentSubject = new VM_ForecastSubject();
            Filters = new VM_ForecastsFilters();
            Items = new List<VM_Forecast>();
            PagingInfo = new VM_PagingInfo();
        }
    }
}