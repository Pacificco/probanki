using Bankiru.Models.Domain;
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
    public class ForecastController : BaseController
    {
        [HttpGet]
        public ActionResult List()
        {
            try
            {
                if (_connected)
                {
                    ForecastManager manager = new ForecastManager();
                    List<VM_Forecast> model = manager.GetCurrentForecasts(true);                    
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        log.Error("[Forecasts] Не удалось загрузить текущие прогнозы!\n" + manager.LastError);
                        return View("Error");
                    }
                }
                else
                {                    
                    log.Error("[Forecasts] Не удалось загрузить текущие прогнозы!\n" + _errMassage);
                    return View("Error");
                }
            }
            catch (Exception e)
            {
                log.Error("[Forecasts] Не удалось загрузить текущие прогнозы!\n" + e.ToString());
                return View("Error");
            }
        }
        [HttpGet]        
        public ActionResult Forecast(string subject_id, int id)
        {
            try
            {
                if (_connected)
                {
                    ForecastManager manager = new ForecastManager();
                    VM_Forecast model = manager.GetForecast(id);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        log.Error("[Forecasts] Не удалось загрузить прогноз!\n" + manager.LastError);
                        return View("Error");
                    }
                }
                else
                {
                    log.Error("[Forecasts] Не удалось загрузить прогноз!\n" + _errMassage);
                    return View("Error");
                }
            }
            catch (Exception e)
            {
                log.Error("[Forecasts] Не удалось загрузить прогноз!\n" + e.ToString());
                return View("Error");
            }
        }
        [HttpGet]
        public ActionResult Archive(string subject = "all")
        {
            try
            {                
                if (_connected)
                {
                    UserManager _manager = new UserManager();
                    List<VM_ForecastUser> model = _manager.GetUserForecasts(-1, subject);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
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
        [OutputCache(Duration = 60)]
        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }
        [HttpGet]
        [OutputCache(Duration = 60)]
        [AllowAnonymous]
        public ActionResult Rules()
        {
            return View();
        }
        [HttpGet]
        [OutputCache(Duration = 60)]        
        public ActionResult Users()
        {
            return View();
        }

        #region AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUserToForecast(VM_AddUserToForecast model)
        {
            try
            {
                if (_connected)
                {
                    if (Request.IsAjaxRequest())
                    {
                        if (ModelState.IsValid)
                        {
                            ForecastManager manager = new ForecastManager();

                            if (String.IsNullOrEmpty(model.Value))
                            {
                                model.SuccessMessage = String.Empty;
                                ModelState.AddModelError("", "Вы не указали значение прогноза!");
                                return PartialView("_moduleAddUserToForecast", model);
                            }
                            
                            bool success = false;
                            double value = TextHelper.DoubleParse(model.Value, out success);
                            if (!success)
                            {
                                model.SuccessMessage = String.Empty;
                                ModelState.AddModelError("", "Вы задали некорректное значение прогноза!");
                                return PartialView("_moduleAddUserToForecast", model);
                            }

                            EnumDisableAddUserToForecast disableState = EnumDisableAddUserToForecast.Undefined;
                            if (!manager.AddUserToForecast(model.ForecastId, model.UserId, value, out disableState))
                            {
                                model.SuccessMessage = String.Empty;
                                switch (disableState)
                                { 
                                    case EnumDisableAddUserToForecast.ForecastClosed:
                                        ModelState.AddModelError("", "В этом прогнозе нельзя принять участие, так как он уже закрыт.");
                                        break;
                                    case EnumDisableAddUserToForecast.ForecastFrozen:
                                        ModelState.AddModelError("", "К сожалению, Вы уже не можете принять участие в этом прогнозе. До оглашения результатов осталось менее двух дней.");
                                        break;
                                    case EnumDisableAddUserToForecast.AlreadyExists:
                                        ModelState.AddModelError("", "Вы уже приняли участие в этом прогнозе.");
                                        break;
                                    case EnumDisableAddUserToForecast.NonAuthorization:
                                        ModelState.AddModelError("", "Чтобы принять участие в прогнозе, Вам необходимо Авторизоваться или Зарегистрироваться.");
                                        break;
                                    case EnumDisableAddUserToForecast.TariffOut:
                                        ModelState.AddModelError("", "В этом прогнозе Вы не можете принять участие. Согласно Вашему тарифу на этот месяц вы исчерпали лимит прогнозов.");
                                        break;
                                    case EnumDisableAddUserToForecast.EmptyBalance:
                                        ModelState.AddModelError("", "Чтобы принять участие в прогнозе, Вам необходимо продлить подписку на участие в клубе.");
                                        break;
                                    case EnumDisableAddUserToForecast.InternalError:
                                        ModelState.AddModelError("", "Ошибка во время выполнения запроса!");
                                        break;
                                    default:
                                        ModelState.AddModelError("", "Ошибка во время выполнения запроса!");
                                        break;
                                }                                
                                return PartialView("_moduleAddUserToForecast", model);
                            }

                            //Отправка письма с реквизитами
                            //try
                            //{
                            //    log.Info("[success email payment] Пытаемся отправить письмо с реквизитами!");

                            //    VM_User user = manager.GetUser(model.UserId, false);

                            //    EmailModel emailModel = new EmailModel();
                            //    emailModel.From = "no-reply@probanki.net";
                            //    emailModel.Subject = "Подписка на ProBanki.net";
                            //    emailModel.To = user.Email;

                            //    VM_TariffPaymentEmail emailData = new VM_TariffPaymentEmail();
                            //    emailData.Tariff = UserTariffHelper.GetTariffName((EnumForecastTariff)model.TariffId);
                            //    emailData.Period = UserTariffHelper.GetTariffPeriodName((EnumClubTariffPeriod)model.PeriodId);
                            //    emailData.UserName = user.Name;
                            //    emailData.Sum = model.Sum;

                            //    var mailController = new EmailController();
                            //    var email = mailController.SendEmailTariffPayment(emailModel, emailData);
                            //    email.Deliver();

                            //    log.Info("[success email payment] Успешно!");

                            //    manager.PaymentEmailSend(model.UserId);
                            //}
                            //catch (Exception ex)
                            //{
                            //    log.Error("[error email payment] Ошибка во время отправки Email!\r\n" + ex.ToString());
                            //    ModelState.AddModelError("", "Ошибка во время отправки письма на электронную почту!");
                            //    return PartialView("_userAddBalanceBlock", model);
                            //}

                            return PartialView("_moduleAddUserToForecast", new VM_AddUserToForecast() { SuccessMessage = "Успешно!" });
                        }
                        else
                        {
                            return PartialView("_moduleAddUserToForecast", model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ошибка запроса к серверу!");
                        return PartialView("_moduleAddUserToForecast", model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка подключения к серверу!");
                    return PartialView("_moduleAddUserToForecast", model);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Ошибка в методе AddBalanceAjax!\n", ex.ToString()));
                ModelState.AddModelError("", "Ошибка запроса к серверу!");
                return PartialView("_moduleAddUserToForecast", model);
            }
        }        
        #endregion

        #region ДОЧЕРНИЕ МЕТОДЫ
        [ChildActionOnly]
        [OutputCache(Duration = 60, VaryByParam = "cur_item")]
        public ActionResult _getModuleClubMenu(string cur_item = "none")
        {
            try
            {
                return PartialView("_moduleClubMenu", cur_item);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());                
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60)]
        public ActionResult _getModuleClubMembers()
        {
            try
            {
                if (_connected)
                {
                    ForecastManager manager = new ForecastManager();
                    List<VM_User> model = new List<VM_User>();
                    model = manager.GetUsers(5);
                    if (model != null)
                    {
                        return PartialView("_moduleClubMembers", model);
                    }
                    else
                    {
                        log.Error(manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60, VaryByParam = "cur_item")]
        public PartialViewResult _getModuleForecastAsHeader(int cur_item)
        {
            try
            {
                if (_connected)
                {
                    ForecastManager manager = new ForecastManager();
                    VM_ForecastHeader model = new VM_ForecastHeader();
                    model.Forecasts = manager.GetCurrentForecasts(false);                    
                    model.CurItemId = cur_item;
                    if (model.Forecasts != null)
                    {
                        return PartialView("_moduleForecastAsHeader", model);
                    }
                    else
                    {
                        log.Error(manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }            
        }        
        [ChildActionOnly]
        public PartialViewResult _getModuleAddUserToForecast(int forecastId)
        {
            try
            {
                if (_connected)
                {
                    if (String.IsNullOrEmpty(SessionPersister.UserEmail))
                    {
                        VM_UserForecastState noneAuthModel = new VM_UserForecastState();
                        noneAuthModel.AddEnable = false;
                        noneAuthModel.DisableState = EnumDisableAddUserToForecast.NonAuthorization;
                        return PartialView("_moduleDisableAddUserToForecast", noneAuthModel);
                    }

                    ForecastManager manager = new ForecastManager();
                    VM_UserForecastState model = manager.GetUserForecastState(forecastId, SessionPersister.UserId);

                    if (model.AddEnable)
                    {
                        VM_AddUserToForecast user = new VM_AddUserToForecast();
                        user.ForecastId = forecastId;
                        user.UserId = SessionPersister.UserId;
                        user.Value = "";// 0.0F;
                        return PartialView("_moduleAddUserToForecast", user);
                    }
                    else
                    {
                        return PartialView("_moduleDisableAddUserToForecast", model);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60)]
        public PartialViewResult _getModuleLastForecastWinners()
        {
            try
            {
                if (_connected)
                {                    
                    ForecastManager manager = new ForecastManager();
                    List<VM_ForecastUser> model = manager.GetLastForecastWinners();
                    return PartialView("_moduleLastForecastWinners", model);
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        #endregion
    }
}
