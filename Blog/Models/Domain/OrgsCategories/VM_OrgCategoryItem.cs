using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.OrgsCategories
{
    public class VM_OrgCategoryItem
    {
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Псевдоним")]
        public string Alias { get; set; }
        [Display(Name = "Название категории")]
        public string Title { get; set; }

        public void Assign(VM_OrgCategoryItem obj)
        {
            Id = obj.Id;
            Alias = obj.Alias;
            Title = obj.Title;
        }
    }
}