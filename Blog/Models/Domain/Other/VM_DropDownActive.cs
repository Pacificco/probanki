using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Other
{
    public class VM_DropDownActive
    {
        public EnumFirstDropDownItem FirstItem { get; set; }        
        public EnumBoolType SelectedId { get; set; }
        public string Name { get; set; }

        public VM_DropDownActive()
        {
            FirstItem = EnumFirstDropDownItem.None;            
            SelectedId = EnumBoolType.None;
            Name = "";
        }
    }
}