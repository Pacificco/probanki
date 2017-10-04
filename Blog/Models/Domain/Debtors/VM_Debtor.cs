using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Debtors
{
    /// <summary>
    /// Должник
    /// </summary>
    public class VM_Debtor
    {
        #region ПОЛЯ И СВОЙСТВА
        /// <summary>
        /// Идентификатор должника
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Опубликован
        /// </summary>
        public bool Published { get; set; }
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
        public Guid RegionId { get; set; }
        /// <summary>
        /// Населенный пункт
        /// </summary>
        public string Locality { get; set; }
        /// <summary>
        /// Сущность долга
        /// </summary>
        public EnumDebtEssenceType DebtEssenceType { get; set; }
        /// <summary>
        /// Решение суда
        /// </summary>
        public EnumCourtDecisionType CourtDecisionType { get; set; }
        /// <summary>
        /// Дата образования долга
        /// </summary>
        public DateTime DebtCreatedDate { get; set; }
        /// <summary>
        /// Сумма долга
        /// </summary>
        public decimal DebtAmount { get; set; }
        /// <summary>
        /// Сумма долга
        /// </summary>
        public decimal SalePrice { get; set; }
        /// <summary>
        /// Тип продавца долга
        /// </summary>
        public EnumDebtSellerType DebtSellerType { get; set; }
        /// <summary>
        /// Контактное лицо
        /// </summary>
        public string ContactPerson { get; set; }
        /// <summary>
        /// Контактный телефон
        /// </summary>
        public int ContactPhone { get; set; }
        /// <summary>
        /// Дополнительный контактный телефон
        /// </summary>
        public int? DopPhone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Дополнительный комментарий
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Дата создания записи в базе данных
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Дата обновления записи в базе данных
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// Архивная запись
        /// </summary>
        public bool IsArchived { get; set; }
        /// <summary>
        /// Дата архивации
        /// </summary>
        public DateTime? ArchivedDate { get; set; }

        /// <summary>
        /// Наименование региона
        /// </summary>
        [HiddenInput]
        public string RegionName { get; set; }
        /// <summary>
        /// Наименование Типа должника
        /// </summary>
        [HiddenInput]
        public string DebtorTypeName { get; set; }
        /// <summary>
        /// Наименование Первоначального кредитора
        /// </summary>
        [HiddenInput]
        public string OriginalCreditorTypeName { get; set; }
        /// <summary>
        /// Наименование Сущности долга
        /// </summary>
        [HiddenInput]
        public string DebtEssenceTypeName { get; set; }        
        /// <summary>
        /// Наименование Решения суда
        /// </summary>
        [HiddenInput]
        public string CourtDecisionTypeName { get; set; }
        /// <summary>
        /// Наименование Типа продавца долга
        /// </summary>
        [HiddenInput]
        public string DebtSellerTypeName { get; set; }


        /// <summary>
        /// Сообщение о последней ошибке
        /// </summary>
        private string _lastError;
        /// <summary>
        /// Сообщение о последней ошибке
        /// </summary>
        public string LastError { get { return _lastError; } }
        #endregion

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public VM_Debtor()
        {
            Clear();
        }
        /// <summary>
        /// Очищает объект
        /// </summary>
        public void Clear()
        {
            Id = 0;
            Published = true;
            DebtorType = EnumDebtorType.Undefind;
            OriginalCreditorType = EnumOriginalCreditorType.Undefind;
            RegionId = Guid.Empty;
            Locality = String.Empty;
            DebtEssenceType = EnumDebtEssenceType.Undefind;
            CourtDecisionType = EnumCourtDecisionType.Undefind;
            DebtCreatedDate = DateTime.Now;
            DebtAmount = 0;
            SalePrice = 0;
            DebtSellerType = EnumDebtSellerType.Undefind;
            ContactPerson = String.Empty;
            ContactPhone = 0;
            DopPhone = null;
            Email = String.Empty;
            Comment = String.Empty;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            IsArchived = false;
            ArchivedDate = null;

            RegionName = String.Empty;
            DebtorTypeName = String.Empty;
            OriginalCreditorTypeName = String.Empty;
            DebtEssenceTypeName = String.Empty;
            CourtDecisionTypeName = String.Empty;
            DebtSellerTypeName = String.Empty;

            _lastError = String.Empty;
        }
    }

    /// <summary>
    /// Тип должника
    /// </summary>
    public enum EnumDebtorType
    {
        /// <summary>
        /// Не определен
        /// </summary>
        Undefind = 0,
        /// <summary>
        /// Юридическое лицо
        /// </summary>
        LegalPerson,
        /// <summary>
        /// Физическое лицо
        /// </summary>
        PhysicalPerson
    }
    /// <summary>
    /// Тип первоначального кредитора
    /// </summary>
    public enum EnumOriginalCreditorType
    {
        /// <summary>
        /// Не определен
        /// </summary>
        Undefind = 0,
        /// <summary>
        /// Банк
        /// </summary>
        Bank,
        /// <summary>
        /// МФО
        /// </summary>
        MFO,
        /// <summary>
        /// Юридическое лицо
        /// </summary>
        LegalPerson,
        /// <summary>
        /// Физическое лицо
        /// </summary>
        PhysicalPerson
    }
    /// <summary>
    /// Тип сущности долга
    /// </summary>
    public enum EnumDebtEssenceType
    {
        /// <summary>
        /// Не определен
        /// </summary>
        Undefind = 0,
        /// <summary>
        /// Банковский кредит
        /// </summary>
        BankLoan,
        /// <summary>
        /// Займ МФО
        /// </summary>
        MFOLoan,
        /// <summary>
        /// Частный долг
        /// </summary>
        PhysicalPerson,
        /// <summary>
        /// Прочее
        /// </summary>
        Other
    }
    /// <summary>
    /// Тип решения суда
    /// </summary>
    public enum EnumCourtDecisionType
    {
        /// <summary>
        /// Не определен
        /// </summary>
        Undefind = 0,
        /// <summary>
        /// Есть
        /// </summary>
        Has,
        /// <summary>
        /// Нет
        /// </summary>
        HasNot
    }
    /// <summary>
    /// Тип продавца долга
    /// </summary>
    public enum EnumDebtSellerType
    {
        /// <summary>
        /// Не определен
        /// </summary>
        Undefind = 0,
        /// <summary>
        /// Банк
        /// </summary>
        Bank,
        /// <summary>
        /// МФО
        /// </summary>
        MFO,
        /// <summary>
        /// Юридическое лицо
        /// </summary>
        LegalPerson,
        /// <summary>
        /// Физическое лицо
        /// </summary>
        PhysicalPerson
    }
}