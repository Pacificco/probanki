using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_DropDownOrgPointTypes
    {
        public EnumFirstDropDownItem FirstItem { get; set; }
        public List<string> Items { get; set; }
        public int SelectedId { get; set; }
        public string Name { get; set; }

        public VM_DropDownOrgPointTypes()
        {
            FirstItem = EnumFirstDropDownItem.None;
            SelectedId = 0;
            Items = new List<string>();
            Name = "";
        }
    }
}