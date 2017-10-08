using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Other
{
    public class VM_StringDropDown
    {
        public EnumFirstDropDownItem FirstItem { get; set; }        
        public int SelectedId { get; set; }
        public string Name { get; set; }

        public VM_StringDropDown()
        {
            FirstItem = EnumFirstDropDownItem.None;            
            SelectedId = 0;
            Name = "";
        }
    }
}