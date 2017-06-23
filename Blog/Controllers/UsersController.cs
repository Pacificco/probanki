using Bankiru.Models.Domain;
using Bankiru.Models.Domain.Account;
using Bankiru.Models.Domain.Club;
using Bankiru.Models.Domain.Users;
using Bankiru.Models.Helpers;
using Bankiru.Models.OutApi;
using Bankiru.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    [CustomAuthorize(Roles = "admin, club_member")]
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
        public ActionResult Archive(int user_id, string subject = "all")
        {
            try
            {
                if (!_isUserValid(user_id))
                    return RedirectToAction("Index", "Home", null);

                if (_connected)
                {
                    UserManager _manager = new UserManager();
                    List<VM_ForecastUser> model = _manager.GetUserForecasts(user_id, subject);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        //return View("NotFound");
                        return View("Archive", null);
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
        public ActionResult Club(int user_id)
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
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public ActionResult AddBalanceAjax(VM_UserAddBalance model)
        {
            try
            {
                if (_connected)
                {
                    if (Request.IsAjaxRequest())
                    {
                        if (ModelState.IsValid)
                        {
                            if (model.Comment == null)
                                model.Comment = "";

                            UserManager manager = new UserManager();
                            model.Sum = UserTariffHelper.CalcPaymentSum((EnumForecastTariff)model.TariffId, (EnumClubTariffPeriod)model.PeriodId);
                            if (!manager.AddBalance(model))
                            {
                                ModelState.AddModelError("", "Ошибка во время выполнения запроса!");
                                return PartialView("_userAddBalanceBlock", model);
                            }

                            //Отправка письма с реквизитами
                            try
                            {
                                log.Info("[success email payment] Пытаемся отправить письмо с реквизитами!");

                                VM_User user = manager.GetUser(model.UserId, false);

                                EmailModel emailModel = new EmailModel();
                                emailModel.From = "no-reply@probanki.net";
                                emailModel.Subject = "Подписка на ProBanki.net";
                                emailModel.To = user.Email;

                                VM_TariffPaymentEmail emailData = new VM_TariffPaymentEmail();
                                emailData.Tariff = UserTariffHelper.GetTariffName((EnumForecastTariff)model.TariffId);
                                emailData.Period = UserTariffHelper.GetTariffPeriodName((EnumClubTariffPeriod)model.PeriodId);
                                emailData.UserName = user.Name;
                                emailData.Sum = model.Sum;

                                var mailController = new EmailController();
                                var email = mailController.SendEmailTariffPayment(emailModel, emailData);
                                email.Deliver();

                                log.Info("[success email payment] Успешно!");

                                manager.PaymentEmailSend(model.UserId);
                            }
                            catch (Exception ex)
                            {
                                log.Error("[error email payment] Ошибка во время отправки Email!\r\n" + ex.ToString());
                                ModelState.AddModelError("", "Ошибка во время отправки письма на электронную почту!");
                                return PartialView("_userAddBalanceBlock", model);
                            }

                            return PartialView("_userAddBalanceBlock", new VM_UserAddBalance() { SuccessMessage = "Успешно!" });
                        }
                        else
                        {
                            return PartialView("_userAddBalanceBlock", model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ошибка запроса к серверу!");
                        return PartialView("_userAddBalanceBlock", model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка подключения к серверу!");
                    return PartialView("_userAddBalanceBlock", model);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Ошибка в методе AddBalanceAjax!\n", ex.ToString()));
                ModelState.AddModelError("", "Ошибка запроса к серверу!");
                return PartialView("_userAddBalanceBlock", model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 36, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public ActionResult CreateUserTariffRequest(VM_UserAddBalance model)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        if (model.Comment == null)
                            model.Comment = "";

                        UserManager manager = new UserManager();
                        model.Sum = UserTariffHelper.CalcPaymentSum((EnumForecastTariff)model.TariffId, (EnumClubTariffPeriod)model.PeriodId);
                        if (!manager.CreateUserTariffRequest(model))
                        {
                            ModelState.AddModelError("", "Ошибка во время выполнения запроса!");
                            return PartialView("_userAddBalanceBlock", model);
                        }
                                                
                        return PartialView("_userAddBalanceBlock", new VM_UserAddBalance() { SuccessMessage = "Успешно!" });
                    }
                    else
                    {
                        return PartialView("_userAddBalanceBlock", model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка подключения к серверу!");
                    return PartialView("_userAddBalanceBlock", model);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Ошибка в методе AddBalanceAjax!\n", ex.ToString()));
                ModelState.AddModelError("", "Ошибка запроса к серверу!");
                return PartialView("_userAddBalanceBlock", model);
            }
        }

        #region ДОЧЕРНИЕ МЕТОДЫ
        [ChildActionOnly]
        public PartialViewResult _getModuleSideUserProfile()
        {
            try
            {
                if (_connected)
                {
                    UserManager manager = new UserManager();
                    VM_UserProfileInfo profile = null;
                    if(String.IsNullOrEmpty(SessionPersister.UserEmail))
                    {
                        profile = manager.GetUserProfiletInfo(-1);
                    }
                    else
                    {
                        profile = manager.GetUserProfiletInfo(SessionPersister.UserId);
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
        [ChildActionOnly]
        public PartialViewResult _getUserForecastInfoBlock(int user_id)
        {
            try
            {
                if (_connected)
                {
                    UserManager manager = new UserManager();
                    VM_UserProfileInfo info = manager.GetUserProfiletInfo(user_id);
                    if (info == null)
                    {
                        log.Error("Ошибка во время отображения блока с информацией о прогнозах пользователя!\r\n" + manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                    return PartialView("_userForecastInfoBlock", info);
                }
                else
                {
                    log.Error("Ошибка во время отображения блока с информацией о прогнозах пользователя!\r\n" + _errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception ex)
            {
                log.Error("Ошибка во время отображения блока с информацией о прогнозах пользователя!\r\n" + ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [AllowAnonymous]
        public PartialViewResult _getUserAddBalanceBlock(int user_id)
        {
            try
            {
                if (_connected)
                {
                    UserManager manager = new UserManager();
                    VM_UserAddBalance balance = new VM_UserAddBalance();
                    balance.UserId = user_id;
                    return PartialView("_userAddBalanceBlock", balance);
                }
                else
                {
                    log.Error("Ошибка во время отображения блока для пополнения баланса пользователя!\r\n" + _errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception ex)
            {
                log.Error("Ошибка во время отображения блока для пополнения баланса пользователя!\r\n" + ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public PartialViewResult _getUserBalanceHistory(int user_id)
        {
            try
            {
                if (_connected)
                {
                    UserManager manager = new UserManager();
                    List<VM_UserBalanceHistoryItem> history = manager.GetUserBalanceHistory(user_id);
                    if (history == null)
                    {
                        log.Error("Ошибка во время отображения истории баланса пользователя!\r\n" + manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                    return PartialView("_userBalanceHistoryBlock", history);
                }
                else
                {
                    log.Error("Ошибка во время отображения истории баланса пользователя!\r\n" + _errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception ex)
            {
                log.Error("Ошибка во время отображения истории баланса пользователя!\r\n" + ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public PartialViewResult _getUserTariffRequestHistory(int user_id)
        {
            try
            {
                if (_connected)
                {
                    UserManager manager = new UserManager();
                    List<VM_UserTariffRequest> history = manager.GetUserTariffRequests(user_id, null);
                    if (history == null)
                    {
                        log.Error("Ошибка во время отображения истории подписок пользователя!\r\n" + manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                    return PartialView("_userTariffRequestsHistoryBlock", history);
                }
                else
                {
                    log.Error("Ошибка во время отображения истории подписок пользователя!\r\n" + _errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception ex)
            {
                log.Error("Ошибка во время отображения истории подписок пользователя!\r\n" + ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        #endregion

        [AllowAnonymous]
        private bool _isUserValid(int user_id)
        {
            try
            {
                if (String.IsNullOrEmpty(Bankiru.Models.Security.SessionPersister.UserEmail))
                    return false;
                //if (Bankiru.Models.Security.SessionPersister.CurrentUser == null)
                //    return false;
                if (Bankiru.Models.Security.SessionPersister.UserId != user_id)
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
