using Bankiru.Models.Domain.Account;
using Bankiru.Models.Domain.Users;
using Bankiru.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    public class UsersController : BaseController
    {        
        [HttpGet]
        public ActionResult Index(int user_id)
        {
            if (!_isUserValid(user_id))
                return RedirectToAction("Index", "Home", null);

            try
            {                
                if (_connected)
                {
                    UserManager _manager = new UserManager();
                    VM_User model = _manager.GetUser(user_id);                    
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        return View("NotFound");
                    }
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
        [HttpGet]
        public ActionResult Edit(int user_id)
        {
            if (!_isUserValid(user_id))
                return RedirectToAction("Index", "Home", null);

            try
            {
                if (_connected)
                {
                    UserManager _manager = new UserManager();
                    VM_User model = _manager.GetUser(user_id);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        return View("NotFound");
                    }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VM_User model)
        {
            if (!_isUserValid(model.Id))
                return RedirectToAction("Index", "Home", null);

            try
            {
                if (_connected)
                {
                    AccountManager manager = new AccountManager();
                    if (manager.NicExists(model.Nic, model.Id))
                    {
                        ModelState.AddModelError("Nic", "Указанный Ник уже зарегистрирован в системе!");
                        return View(model);
                    }
                    if (manager.MailExists(model.Email, model.Id))
                    {
                        ModelState.AddModelError("Email", "Указанный Email уже зарегистрирован в системе!");
                        return View(model);
                    }

                    if (ModelState.IsValid)
                    {
                        UserManager _manager = new UserManager();
                        if (_manager.UpdateUserProfile(model))
                        {
                            return View("Index", model);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Во время сохранения данных произошла ошибка! Повторите попытку позже.");
                            log.Error(_manager.LastError);
                            return View(model);
                        }
                    }
                    else
                    {
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return RedirectToAction("Error", "Error", null);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                return RedirectToAction("Error", "Error", null);
            }
        }
        [HttpGet]
        public ActionResult Archive(int user_id)
        {
            try
            {
                if (!_isUserValid(user_id))
                    return RedirectToAction("Index", "Home", null);

                if (_connected)
                {
                    UserManager _manager = new UserManager();
                    VM_User model = _manager.GetUser(user_id);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        return View("NotFound");
                    }
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
        [HttpGet]
        public ActionResult Forecasts(int user_id)
        {
            if (!_isUserValid(user_id))
                return RedirectToAction("Index", "Home", null);

            try
            {
                if (_connected)
                {
                    UserManager _manager = new UserManager();
                    VM_User model = _manager.GetUser(user_id);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        return View("NotFound");
                    }
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

        [ChildActionOnly]
        //[OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public PartialViewResult _getModuleSideUserProfile()
        {
            try
            {
                if (_connected)
                {
                    UserManager manager = new UserManager();
                    VM_UserProfileInfo profile = null;
                    if(String.IsNullOrEmpty(SessionPersister.Username))
                    {
                        profile = manager.GetUserProfiletInfo(-1);
                    }
                    else
                    {
                        profile = manager.GetUserProfiletInfo(SessionPersister.CurrentUser.Id);
                    }                    
                    return PartialView("_moduleSideUserProfile", profile);
                }
                else
                {
                    log.Error("Ошибка во время отображения блока с профилем пользователя!\r\n" + _errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception ex)
            {
                log.Error("Ошибка во время отображения блока с профилем пользователя!\r\n" + ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60, VaryByParam = "cur_item")]
        public PartialViewResult _getModuleSideUserMenu(string cur_item = "none")
        {
            try
            {
                return PartialView("_modelSideUserMenu", cur_item);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }

        private bool _isUserValid(int user_id)
        {
            try
            {
                if (String.IsNullOrEmpty(Bankiru.Models.Security.SessionPersister.Username))
                    return false;
                if (Bankiru.Models.Security.SessionPersister.CurrentUser == null)
                    return false;
                if (Bankiru.Models.Security.SessionPersister.CurrentUser.Id != user_id)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
