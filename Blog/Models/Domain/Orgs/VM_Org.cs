using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Orgs
{
    public class VM_Org
    {
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name="Псевдоним")]
        public string Alias { get; set; }
        [Display(Name="Название организации")]
        [Required(ErrorMessage = "Вы не указали название организации")]
        public string Title { get; set; }
        [Display(Name="Описание")]
        [Required(ErrorMessage = "Вы не указали описание организации")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]        
        [AllowHtml]
        public string Descriptions { get; set; }
        [Display(Name="Комментарий (внутренний)")]
        public string Comment { get; set; }
        [Display(Name="Иконка")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Upload)]
        public string Icon { get; set; }
        [Display(Name="Заголовок иконки")]
        public string IconTitle { get; set; }
        [Display(Name="Альтернативное название иконки")]
        public string IconAlt { get; set; }
        [Display(Name = "Категория")]
        [Range(1, 10)]
        public int CategoryId { get; set; }
        [Display(Name = "Родительская организация")]
        public int ParentId { get; set; }
        [Display(Name = "Регион")]
        public Guid RegionId { get; set; }
        [MaxLength(255, ErrorMessage = "Максимальная длина Meta-заголовка не должна превышать 255 символов")]
        [Display(Name="Meta-заголовок")]        
        public string MetaTitle { get; set; }
        [MaxLength(255, ErrorMessage = "Максимальная длина Meta-ключевых слов не должна превышать 255 символов")]
        [Display(Name="Meta-ключевые слова")]
        public string MetaKeywords { get; set; }
        [MaxLength(255, ErrorMessage = "Максимальная длина Meta-описания не должна превышать 255 символов")]
        [Display(Name="Meta-описание")]
        public string MetaDescriptions { get; set; }
        [Display(Name = "No Follow")]
        public bool MetaNoFollow { get; set; }
        [Display(Name = "No Index")]
        public bool MetaNoIndex { get; set; }        
        [Display(Name="Дата создания")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        [Display(Name="Дата последнего изменения")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime UpdatedAt { get; set; }
        [Display(Name="Дата последнего просмотра")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime? LastVisitedAt { get; set; }        
        [Display(Name="Показов")]
        public int Hits { get; set; }
        [Display(Name="Активна")]
        public bool IsActive { get; set; }

        public string CategoryAlias { get; set; }
        public int PointsCount { get; set; }
        public int ReviewsCount { get; set; }
    }
}