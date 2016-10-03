using Bankiru.Models.Domain.OrgsCategories;
using Bankiru.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_Orgs
    {
        public VM_OrgCategory CurrentCategory { get; set; }
        public VM_OrgsFilters Filters { get; set; }
        public List<VM_OrgItem> Items { get; set; }
        public VM_PagingInfo PagingInfo { get; set; }

        public VM_Orgs()
        {
            CurrentCategory = new VM_OrgCategory();
            Filters = new VM_OrgsFilters();
            Items = new List<VM_OrgItem>();
            PagingInfo = new VM_PagingInfo();
        }
    }
}