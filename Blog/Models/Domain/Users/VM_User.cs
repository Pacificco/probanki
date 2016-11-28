using Bankiru.Models.DataBase;
using Bankiru.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Users
{
    public class VM_User
    {
        #region ПОЛЯ и СВОЙСТВА

        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Вы не указали имени")]
        [MaxLength(20, ErrorMessage = "Максимальная длина Имени не должна превышать 20 символов")]
        [MinLength(2, ErrorMessage = "Минимальная длина Имени не должна превышать 20 символов")]
        public string Name{ get; set; }

        [Display(Name = "Фамилия")]        
        [MaxLength(20, ErrorMessage = "Максимальная длина Фамилии не должна превышать 20 символов")]
        [MinLength(2, ErrorMessage = "Минимальная длина Фамилии не должна превышать 20 символов")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        [MaxLength(20, ErrorMessage = "Максимальная длина Отчества не должна превышать 20 символов")]
        [MinLength(2, ErrorMessage = "Минимальная длина Отчества не должна превышать 20 символов")]
        public string FatherName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Вы не указали Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный Email!")]
        public string Email{ get; set; }

        [Display(Name = "Email подтвержден")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Пароль")]
        public string Password{ get; set; }

        [Display(Name = "Пол")]
        [Range(1, 2, ErrorMessage = "Вы не указали Пол!")]
        public int Sex { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive{ get; set; }

        [Display(Name = "Подписан на рассылку")]
        public bool IsSubscribed{ get; set; }

        [Display(Name = "Ник")]
        [Required(ErrorMessage = "Вы не указали Ник")]
        [MaxLength(20, ErrorMessage = "Максимальная длина Ника не должна превышать 20 символов")]
        [MinLength(2, ErrorMessage = "Минимальная длина Ника не должна превышать 20 символов")]
        public string Nic{ get; set; }

        [Display(Name = "Роль")]
        [HiddenInput]
        public string[] Rols { get; set; }

        [Display(Name = "Аватарка")]
        public string Avatar { get; set; }

        [Display(Name = "Страна")]
        public string Country { get; set; }

        [Display(Name = "Регион")]
        public Guid? RegionId { get; set; }

        [Display(Name = "Город, населенный пункт")]
        public string City { get; set; }

        [Display(Name = "Дата регистрации")]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Дата последнего визита")]
        [DataType(DataType.Date)]
        public DateTime? LastVisitDate { get; set; }

        [Display(Name = "Забанин")]
        public bool IsBan { get; set; }

        [Display(Name = "Пользователь был забанин")]
        [DataType(DataType.Date)]
        public DateTime? BanDate { get; set; }

        [Display(Name = "Комментарий")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [MaxLength(20, ErrorMessage = "Максимальная длина Комментария не должна превышать 255 символов!")]
        public string Comment { get; set; }

        [HiddenInput]
        public string Token { get; set; }
                
        public VM_UserForecastInfo ForecastInfo { get; set; }

        public int ForecastCount { get; set; }
        public int WinCount { get; set; }

        private string _lastError;
        public string LastError { get { return _lastError; } }
        #endregion

        public VM_User()
        {
            ForecastInfo = new VM_UserForecastInfo();
            Clear();
        }
        public void Clear()
        {
            Id = -1;
            Rols = null;
            IsActive = false;
            Nic = String.Empty;
            Name = String.Empty;
            LastName = String.Empty;
            FatherName = String.Empty;
            Sex = 0; // VM_UserSex.Undefined;
            Email = String.Empty;
            EmailConfirmed = false;
            IsSubscribed = false;
            Avatar = String.Empty;
            Country = String.Empty;
            RegionId = Guid.Empty;
            City = String.Empty;
            RegistrationDate = DateTime.Now;
            LastVisitDate = null;
            IsBan = false;
            BanDate = null;
            Comment = String.Empty;
            Token = "";

            ForecastInfo.Clear();

            ForecastCount = 0;
            WinCount = 0;

            _lastError = "";
        }

        #region РАБОТА С БАЗОЙ ДАННЫХ
        public bool UpdateEmailConfirmed()
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UserEmailConfirmedEdit.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UserEmailConfirmedEdit.Params.Id, Id);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UserEmailConfirmedEdit.Params.Confirmed, EmailConfirmed);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {
                    
                    if(command.ExecuteNonQuery() > 0)                    
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время обновления поля EmailConfirmed! " + ex.ToString();                    
                    return false;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
        }
        public bool UpdateToken(string token)
        {
            SqlCommand command = new SqlCommand(DbStruct.PROCEDURES.UserTokenEdit.Name, GlobalParams.GetConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UserTokenEdit.Params.Id, Id);
            command.Parameters.AddWithValue(DbStruct.PROCEDURES.UserTokenEdit.Params.Token, token);
            command.CommandTimeout = 15;
            lock (GlobalParams._DBAccessLock)
            {
                try
                {

                    if (command.ExecuteNonQuery() > 0)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    _lastError = "Ошибка во время обновления поля Token! " + ex.ToString();
                    return false;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                }
            }
        }        
        #endregion

        public bool SetFieldValue(string fName, object fValue)
        {
            switch (fName)
            {
                case "Id":
                    Id = (int)fValue;
                    break;
                case "Name":
                    Name = (string)fValue;
                    break;
                case "Email":
                    Email = (string)fValue;
                    break;
                case "Password":
                    Password = "";
                    break;
                case "Sex":
                    Sex = (int)fValue;
                    break;
                case "LastName":
                    LastName = (string)fValue;
                    break;
                case "FatherName":
                    FatherName = (string)fValue;
                    break;
                case "IsActive":
                    IsActive = (bool)fValue;
                    break;
                case "IsSubscribed":
                    IsSubscribed = (bool)fValue;
                    break;
                case "Nic":
                    Nic = (string)fValue;
                    break;
                case "Rols":
                    Rols = fValue == DBNull.Value ? new string[] { "guest" } : fValue.ToString().Split(new char[] { ',' });
                    break;
                case "EmailConfirmed":
                    EmailConfirmed = (bool)fValue;
                    break;
                case "Avatar":
                    Avatar = fValue == DBNull.Value ? "" : (string)fValue;
                    break;
                case "Country":
                    Country = fValue == DBNull.Value ? "" : (string)fValue;
                    break;
                case "RegionId":
                    RegionId = fValue == DBNull.Value ? Guid.Empty : (Guid)fValue;
                    break;
                case "City":
                    City = fValue == DBNull.Value ? "" : (string)fValue;
                    break;
                case "RegistrationDate":
                    RegistrationDate = (DateTime)fValue;
                    break;
                case "LastVisitDate":
                    LastVisitDate = fValue == DBNull.Value ? null : (DateTime?)fValue;
                    break;
                case "IsBan":
                    IsBan = (bool)fValue;
                    break;
                case "BanDate":
                    BanDate = fValue == DBNull.Value ? null : (DateTime?)fValue;
                    break;
                case "Comment":
                    Comment = (string)fValue;
                    break;
                case "Token":
                    Token = (string)fValue;
                    break;
                default:
                    
                    return false;                    
            }
            return true;
        }
        public string GetFIO()
        {
            string result = "";
            if (!String.IsNullOrEmpty(LastName))
                result += LastName + " ";
            if (!String.IsNullOrEmpty(Name))
                result += Name + " ";
            if (!String.IsNullOrEmpty(FatherName))
                result += FatherName + " ";
            return result.Trim();
        }
    }

    public static class VM_UserGroup
    {
        public static string Admin = "admin";        
        public static string SEO = "seo";
        public static string MMS = "mms";
        public static string ContentManager = "content_manager";
        public static string ClubMember = "club_member";
        public static string Guest = "guest";
    }
    public enum VM_UserSex
    {
        Undefined = 0,
        Male,
        Female
    }
    public enum VM_ForecastTariff
    {
        Undefined = 0,
        Platinum = 1,
        Gold = 2,
        Silver = 3,
        Bronze = 4
    }
}