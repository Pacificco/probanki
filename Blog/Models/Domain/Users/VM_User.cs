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
        public int Id { get; set; }
        public string Name{ get; set; }
        public string Email{ get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password{ get; set; }
        public VM_UserSex Sex { get; set; }
        public string LastName{ get; set; }
        public string FatherName{ get; set; }
        public bool IsActive{ get; set; }
        public bool IsSubscribed{ get; set; }
        public string Nic{ get; set; }
        public string[] Rols { get; set; }
        public string Avatar { get; set; }
        public string Country { get; set; }
        public Guid? RegionId { get; set; }
        public string City { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastVisitDate { get; set; }
        public bool IsBan { get; set; }
        public DateTime? BanDate { get; set; }
        public string Comment { get; set; }
        public string Token { get; set; }

        private string _lastError;
        public string LfstError { get { return _lastError; } }

        public VM_User()
        {
            Id = -1;
            Rols = null;
            IsActive = false;            
            Nic = String.Empty;
            Name = String.Empty;
            LastName = String.Empty;
            FatherName = String.Empty;
            Sex = VM_UserSex.Undefined;
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
                    Sex = (VM_UserSex)(int)fValue;
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
}