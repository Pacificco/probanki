using Bankiru.Models.Helpers;
using Bankiru.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Debtors
{
    /// <summary>
    /// Должник (модель)
    /// </summary>
    public class VM_Debtor
    {
        #region ПОЛЯ И СВОЙСТВА
        /// <summary>
        /// Идентификатор должника
        /// </summary>
        [HiddenInput]
        public int Id { get; set; }
        /// <summary>
        /// Опубликован
        /// </summary>
        [Display(Name = "Активен")]
        public bool Published { get; set; }
        /// <summary>
        /// Тип должника
        /// </summary>
        [Display(Name = "Должник")]
        [Required(ErrorMessage = "Необходимо указать Должника!")]
        [Range(1, 100, ErrorMessage = "Необходимо указать Должника!")]
        public int DebtorType { get; set; }
        /// <summary>
        /// Первоначальный кредитор
        /// </summary>
        [Display(Name = "Первоначальный кредитор")]
        [Range(1, 100, ErrorMessage = "Необходимо указать Первоначального кредитора!")]
        public int OriginalCreditorType { get; set; }
        /// <summary>
        /// Регион должника
        /// </summary>
        [Display(Name = "Регион")]
        public Guid RegionId { get; set; }
        /// <summary>
        /// Населенный пункт
        /// </summary>
        [Display(Name = "Населенный пункт")]
        public string Locality { get; set; }
        /// <summary>
        /// Сущность долга
        /// </summary>
        [Display(Name = "Сущность долга")]
        [Range(1, 100, ErrorMessage = "Необходимо указать Сущность долга!")]
        public int DebtEssenceType { get; set; }
        /// <summary>
        /// Решение суда
        /// </summary>
        [Display(Name = "Решение суда")]
        [Range(1, 100, ErrorMessage = "Необходимо указать Решение суда!")]
        public int CourtDecisionType { get; set; }
        /// <summary>
        /// Дата образования долга
        /// </summary>
        [Display(Name = "Дата образования долга")]
        public DateTime DebtCreatedDate { get; set; }
        /// <summary>
        /// Сумма долга
        /// </summary>
        [Display(Name = "Сумма долга")]
        //[Range(10, 1000000000000, ErrorMessage = "Укажите корректное значение Суммы долга")]
        [Required(ErrorMessage = "Необходимо указать Сумму долга!")]
        public string DebtAmount { get; set; }
        /// <summary>
        /// Сумма долга
        /// </summary>        
        [Display(Name = "Цена продажи")]
        [Required(ErrorMessage = "Необходимо указать Цену продажи!")]
        public string SalePrice { get; set; }
        /// <summary>
        /// Продавец долга
        /// </summary>
        [Display(Name = "Продавец долга")]
        [Range(1, 100, ErrorMessage = "Необходимо указать Продавца долга!")]
        public int DebtSellerType { get; set; }
        /// <summary>
        /// Контактное лицо
        /// </summary>
        [Display(Name = "Контактное лицо")]
        [Required(ErrorMessage = "Укажите контактное лицо!")]
        public string ContactPerson { get; set; }
        /// <summary>
        /// Контактный телефон
        /// </summary>
        [Display(Name = "Контактный телефон")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Укажите контактный телефон!")]
        public string ContactPhone { get; set; }
        /// <summary>
        /// Дополнительный контактный телефон
        /// </summary>
        [Display(Name = "Дополнительный телефон")]
        [DataType(DataType.Text)]
        public string DopPhone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Display(Name = "Email")]
        //[DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Укажите Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный Email!")]
        public string Email { get; set; }
        /// <summary>
        /// Дополнительный комментарий
        /// </summary>
        [Display(Name = "Дополнительный комментарий")]
        [DataType(DataType.MultilineText)]
        [MaxLength(2000, ErrorMessage = "Максимальная длина комментария не должна превышать 2000 символов!")]
        public string Comment { get; set; }
        /// <summary>
        /// Дата создания записи в базе данных
        /// </summary>
        [Display(Name = "Дата создания записи")]
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Дата обновления записи в базе данных
        /// </summary>
        [Display(Name = "Дата изменения записи")]
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// Архивная запись
        /// </summary>
        [Display(Name = "Архивная")]
        public bool IsArchived { get; set; }
        /// <summary>
        /// Соглашение принято
        /// </summary>
        public bool AgreementAccept { get; set; }
        /// <summary>
        /// Дата архивации
        /// </summary>
        [Display(Name = "Дата архивации")]
        public DateTime? ArchivedDate { get; set; }
        [Display(Name = "Введите код с картинки")]
        [Required(ErrorMessage = "Код с картинки указан не верно!")]
        public string CaptchaCode { get; set; }

        /// <summary>
        /// Сообщение об успешном создании
        /// </summary>
        [HiddenInput]
        public EnumEditState EditState { get; set; }        
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
            DebtorType = 0;
            OriginalCreditorType = 0;
            RegionId = Guid.Empty;
            Locality = String.Empty;
            DebtEssenceType = 0;
            CourtDecisionType = 0;
            DebtCreatedDate = DateTime.Now;
            DebtAmount = String.Empty;
            SalePrice = String.Empty;
            DebtSellerType = 0;
            ContactPerson = String.Empty;
            ContactPhone = String.Empty;
            DopPhone = String.Empty;
            Email = String.Empty;
            Comment = String.Empty;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            IsArchived = false;
            ArchivedDate = null;

            CaptchaCode = "";
            AgreementAccept = false;
        }
        /// <summary>
        /// Присваивает значения указанного объекта
        /// </summary>
        /// <param name="obj">Должник</param>
        public void Assign(Debtor obj)
        {
            Id = obj.Id;
            Published = obj.Published;
            DebtorType = (int)obj.DebtorType;
            OriginalCreditorType = (int)obj.OriginalCreditorType;
            RegionId = obj.RegionId;
            Locality = obj.Locality;
            DebtEssenceType = (int)obj.DebtEssenceType;
            CourtDecisionType = (int)obj.CourtDecisionType;
            DebtCreatedDate = obj.DebtCreatedDate;
            DebtAmount = obj.DebtAmount.ToString(GlobalParams.LocalCultureInfo);
            SalePrice = obj.SalePrice.ToString(GlobalParams.LocalCultureInfo);
            DebtSellerType = (int)obj.DebtSellerType;
            ContactPerson = obj.ContactPerson;
            ContactPhone = obj.ContactPhone.ToString();
            DopPhone = obj.DopPhone == 0 ? String.Empty : obj.DopPhone.ToString();
            Email = obj.Email;
            Comment = obj.Comment;
            CreatedAt = obj.CreatedAt;
            UpdatedAt = obj.UpdatedAt;
            IsArchived = obj.IsArchived;
            ArchivedDate = obj.ArchivedDate;

            CaptchaCode = String.Empty;
            AgreementAccept = false;
        }
    }

    /// <summary>
    /// Должник
    /// </summary>
    public class Debtor
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
        /// Продавец долга
        /// </summary>
        public EnumDebtSellerType DebtSellerType { get; set; }
        /// <summary>
        /// Контактное лицо
        /// </summary>
        public string ContactPerson { get; set; }
        /// <summary>
        /// Контактный телефон
        /// </summary>
        public Int64 ContactPhone { get; set; }
        /// <summary>
        /// Дополнительный контактный телефон
        /// </summary>
        public Int64? DopPhone { get; set; }
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
        public string RegionName { get; set; }
        /// <summary>
        /// Наименование Типа должника
        /// </summary>
        public string DebtorTypeName { get; set; }
        /// <summary>
        /// Наименование Первоначального кредитора
        /// </summary>
        public string OriginalCreditorTypeName { get; set; }
        /// <summary>
        /// Наименование Сущности долга
        /// </summary>
        public string DebtEssenceTypeName { get; set; }
        /// <summary>
        /// Наименование Решения суда
        /// </summary>
        public string CourtDecisionTypeName { get; set; }
        /// <summary>
        /// Наименование Типа продавца долга
        /// </summary>
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
        public Debtor()
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
        /// <summary>
        /// Присваивает значения модели должника
        /// </summary>
        /// <param name="model">Модель должника</param>
        /// <param name="modelErrors">Ошибки в модели</param>
        /// <returns>Логическое значение</returns>
        public bool Assign(VM_Debtor model, out Dictionary<string, string> modelErrors)
        {
            modelErrors = new Dictionary<string, string>();
            try
            {
                Id = model.Id;
                Published = model.Published;
                DebtorType = (EnumDebtorType)model.DebtorType;
                OriginalCreditorType = (EnumOriginalCreditorType)model.OriginalCreditorType;
                if (model.RegionId == Guid.Empty)
                {
                    modelErrors.Add("RegionId", "Необходимо указать регион!");
                    RegionId = Guid.Empty;
                }
                else
                    RegionId = model.RegionId;
                Locality = model.Locality;
                DebtEssenceType = (EnumDebtEssenceType)model.DebtEssenceType;
                CourtDecisionType = (EnumCourtDecisionType)model.CourtDecisionType;
                DebtCreatedDate = model.DebtCreatedDate;
                decimal d = 0m;
                if (!TextHelper.DecimalParse(model.DebtAmount, out d))
                {
                    modelErrors.Add("DebtAmount", "Некорректное значение Суммы долга!");
                    DebtAmount = 0m;
                }
                else
                    DebtAmount = d;
                d = 0m;
                if (!TextHelper.DecimalParse(model.SalePrice, out d))
                {
                    modelErrors.Add("DebtAmount", "Некорректное значение Цены продажи!");
                    SalePrice = 0m;
                }
                else
                    SalePrice = d;                 
                DebtSellerType = (EnumDebtSellerType)model.DebtSellerType;
                ContactPerson = model.ContactPerson;
                string phoneString = TextHelper.GetAsDigits(model.ContactPhone.Replace("+7",""));
                if (phoneString.Length != 10)
                {
                    modelErrors.Add("ContactPhone", "Некорректное значение Контактного телефона!");
                    ContactPhone = 0;
                }
                else
                    ContactPhone = Int64.Parse(phoneString);
                if (String.IsNullOrEmpty(model.DopPhone))
                {
                    DopPhone = 0;
                }
                else
                {
                    phoneString = TextHelper.GetAsDigits(model.DopPhone.Replace("+7", ""));
                    if (phoneString.Length != 10)
                    {
                        modelErrors.Add("DopPhone", "Некорректное значение Дополнительного телефона!");
                        DopPhone = 0;
                    }
                    else
                        DopPhone = Int64.Parse(phoneString);
                }
                Email = model.Email;
                Comment = model.Comment;
                CreatedAt = model.CreatedAt;
                UpdatedAt = model.UpdatedAt;
                IsArchived = false;
                ArchivedDate = null;

                RegionName = String.Empty;
                DebtorTypeName = String.Empty;
                OriginalCreditorTypeName = String.Empty;
                DebtEssenceTypeName = String.Empty;
                CourtDecisionTypeName = String.Empty;
                DebtSellerTypeName = String.Empty;

                _lastError = String.Empty;

                return modelErrors.Count == 0 ? true : false;
            }
            catch(Exception ex)
            {
                modelErrors.Add("", "Ошибка во время проверки корректности введенных данных. Повторите попытку позже.");
                return false;
            }
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
    /// <summary>
    /// Тип продавца долга
    /// </summary>
    public enum EnumEditState
    {
        None = 0,
        Creating,
        Created,
        Edit,
        Editing,
        Deleted,
        Deleting,
        Activated,
        Disactivated,
        Confirmed,
        UnConfirmed,
        Error
    }
}