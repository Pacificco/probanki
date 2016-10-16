using Bankiru.Controllers;
using Bankiru.Models.Domain.Account;
using Bankiru.Models.Infrastructure;
using Bankiru.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View(new VM_UserLogin());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(VM_UserLogin model)
        {
            try
            {
                if (_connected)
                {                    
                    AccountManager _manager = new AccountManager();
                    VM_User user = _manager.Login(model.Username, model.Password);
                    if (user == null)
                    {
                        ViewBag.ErrorMessage = "Не верный логин или пароль!";
                        model.Password = "";
                        return View(model);
                    }
                    SessionPersister.Username = model.Username;
                    return RedirectToAction("Index", "Info");
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return RedirectToAction("Error", "Error", null);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return RedirectToAction("Error", "Error", null);
            }
        }

        public ActionResult Logout()
        {
            SessionPersister.Username = String.Empty;
            return RedirectToAction("Login");
        }
    }
}
