using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.News
{
    public class VM_NewsFilters
    {
        [Display(Name = "Название")]
        public string Title { get; set; }        
        [Display(Name = "Активность")]
        public EnumBoolType IsActive  { get; set; }

        public VM_NewsFilters()
        {
            Title = "";            
            IsActive = EnumBoolType.None;
        }
        public void Assign(VM_NewsFilters obj)
        {
            Title = obj.Title;
            IsActive = obj.IsActive;
        }
        public object GetFilterAsRouteValues(int currentPage)
        {
            return new
            {
                title = Title,
                IsActive = (int)IsActive,
                page = currentPage
            };
        }
    }
}