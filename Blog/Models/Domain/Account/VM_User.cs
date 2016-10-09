using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Models.Domain.Account
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
        }
    }

    public enum VM_UserSex
    {
        Undefined = 0,
        Male,
        Female
    }
}