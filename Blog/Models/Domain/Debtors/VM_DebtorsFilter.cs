using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Debtors
{
    /// <summary>
    /// Фильтр по должникам для Front
    /// </summary>
    public class VM_DebtorsFilter
    {
        /// <summary>
        /// Публикация
        /// </summary>        
        [Display(Name = "Активность")]
        public EnumBoolType Published { get; set; }
        /// <summary>
        /// Идентификатор должника
        /// </summary>
        [Display(Name = "Id")]
        [HiddenInput]
        public int DebtorId { get; set; }
        /// <summary>
        /// Тип должника
        /// </summary>
        [Display(Name = "Должник")]
        public EnumDebtorType DebtorType { get; set; }
        /// <summary>
        /// Первоначальный кредитор
        /// </summary>
        [Display(Name = "Первоначальный кредитор")]
        public EnumOriginalCreditorType OriginalCreditorType { get; set; }
        /// <summary>
        /// Регион должника
        /// </summary>
        [Display(Name = "Регион")]
        public Guid RegionId { get; set; }
        /// <summary>
        /// Сущность долга
        /// </summary>
        [Display(Name = "Сущность долга")]
        public EnumDebtEssenceType DebtEssenceType { get; set; }
        /// <summary>
        /// Решение суда
        /// </summary>
        [Display(Name = "Решение суда")]
        public EnumCourtDecisionType CourtDecisionType { get; set; }
        /// <summary>
        /// Дата образования долга: 0 - за весь период, 1 - за последний месяц, 2 - за последний год
        /// </summary>
        [Display(Name = "Дата образования долга")]
        public int DebtCreatedRange { get; set; }        
        /// <summary>
        /// Сумма долга: 0 - Любая сумма, 1 - до 50000, 2 - до 100000 3 - до 300000, 4 - до 500000, 5 - до 1000000, 5 - больше 1000000
        /// </summary>
        [Display(Name = "Сумма долга")]
        public int DebtAmountRange { get; set; }
        /// <summary>
        /// Цена продажи: 0 - Любая сумма, 1 - до 50000, 2 - до 100000 3 - до 300000, 4 - до 500000, 5 - до 1000000, 5 - больше 1000000
        /// </summary>
        [Display(Name = "Цена продажи")]
        public int SalePriceRange { get; set; }
        /// <summary>
        /// Тип продавца долга
        /// </summary>
        [Display(Name = "Продавец долга")]
        public EnumDebtSellerType DebtSellerType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VM_DebtorsFilter()
        {
            Published = EnumBoolType.True;
            DebtorId = 0;
            DebtorType = EnumDebtorType.Undefind;
            OriginalCreditorType = EnumOriginalCreditorType.Undefind;
            RegionId = Guid.Empty;
            DebtEssenceType = EnumDebtEssenceType.Undefind;
            CourtDecisionType = EnumCourtDecisionType.Undefind;
            DebtCreatedRange = 0;
            DebtAmountRange = 0;
            SalePriceRange = 0;
            DebtSellerType = EnumDebtSellerType.Undefind;
        }

        public void Assign(VM_DebtorsFilter obj)
        {
            DebtorId = obj.DebtorId;
            Published = obj.Published;
            DebtorType = obj.DebtorType;
            OriginalCreditorType = obj.OriginalCreditorType;
            RegionId = obj.RegionId;
            DebtEssenceType = obj.DebtEssenceType;
            CourtDecisionType = obj.CourtDecisionType;
            DebtCreatedRange = obj.DebtCreatedRange;
            DebtAmountRange = obj.DebtAmountRange;
            SalePriceRange = obj.SalePriceRange;
            DebtSellerType = obj.DebtSellerType;
        }
        public object GetFilterAsRouteValues(int currentPage)
        {
            return new
            {
                //categoryId = CategoryId,
                //regionId = RegionId,
                //title = Title,
                //IsActive = (int)IsActive,
                page = currentPage
            };
        }
    }
}