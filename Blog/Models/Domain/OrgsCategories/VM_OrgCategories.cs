using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.OrgsCategories
{
    public class VM_OrgCategories
    {
        public int currentId { get; set; }
        public List<VM_OrgCategoryItem> items { get; set; }

        public VM_OrgCategories()
        {
            currentId = -1;
            items = new List<VM_OrgCategoryItem>();
        }
    }
}