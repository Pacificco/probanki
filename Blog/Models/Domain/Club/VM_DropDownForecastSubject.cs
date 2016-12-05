using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Club
{
    public class VM_DropDownForecastSubject
    {
        public EnumFirstDropDownItem FirstItem { get; set; }
        public List<VM_ForecastSubject> Items { get; set; }
        public int SelectedId { get; set; }
        public string Name { get; set; }

        public VM_DropDownForecastSubject()
        {
            FirstItem = EnumFirstDropDownItem.None;
            SelectedId = 0;
            Items = new List<VM_ForecastSubject>();
            Name = "";
        }
    }
}