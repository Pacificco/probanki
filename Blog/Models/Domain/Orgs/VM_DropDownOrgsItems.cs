using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_DropDownOrgItems
    {
        public EnumFirstDropDownItem FirstItem { set; get; }
        public List<VM_OrgItem> Items { set; get; }
        public int SelectedId { set; get; }
        public string Name { get; set; }

        public VM_DropDownOrgItems()
        {
            FirstItem = EnumFirstDropDownItem.None;
            Items = new List<VM_OrgItem>();
            SelectedId = 0;
            Name = "";
        }
    }
}