using Bankiru.Controllers;
using Bankiru.Models.Domain;
using Bankiru.Models.Domain.Users;
using Bankiru.Models.Helpers;
using Bankiru.Models.Security;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class UsersController : BaseController
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(UsersController));

        [HttpGet]
        public ActionResult List(VM_UsersFilters filter, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    HttpCookie cookie = _setUsersFiltersCookie(filter, page);
                    UserManager _manager = new UserManager();
                    VM_Users model = _manager.GetUsers(filter, page);
                    if (model != null)
                    {
                        if (cookie != null)
                            HttpContext.Response.Cookies.Add(cookie);
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        return RedirectToAction("Error", "Error", null);
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
            try
            {
                if (_connected)
                {
                    UserManager manager = new UserManager();
                    VM_User model = manager.GetUser(user_id);
                    if (model == null)
                    {
                        log.Error("Ошибка во время загрузки пользователя!\r\n" + manager.LastError);
                        return View(_errPage);
                    }
                    return View(model);
                }
                else
                {
                    log.Error("Ошибка во время загрузки пользователя!\r\n" + _errMassage);
                    return View(_errPage);
                }
            }
            catch (Exception ex)
            {
                log.Error("Ошибка во время загрузки пользователя!\r\n" + ex.ToString());
                return View(_errPage);
            }
        }
        [HttpGet]
        public ActionResult Delete(int user_id)
        {
            try
            {
                if (_connected)
                {
                    UserManager _manager = new UserManager();
                    if (true)//_manager.DeleteUser(user_id))
                    {
                        ViewBag.InfoMessage = String.Format("Пользователь успешно удален.");
                        VM_UsersFilters f = _getUsersFiltersFromCookie(HttpContext.Request.Cookies["user_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getUsersListPageFromCookie(HttpContext.Request.Cookies["user_filters"])));
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        return RedirectToAction("Error", "Error", null);
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
                            if (manager.AddBalance(model))
                            {
                                return PartialView("_userAddBalanceBlock", new VM_UserAddBalance() { SuccessMessage = "Успешно!" });
                            }
                            else
                            {
                                ModelState.AddModelError("", "Ошибка во время выполнения запроса!<br />" + manager.LastError);
                                return PartialView("_userAddBalanceBlock", model);
                            }
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
        public ActionResult PaymentConfirmed(VM_User user)
        {
            try
            {
                if (user.Id <= 0)
                    return RedirectToAction("Edit", "Users", new { user_id = user.Id });

                if (_connected)
                {
                    UserManager manager = new UserManager();
                    if (!manager.PaymentConfirmed(user.Id))
                        log.Error("[payment_confirmed] " + manager.LastError);
                    return RedirectToAction("Edit", "Users", new { user_id = user.Id});
                }
                else
                {
                    return RedirectToAction("Edit", "Users", new { user_id = user.Id });
                }
            }
            catch (Exception ex)
            {
                log.Error("[payment_confirmed] " + ex.ToString());
                return RedirectToAction("Edit", "Users", new { user_id = user.Id });
            }
        }
        
        #region ДОЧЕРНИЕ МЕТОДЫ
        [ChildActionOnly]
        public ActionResult _getUsersList(VM_Users model)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_usersList", model);
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                log.Error("Ошибка во время отображения списка пользователей!\r\n" + ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getUsersListFilter(VM_UsersFilters filter)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_usersListFilter", filter);
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                log.Error("Ошибка во время отображения фильтра пользователей!\r\n" + ex.ToString());
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
                    if(info == null)
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
        #endregion

        #region COOKIES
        private HttpCookie _setUsersFiltersCookie(VM_UsersFilters filter, int page)
        {
            try
            {
                HttpCookie cookie = new HttpCookie("art_filters");
                cookie["is_active"] = ((int)filter.IsActive).ToString();
                cookie["name"] = filter.Name;
                cookie["nic"] = filter.Nic;
                cookie["email"] = filter.Email;
                cookie["page"] = page.ToString();
                cookie.Expires = DateTime.Now.AddDays(1);
                return cookie;
            }
            catch(Exception ex)
            {
                log.Error("Ошибка во время установки куки фильтра поисков!\r\n" + ex.ToString());
                return null;
            }
        }
        private VM_UsersFilters _getUsersFiltersFromCookie(HttpCookie cookie)
        {
            try
            {
                if (cookie != null)
                {
                    VM_UsersFilters filter = new VM_UsersFilters();
                    if (cookie["is_active"] != null)
                        filter.IsActive = (EnumBoolType)(Convert.ToInt32(cookie["is_active"]));
                    else
                        filter.IsActive = EnumBoolType.None;
                    if (cookie["name"] != null)
                        filter.Name = cookie["name"].ToString();
                    else
                        filter.Name = String.Empty;
                    if (cookie["nic"] != null)
                        filter.Nic = cookie["nic"].ToString();
                    else
                        filter.Nic = String.Empty;
                    if (cookie["email"] != null)
                        filter.Email = cookie["email"].ToString();
                    else
                        filter.Email = String.Empty;
                    return filter;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        private int _getUsersListPageFromCookie(HttpCookie cookie)
        {
            try
            {
                if (cookie != null)
                {
                    if (cookie["page"] != null)
                        return Convert.ToInt32(cookie["page"]);
                    else
                        return 1;
                }
                else
                {
                    return 1;
                }
            }
            catch
            {
                return 1;
            }
        }
        #endregion
    }
}
