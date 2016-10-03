using Bankiru.Models.Domain.Categories;
using Bankiru.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Articles
{
    public class VM_Articles
    {
        public VM_Category CurrentCategory { get; set; }
        public VM_ArtsFilters Filters { get; set; }
        public List<VM_ArtItem> Items { get; set; }
        public VM_PagingInfo PagingInfo { get; set; }

        public VM_Articles()
        {
            CurrentCategory = null;
            Filters = new VM_ArtsFilters();
            Items = new List<VM_ArtItem>();
            PagingInfo = new VM_PagingInfo();
        }
    }
}