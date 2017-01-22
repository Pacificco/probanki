using Bankiru.Models.Domain.Account;
using Bankiru.Models.Domain.Users;
using Bankiru.Models.OutApi;
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
        #region РЕГИСТРАЦИЯ
        [HttpGet]
        public ActionResult Register()
        {
            return View(new VM_UserRegistration());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(VM_UserRegistration model)
        {
            try
            {
                if (_connected)
                {
                    log.Info(String.Format("[try register] nic={0}, name={1}, email={2}, sex={3}, subscribed={4}",
                        model.NicName,
                        model.Name,
                        model.Email,
                        model.Sex.ToString(),
                        model.Subscribed ? "Да" : "Нет"
                        ));

                    if (ModelState.IsValid)
                    {
                        if (Session["captcha_code"] != null)
                        {
                            if (!model.CaptchaCode.ToLower().Equals(Session["captcha_code"].ToString().ToLower()))
                            {
                                ModelState.AddModelError("CaptchaCode", "Код с картинки указан не верно!");
                                log.Warn("[incorrect register] Код с картинки указан не верно!");
                                return View(model);
                            }
                        }

                        AccountManager manager = new AccountManager();
                        if (manager.NicExists(model.NicName))
                        {
                            ModelState.AddModelError("NicName", "Указанный Никнейм уже зарегистрирован в системе!");
                            model.Password = "";
                            model.PasswordConfirmed = "";
                            log.Warn("[incorrect register] Указанный Никнейм уже зарегистрирован в системе!");
                            return View(model);
                        }
                        if (manager.MailExists(model.Email))
                        {
                            ModelState.AddModelError("Email", "Указанный Email уже зарегистрирован в системе!");
                            model.Password = "";
                            model.PasswordConfirmed = "";
                            log.Warn("[incorrect register] Указанный Email уже зарегистрирован в системе!");
                            return View(model);
                        }
                        string token = Guid.NewGuid().ToString().ToLower().Replace("-","");
                        int id = manager.RegisterNewUser(model, VM_UserGroup.ClubMember, token);
                        if (id == -1)
                        {
                            ModelState.AddModelError("", "Ошибка во время регистрации! Повторите попытку через несколько минут.");
                            model.Password = "";
                            model.PasswordConfirmed = "";
                            log.Info("[error register] Ошибка во время регистрации!\r\n" + manager.LastError);
                            return View(model);
                        }

                        try
                        {
                            EmailModel emailModel = new EmailModel();
                            emailModel.From = "no-reply@probanki.net";
                            emailModel.Subject = "";                            
                            emailModel.To = model.Email.Trim();

                            VM_UserEmailConfirmed emailConfirmed = new VM_UserEmailConfirmed();
                            emailConfirmed.UserId = id;
                            emailConfirmed.UserMail = model.Email;
                            emailConfirmed.UserName = model.Name;
                            emailConfirmed.Token = token;

                            var mailController = new EmailController();
                            var email = mailController.SendEmailRegister(emailModel, emailConfirmed);
                            email.Deliver();
                        }
                        catch (Exception ex)
                        {
                            log.Error("[error register] Ошибка во время отправки Email!\r\n" + ex.ToString());
                            ModelState.AddModelError("", "Ошибка регистрации!");
                            return View(model);
                        }
                        log.Info("[success register] Успешно!");
                        return View("EmailSent");
                    }
                    else
                    {
                        log.Warn("[incorrect register] Данные с формы не прошли валидацию!");
                        return View(model);
                    }
                }
                else
                {
                    log.Error("[error register] " + _errMassage);
                    ModelState.AddModelError("", "Ошибка регистрации!");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                log.Error("[error register] " + ex.ToString());
                ModelState.AddModelError("", "Ошибка регистрации!");
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult EmailConfirmed(int user_id, string token)
        {
            try
            {
                if (_connected)
                {
                    AccountManager manager = new AccountManager();
                    VM_User user = manager.FindUser(user_id);
                    
                    if (user == null)
                        return View("NotFound");

                    if(!user.Token.Equals(token))
                        return View("NotFound");

                    if(user.EmailConfirmed)
                        return View("EmailAlreadyConfirmed");

                    user.EmailConfirmed = true;
                    if(!user.UpdateEmailConfirmed())
                        return View("Error");

                    return View("EmailSuccsessConfirmed");
                }
                else
                {
                    log.Error(_errMassage);
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                log.Error("Ошибка во время подтверждения Email! " + ex.ToString());
                return View("NotFound");
            }
        }
        #endregion

        #region АВТОРИЗАЦИЯ
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
        public ActionResult LoginAjax(VM_UserLogin model)
        {
            try
            {
                if (_connected || !Request.IsAjaxRequest())
                {
                    if (ModelState.IsValid)
                    {
                        AccountManager _manager = new AccountManager();
                        VM_User user = _manager.Login(model.Username, model.Password);
                        if (user == null)
                        {
                            ModelState.AddModelError("", "Не верный логин или пароль!");
                            model.Password = "";
                            return PartialView("_moduleFormLogin", model);
                        }

                        SessionPersister.Username = model.Username;
                        SessionPersister.SetTimeout(86400);

                        //return PartialView("_moduleWellcomeBlock", user);
                        model.AuthSuccessMes = "Вы успешно авторизовались!";
                        return PartialView("_moduleFormLogin", model);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ошибка авторизации!");
                        return PartialView("_moduleFormLogin", model);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    ModelState.AddModelError("", "Ошибка авторизации!");
                    return PartialView("_moduleFormLogin", model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "Ошибка авторизации!");
                return PartialView("_moduleFormLogin", model);
            }
        }
        [HttpGet]        
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public PartialViewResult LoginAjax()
        {
            try
            {
                if (_connected || !Request.IsAjaxRequest())
                {
                    return PartialView("_moduleFormLogin", new VM_UserLogin());
                }
                else
                {
                    log.Error("Ошибка авторизации! Нет доступа к базе данных или запрос не является Ajax-запросом.");
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [HttpGet]
        public ActionResult Logout()
        {
            SessionPersister.Username = String.Empty;
            SessionPersister.CurrentUser = null;
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region ВОССТАНОВЛЕНИЕ ПАРОЛЯ
        [HttpGet]
        public ActionResult PasswordRecover()
        {
            return View(new VM_UserPasswordRecover());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordRecover(VM_UserPasswordRecover model)
        {
            try
            {
                if (_connected)
                {
                    log.Info("[try password_recover] " + model.Email);
                    if (ModelState.IsValid)
                    {
                        if (Session["captcha_code"] != null)
                        {
                            if (!model.CaptchaCode.ToLower().Equals(Session["captcha_code"].ToString().ToLower()))
                            {
                                ModelState.AddModelError("CaptchaCode", "Код с картинки указан не верно!");
                                log.Warn("[incorrect password_recover] Код с картинки указан не верно!");
                                return View(model);
                            }
                        }

                        AccountManager manager = new AccountManager();
                        VM_User user = manager.FindActiveUser(model.Email.Trim());
                        if (user == null)
                        {
                            ModelState.AddModelError("Email", "Указанный Вами Email не зарегистрирован в системе!");
                            log.Warn("[incorrect password_recover] Указанный Вами Email не зарегистрирован в системе!");
                            return View(model);
                        }
                            
                        string token = Guid.NewGuid().ToString().ToLower().Replace("-","");
                        if (!user.UpdateToken(token))
                        {
                            ModelState.AddModelError("", "Ошибка во время восстановления пароля!");
                            log.Error("[error password_recover] Ошибка!\r\n" + user.LastError);
                            return View(model);
                        }

                        try
                        {
                            EmailModel emailModel = new EmailModel();
                            emailModel.From = "no-reply@probanki.net";
                            emailModel.Subject = "";
                            emailModel.To = "pacificco@mail.ru";
                            //emailModel.To = model.Email.Trim();

                            VM_UserEmailConfirmed emailConfirmed = new VM_UserEmailConfirmed();
                            emailConfirmed.UserId = user.Id;
                            emailConfirmed.UserMail = model.Email;
                            emailConfirmed.UserName = user.Name;
                            emailConfirmed.Token = token;

                            var mailController = new EmailController();
                            var email = mailController.SendEmailPasswordRecover(emailModel, emailConfirmed);
                            email.Deliver();
                        }
                        catch (Exception ex)
                        {
                            log.Error("[error password_recover] Ошибка во время отправки Email!]\r\n" + ex.ToString());
                            ModelState.AddModelError("", "Ошибка во время восстановления пароля!");
                            return View(model);
                        }

                        log.Info("[success password_recover] Успешно!");
                        return View("PasswordRecoverEmailSent");
                    }
                    else
                    {
                        log.Warn("[incorrect password_recover] Данные с формы не прошли валидацию!");
                        return View(model);
                    }
                }
                else
                {
                    log.Error("[error password_recover] " + _errMassage);
                    ModelState.AddModelError("", "Ошибка во время восстановления пароля!");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                log.Error("[error password_recover] " + ex.ToString());
                ModelState.AddModelError("", "Ошибка во время восстановления пароля!");
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult ChangePassword(int user_id, string token)
        {
            try
            {
                if (_connected)
                {
                    AccountManager manager = new AccountManager();
                    VM_User user = manager.FindUser(user_id);

                    if (user == null)
                        return View("NotFound");

                    if (!user.Token.Equals(token))
                        return View("NotFound");

                    return View(new VM_UserChangePassword() { Id = user_id, Token = token });
                }
                else
                {
                    log.Error(_errMassage);
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                log.Error("Ошибка во время подтверждения Email! " + ex.ToString());
                return View("NotFound");
            }            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(VM_UserChangePassword model)
        {
            try
            {
                if (_connected)
                {
                    log.Info("[try change_password]");
                    if (ModelState.IsValid)
                    {
                        if (Session["captcha_code"] != null)
                        {
                            if (!model.CaptchaCode.ToLower().Equals(Session["captcha_code"].ToString().ToLower()))
                            {
                                log.Warn("[incorrect change_password] Код с картинки указан не верно!");
                                ModelState.AddModelError("CaptchaCode", "Код с картинки указан не верно!");
                                return View(model);
                            }
                        }

                        AccountManager manager = new AccountManager();
                        VM_User user = manager.FindActiveUser(model.Id);
                        if (user == null)
                        {
                            ModelState.AddModelError("", "Ошибка во время изменения пароля!");
                            log.Error("[error change_password] Пользователь не определен!\r\n" + manager.LastError);
                            return View(model);
                        }

                        if (!user.Token.Equals(model.Token))
                        {
                            ModelState.AddModelError("", "Ключ безопасности нарушен!");
                            log.Warn("[incorrect change_password] Ключ безопасности нарушен!");
                            return View(model);
                        }

                        if (!manager.ChangeUserPassword(user.Id, model.Password))
                        {
                            ModelState.AddModelError("", "Ошибка во время изменения пароля!");
                            log.Error("[error change_password] Ошибка во время изменения пароля!\r\n" + manager.LastError);
                            return View(model);
                        }

                        SessionPersister.Username = String.Empty;
                        SessionPersister.CurrentUser = null;

                        log.Info("[success change_password] Успешно!");
                        return View("ChangePasswordSuccess");
                    }
                    else
                    {
                        log.Warn("[incorrect change_password] Данные с формы не прошли валидацию!");
                        return View(model);
                    }
                }
                else
                {
                    log.Error("[error change_password] Ошибка!\r\n" + _errMassage);
                    ModelState.AddModelError("", "Ошибка во время изменения пароля!");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                log.Error("[error change_password] Ошибка!\r\n" + ex.ToString());
                ModelState.AddModelError("", "Ошибка во время изменения пароля!");
                return View(model);
            }
        }
        #endregion
    }
}
