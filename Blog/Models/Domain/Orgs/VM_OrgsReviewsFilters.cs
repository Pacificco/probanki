using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_OrgsReviewsFilters
    {
        [Display(Name = "Текст отзыва")]
        public string ReviewText { get; set; }        
        [Display(Name = "Активность")]
        public EnumBoolType IsActive  { get; set; }
        [Display(Name = "Статус")]
        public EnumBoolType Confirmed  { get; set; }

        public VM_OrgsReviewsFilters()
        {
            ReviewText = "";            
            Confirmed = EnumBoolType.None;
            IsActive = EnumBoolType.None;
        }
        public void Assign(VM_OrgsReviewsFilters obj)
        {
            ReviewText = obj.ReviewText;
            Confirmed = obj.Confirmed;
            IsActive = obj.IsActive;
        }
        public object GetFilterAsRouteValues(int currentPage)
        {
            return new
            {                
                review_text = ReviewText,
                isActive = (int)IsActive,
                confirm_status = (int)Confirmed,
                page = currentPage
            };
        }
    }
}