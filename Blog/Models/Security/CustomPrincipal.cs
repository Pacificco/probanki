using Bankiru.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Bankiru.Models.Security
{
    public class CustomPrincipal : IPrincipal
    {
        private VM_User Account;

        public CustomPrincipal(VM_User account)
        {
            this.Account = account;
            this.Identity = new GenericIdentity(account.Email);
        }

        public IIdentity Identity { get; set; }

        public bool IsInRole(string role)
        {
            var roles = role.Split(new char[] { ',' });
            return roles.Any(r => this.Account.Rols.Contains(r));
        }
    }
}