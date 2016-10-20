using Bankiru.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Users
{
    public class VM_Users
    {      
        public VM_UsersFilters Filters { get; set; }
        public List<VM_UserItem> Items { get; set; }
        public VM_PagingInfo PagingInfo { get; set; }

        public VM_Users()
        {
            Filters = new VM_UsersFilters();
            Items = new List<VM_UserItem>();
            PagingInfo = new VM_PagingInfo();
        }
    }
}