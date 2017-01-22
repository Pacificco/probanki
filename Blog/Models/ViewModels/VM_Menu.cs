using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.ViewModels
{
    public class VM_Menu
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public List<VM_MenuItem> Items { get; set; }

        public VM_Menu()
        {
            Icon = String.Empty;
            Title = String.Empty;
            Items = new List<VM_MenuItem>();
        }
    }

    public class VM_MenuItem
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public bool IsActive { get; set; }
        public bool IsCurrent { get; set; }
        public List<VM_MenuItem> Items { get; set; }

        public VM_MenuItem()
        {
            Icon = String.Empty;
            Title = String.Empty;
            Alias = String.Empty;
            IsActive = false;
            IsCurrent = false;
            Items = new List<VM_MenuItem>();
        }
    }
}