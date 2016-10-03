using Bankiru.Models.Domain.Categories;
using Bankiru.Models.Domain.OrgsCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Articles
{
    public class VM_DropDownCategories
    {
        public EnumFirstDropDownItem FirstItem { get; set; }
        public List<VM_Category> Items { get; set; }
        public int SelectedId { get; set; }
        public string Name { get; set; }

        public VM_DropDownCategories()
        {
            FirstItem = EnumFirstDropDownItem.None;
            SelectedId = 0;
            Items = new List<VM_Category>();
            Name = "";
        }
    }
}