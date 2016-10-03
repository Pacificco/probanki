using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_OrgsPointsFilters
    {
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Display(Name = "Регион")]
        public Guid RegionId { get; set; }
        [Display(Name = "Тип")]
        public EnumOrgPointType PointType { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }        
        [Display(Name = "Активность")]
        public EnumBoolType IsActive  { get; set; }

        public VM_OrgsPointsFilters()
        {
            Title = "";
            RegionId = Guid.Empty;
            Address = "";
            PointType = EnumOrgPointType.None;
            IsActive = EnumBoolType.None;
        }
        public void Assign(VM_OrgsPointsFilters obj)
        {
            Title = obj.Title;
            RegionId = obj.RegionId;
            Address = obj.Address;
            PointType = obj.PointType;
            IsActive = obj.IsActive;
        }
        public object GetFilterAsRouteValues(int currentPage)
        {
            return new
            {
                address = Address,
                regionId = RegionId,
                title = Title,
                isActive = (int)IsActive,
                pointType = (int)PointType,
                page = currentPage
            };
        }
    }
}