using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Users
{
    public class VM_UserItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public VM_UserSex Sex { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public bool IsActive { get; set; }
        public bool IsSubscribed { get; set; }
        public string Nic { get; set; }
        public string[] Rols { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsBan { get; set; }
        public string Comment { get; set; }

        private string _lastError;
        public string LfstError { get { return _lastError; } }

        public VM_UserItem()
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
            RegistrationDate = DateTime.Now;
            IsBan = false;
            Comment = String.Empty;

            _lastError = "";
        }

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
                case "RegistrationDate":
                    RegistrationDate = (DateTime)fValue;
                    break;                
                case "IsBan":
                    IsBan = (bool)fValue;
                    break;                
                case "Comment":
                    Comment = (string)fValue;
                    break;                
                default:

                    return false;
            }
            return true;
        }
        
        public string GetFIO()
        {
            string fio = "";
            if (!String.IsNullOrEmpty(LastName))
                fio += LastName + " ";
            if (!String.IsNullOrEmpty(Name))
                fio += Name + " ";
            if (!String.IsNullOrEmpty(FatherName))
                fio += FatherName + " ";
            return fio.Trim();
        }
        public string GetRols()
        {
            if (Rols == null)
                return "";
            if (Rols.Length == 0)
                return "";
            else
            {
                string str = "";
                foreach (string r in Rols)
                    str += ", " + r;
                return str.Substring(2);
            }
        }
    }
}