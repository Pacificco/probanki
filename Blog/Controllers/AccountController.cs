using Bankiru.Models.Domain.Account;
using Bankiru.Models.Security;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{    
    public class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View(new VM_Login());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(VM_Login model)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        AccountManager _manager = new AccountManager();
                        VM_User user = _manager.Login(model.Username, model.Password);
                        if (user == null)
                        {
                            ModelState.AddModelError("", "Не верный логин или пароль!");
                            model.Password = "";
                            return View(model);
                        }

                        SessionPersister.Username = model.Username;
                        SessionPersister.SetTimeout(86400);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return View(model);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    ModelState.AddModelError("", "Ошибка авторизации!");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Ошибка авторизации!");
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public ActionResult LoginAjax(VM_Login model)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid && Request.IsAjaxRequest())
                    {
                        AccountManager _manager = new AccountManager();
                        VM_User user = _manager.Login(model.Username, model.Password);
                        if (user == null)
                        {
                            ModelState.AddModelError("", "Не верный логин или пароль!");
                            model.Password = "";
                            return View(model);
                        }

                        SessionPersister.Username = model.Username;
                        SessionPersister.CurrentUser = user;
                        SessionPersister.SetTimeout(86400);

                        return PartialView("_moduleWellcomeBlock", user);
                    }
                    else
                    {
                        return View(model);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    ModelState.AddModelError("", "Ошибка авторизации!");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Ошибка авторизации!");
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult Logout()
        {
            SessionPersister.Username = String.Empty;
            SessionPersister.CurrentUser = null;
            return RedirectToAction("Index", "Home");
        }        
    }
}
