using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Users
{
    public class VM_UsersFilters
    {
        [Display(Name = "Никнейм")]
        public string Nic { get; set; }        
        [Display(Name = "Email")]
        public string Email { get; set; }        
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Активность")]
        public EnumBoolType IsActive  { get; set; }

        public VM_UsersFilters()
        {
            Nic = "";
            Email = "";
            IsActive = EnumBoolType.None;
        }
        public void Assign(VM_UsersFilters obj)
        {
            Nic = obj.Nic;
            Email = obj.Email;
            IsActive = obj.IsActive;
        }
        public object GetFilterAsRouteValues(int currentPage)
        {
            return new
            {
                Email = Email,
                title = Nic,
                IsActive = (int)IsActive,
                page = currentPage
            };
        }
    }
}