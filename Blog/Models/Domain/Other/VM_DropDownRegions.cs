using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Other
{
    public class VM_DropDownRegions
    {
        public EnumFirstDropDownItem FirstItem { get; set; }
        public List<VM_Region> Items { get; set; }
        public Guid SelectedId { get; set; }
        public string Name { get; set; }

        public VM_DropDownRegions()
        {
            FirstItem = EnumFirstDropDownItem.None;
            Items = new List<VM_Region>();
            SelectedId = Guid.Empty;
            Name = "";
        }
    }
}