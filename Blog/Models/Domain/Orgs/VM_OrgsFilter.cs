using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_OrgsFilters
    {
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Display(Name = "Регион")]
        public Guid RegionId { get; set; }
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }        
        [Display(Name = "Активность")]
        public EnumBoolType IsActive  { get; set; }
        [Display(Name = "Начальная буква")]
        public string Letter { get; set; }

        public VM_OrgsFilters()
        {
            Title = "";
            RegionId = Guid.Empty;
            CategoryId = 0;
            IsActive = EnumBoolType.None;
            Letter = null;
        }
        public void Assign(VM_OrgsFilters obj)
        {
            Title = obj.Title;
            RegionId = obj.RegionId;
            CategoryId = obj.CategoryId;
            IsActive = obj.IsActive;
            Letter = obj.Letter;
        }
        public object GetFilterAsRouteValues(int currentPage)
        {
            return new
            {
                categoryId = CategoryId,
                regionId = RegionId,
                title = Title,
                IsActive = (int)IsActive,
                page = currentPage
            };
        }
    }
}