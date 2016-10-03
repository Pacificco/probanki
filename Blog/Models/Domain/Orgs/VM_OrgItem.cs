using Bankiru.Models.Domain.OrgsCategories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_OrgItem
    {
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Псевдоним")]
        public string Alias { get; set; }
        [Display(Name = "Название организации")]
        public string Title { get; set; }        
        [Display(Name = "Комментарий")]
        public string Comment { get; set; }
        [Display(Name = "Иконка")]
        public string Icon { get; set; }        
        [Display(Name = "Категория")]
        public VM_OrgCategoryItem Category { get; set; }        
        [Display(Name = "Родительская организация")]
        public VM_OrgItem Parent { get; set; }        
        [Display(Name = "Регион")]
        public string Region { get; set; }        
        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Дата последнего изменения")]
        public DateTime ChangedAt { get; set; }
        [Display(Name = "Дата последнего просмотра")]
        public DateTime LastVisitedAt { get; set; }
        [Display(Name = "Показов")]
        public int Hits { get; set; }
        [Display(Name = "Активен")]
        public bool IsActive { get; set; }

        public int PointsCount { get; set; }
        public int ReviewsCount { get; set; }

        public void Assign(VM_OrgItem obj)
        {
            Id = obj.Id;
            Alias = obj.Alias;
            Title = obj.Title;
            Comment = obj.Comment;
            Icon = obj.Icon;
            if (Category == null & obj.Category != null)
            {
                Category = new VM_OrgCategoryItem();
                Category.Assign(obj.Category);
            }
            if (obj.Parent == null)
                Parent = null;
            else
            {
                if (Parent == null)
                    Parent = new VM_OrgItem();
                Parent.Assign(obj.Parent);
            }
            Region = obj.Region;
            CreatedAt = obj.CreatedAt;
            ChangedAt = obj.ChangedAt;
            LastVisitedAt = obj.LastVisitedAt;
            Hits = obj.Hits;
            IsActive = obj.IsActive;

            PointsCount = 0;
            ReviewsCount = 0;
        }
    }
}