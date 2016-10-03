using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bankiru.Models.Security
{
    public static class SessionPersister
    {
        static string usernameSessionVar = "Email";
        public static string Username
        {
            get
            {
                if (HttpContext.Current == null)
                    return String.Empty;
                var sessionVar = HttpContext.Current.Session[usernameSessionVar];
                if (sessionVar != null)
                    return sessionVar as string;
                return null;
            }
            set
            {
                HttpContext.Current.Session[usernameSessionVar] = value;
                HttpContext.Current.Session.Timeout = 60;
            }
        }
    }
}