using Bankiru.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.News
{
    public class VM_NewsList

    {
        public VM_NewsFilters Filters { get; set; }
        public List<VM_NewsItem> Items { get; set; }
        public VM_PagingInfo PagingInfo { get; set; }

        public VM_NewsList()
        {
            Filters = new VM_NewsFilters();
            Items = new List<VM_NewsItem>();
            PagingInfo = new VM_PagingInfo();
        }
    }
}