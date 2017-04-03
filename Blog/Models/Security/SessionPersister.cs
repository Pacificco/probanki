using Bankiru.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Security
{
    public static class SessionPersister
    {
        static string idSessionVar = "Id";
        static string emailSessionVar = "Email";
        static string nicSessionVar = "Nic";
        static string nameSessionVar = "Name";
        //static string usernameSessionVar = "Token";

        //public static VM_User CurrentUser { get; set; }

        public static int UserId
        {
            get
            {
                if (HttpContext.Current == null)
                    return 0;
                var sessionVar = HttpContext.Current.Session[idSessionVar];
                if (sessionVar != null)
                    return Convert.ToInt32(sessionVar);
                return 0;
            }
            set
            {
                HttpContext.Current.Session[idSessionVar] = value.ToString();
            }
        }
        public static string UserEmail
        {
            get
            {
                if (HttpContext.Current == null)
                    return String.Empty;
                var sessionVar = HttpContext.Current.Session[emailSessionVar];
                if (sessionVar != null)
                    return sessionVar as string;
                return null;
            }
            set
            {
                HttpContext.Current.Session[emailSessionVar] = value;
                //HttpContext.Current.Session.Timeout = 60;
            }
        }
        public static string UserNic
        {
            get
            {
                if (HttpContext.Current == null)
                    return String.Empty;
                var sessionVar = HttpContext.Current.Session[nicSessionVar];
                if (sessionVar != null)
                    return sessionVar as string;
                return null;
            }
            set
            {
                HttpContext.Current.Session[nicSessionVar] = value;
                //HttpContext.Current.Session.Timeout = 60;
            }
        }
        public static string UserName
        {
            get
            {
                if (HttpContext.Current == null)
                    return String.Empty;
                var sessionVar = HttpContext.Current.Session[nameSessionVar];
                if (sessionVar != null)
                    return sessionVar as string;
                return null;
            }
            set
            {
                HttpContext.Current.Session[nameSessionVar] = value;
                //HttpContext.Current.Session.Timeout = 60;
            }
        }

        public static void SetTimeout(int timeOut)
        {
            HttpContext.Current.Session.Timeout = timeOut;
        }
        public static void Clear()
        {
            HttpContext.Current.Session[nameSessionVar] = String.Empty;
            HttpContext.Current.Session[emailSessionVar] = String.Empty;
            HttpContext.Current.Session[nicSessionVar] = String.Empty;
            HttpContext.Current.Session[nameSessionVar] = String.Empty;
        }
    }
}