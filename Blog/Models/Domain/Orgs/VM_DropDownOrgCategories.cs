using Bankiru.Models.Domain.OrgsCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_DropDownOrgCategories
    {
        public EnumFirstDropDownItem FirstItem { get; set; }
        public List<VM_OrgCategoryItem> Items { get; set; }
        public int SelectedId { get; set; }
        public string Name { get; set; }

        public VM_DropDownOrgCategories()
        {
            FirstItem = EnumFirstDropDownItem.None;
            SelectedId = 0;
            Items = new List<VM_OrgCategoryItem>();
            Name = "";
        }
    }
}