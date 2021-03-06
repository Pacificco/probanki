﻿using Bankiru.Models.Domain.Account;
using Bankiru.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bankiru.Models.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            if (String.IsNullOrEmpty(SessionPersister.UserEmail))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Account", action = "Login" })
                    );
            }
            else
            {
                AccountManager _manager = new AccountManager();
                VM_User user = _manager.FindUser(SessionPersister.UserEmail);
                
                if (user == null)
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Account", action = "Login" })
                    );

                CustomPrincipal mp = new CustomPrincipal(user);
                if (!mp.IsInRole(Roles))
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Account", action = "Login" })
                    );
            }
        }
    }
}