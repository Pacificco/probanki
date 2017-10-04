using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Domain.Debtors
{
    /// <summary>
    /// Фильтр должников
    /// </summary>
    public class DebtorsFilter
    {
        /// <summary>
        /// Публикация
        /// </summary>
        public bool Published { get; set; }
        /// <summary>
        /// Идентификатор должника
        /// </summary>
        public int? DebtorId { get; set; }
        /// <summary>
        /// Тип должника
        /// </summary>
        public EnumDebtorType DebtorType { get; set; }
        /// <summary>
        /// Первоначальный кредитор
        /// </summary>
        public EnumOriginalCreditorType OriginalCreditorType { get; set; }
        /// <summary>
        /// Регион должника
        /// </summary>
        public Guid? RegionId { get; set; }
        /// <summary>
        /// Сущность долга
        /// </summary>
        public EnumDebtEssenceType DebtEssenceType { get; set; }
        /// <summary>
        /// Решение суда
        /// </summary>
        public EnumCourtDecisionType CourtDecisionType { get; set; }
        /// <summary>
        /// Дата образования долга: 0 - за весь период, 1 - за последний месяц, 2 - за последний год
        /// </summary>
        public int DebtCreatedRange { get; set; }        
        /// <summary>
        /// Сумма долга: 0 - Любая сумма, 1 - до 50000, 2 - до 100000 3 - до 300000, 4 - до 500000, 5 - до 1000000, 5 - больше 1000000
        /// </summary>
        public int DebtAmountRange { get; set; }
        /// <summary>
        /// Цена продажи: 0 - Любая сумма, 1 - до 50000, 2 - до 100000 3 - до 300000, 4 - до 500000, 5 - до 1000000, 5 - больше 1000000
        /// </summary>
        public int SalePriceRange { get; set; }
        /// <summary>
        /// Тип продавца долга
        /// </summary>
        public EnumDebtSellerType DebtSellerType { get; set; }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public DebtorsFilter()
        {
            Clear();
        }
        /// <summary>
        /// Очищает объект
        /// </summary>
        public void Clear()
        {
            Published = true;
            DebtorId = null;
            DebtorType = EnumDebtorType.Undefind;
            OriginalCreditorType = EnumOriginalCreditorType.Undefind;
            RegionId = null;
            DebtEssenceType = EnumDebtEssenceType.Undefind;
            CourtDecisionType = EnumCourtDecisionType.Undefind;
            DebtCreatedRange = 0;
            DebtAmountRange = 0;
            SalePriceRange = 0;
            DebtSellerType = EnumDebtSellerType.Undefind;
        }
        /// <summary>
        /// Присваивает значения указанного объекта
        /// </summary>
        /// <param name="obj">Фильтр должников с Front End</param>
        public void Assign(VM_DebtorsFilter obj)
        {
            Published = obj.Published;
            DebtorId = obj.DebtorId == 0 ? null : (int?)obj.DebtorId;
            DebtorType = obj.DebtorType;
            OriginalCreditorType = obj.OriginalCreditorType;
            RegionId = obj.RegionId == Guid.Empty ? null : (Guid?)obj.RegionId;
            DebtEssenceType = obj.DebtEssenceType;
            CourtDecisionType = obj.CourtDecisionType;
            DebtCreatedRange = obj.DebtCreatedRange;
            DebtAmountRange = obj.DebtAmountRange;
            SalePriceRange = obj.SalePriceRange;
            DebtSellerType = obj.DebtSellerType;
        }
    }
}