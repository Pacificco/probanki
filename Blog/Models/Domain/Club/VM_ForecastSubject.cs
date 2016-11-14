using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Club
{
    public class VM_ForecastSubject
    {
        [HiddenInput]
        public byte Id { get; set; }
        [Display(Name = "Псевдоним")]
        public string Alias { get; set; }
        [Display(Name = "Название категории")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Иконка")]
        public string Icon { get; set; }
        [Display(Name = "Meta-заголовок")]
        public string MetaTitle { get; set; }
        [Display(Name = "Meta-описание")]
        public string MetaDescriptions { get; set; }
        [Display(Name = "Keywords")]
        public string MetaKeywords { get; set; }
        [Display(Name = "NoFollow")]
        public bool MetaNoFollow { get; set; }
        [Display(Name = "NoIndex")]
        public bool MetaNoIndex { get; set; }
        [Display(Name = "Активна")]
        public bool IsActive { get; set; }

        public void Clear()
        {
            Id = 0;
            Alias = "";
            Name = "";
            Description = "";
            Icon = "";
            MetaTitle = "";
            MetaDescriptions = "";
            MetaKeywords = "";
            MetaNoFollow = false;
            MetaNoIndex = false;
            IsActive = true;
        }
        public void Assign(VM_ForecastSubject obj)
        {
            if (obj == null)
                return;

            Alias = obj.Alias;
            Name = obj.Name;
            Description = obj.Description;
            Icon = obj.Icon;
            MetaTitle = obj.MetaTitle;
            MetaDescriptions = obj.MetaDescriptions;
            MetaKeywords = obj.MetaKeywords;
            MetaNoFollow = obj.MetaNoFollow;
            MetaNoIndex = obj.MetaNoIndex;
            IsActive = obj.IsActive;
        }
    }
}