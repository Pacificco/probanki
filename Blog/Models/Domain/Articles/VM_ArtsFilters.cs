using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Articles
{
    public class VM_ArtsFilters
    {
        [Display(Name = "Название")]
        public string Title { get; set; }        
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }        
        [Display(Name = "Активность")]
        public EnumBoolType IsActive  { get; set; }
        [Display(Name = "Центральные публикации")]
        public bool IsCentral{ get; set; }

        public VM_ArtsFilters()
        {
            Title = "";
            CategoryId = 0;
            IsActive = EnumBoolType.None;
            IsCentral = false;
        }
        public void Assign(VM_ArtsFilters obj)
        {
            Title = obj.Title;
            CategoryId = obj.CategoryId;
            IsActive = obj.IsActive;
            IsCentral = obj.IsCentral;
        }
        public object GetFilterAsRouteValues(int currentPage)
        {
            return new
            {
                categoryId = CategoryId,
                title = Title,
                IsActive = (int)IsActive,
                IsCentral = IsCentral,
                page = currentPage
            };
        }
    }
}