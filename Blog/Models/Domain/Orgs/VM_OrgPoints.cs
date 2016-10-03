using Bankiru.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_OrgPoints
    {
        public VM_Org Org { get; set; }
        public VM_OrgsPointsFilters Filters { get; set; }
        public List<VM_OrgPoint> Items { get; set; }
        public VM_PagingInfo PagingInfo { get; set; }
        public int ATMCount { get; set; }
        public int OfficeCount { get; set; }

        public VM_OrgPoints()
        {
            Org = new VM_Org();
            Filters = new VM_OrgsPointsFilters();
            Items = new List<VM_OrgPoint>();
            PagingInfo = new VM_PagingInfo();
            ATMCount = 0;
            OfficeCount = 0;
        }
    }
}