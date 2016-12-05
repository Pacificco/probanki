using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Club
{
    public class VM_ForecastsFilters
    {      
        [Display(Name = "Предмет прогноза")]
        public byte SubjectId { get; set; }        
        [Display(Name = "Активность")]
        public EnumBoolType IsArchive  { get; set; }

        public VM_ForecastsFilters()
        {
            SubjectId = 0;
            IsArchive = EnumBoolType.None;
        }
        public void Assign(VM_ForecastsFilters obj)
        {
            SubjectId = obj.SubjectId;
            IsArchive = obj.IsArchive;
        }
        public object GetFilterAsRouteValues(int currentPage)
        {
            return new
            {
                subjectId = SubjectId,
                IsArchive = (int)IsArchive,
                page = currentPage
            };
        }
    }
}