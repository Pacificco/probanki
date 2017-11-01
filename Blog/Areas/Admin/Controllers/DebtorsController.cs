using Bankiru.Controllers;
using Bankiru.Models.Domain;
using Bankiru.Models.Domain.Debtors;
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
    public class DebtorsController : BaseController
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(DebtorsController));

        [HttpGet]
        public ActionResult List(VM_DebtorsFilter filter, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    //HttpCookie cookie = _setDebtorsFiltersCookie(filter, page);
                    ViewBag.DebtorsListPage = page;
                    return View(filter);
                }
                else
                {
                    log.Error(_errMassage);
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return View("Error");
            }
        }
        [HttpGet]
        [ChildActionOnly]
        public ActionResult _list(VM_DebtorsFilter filter, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    DebtorsManager manager = new DebtorsManager();
                    VM_Debtors model = manager.GetDebtors(filter, page);
                    if (model != null)
                    {
                        //if (cookie != null)
                        //    HttpContext.Response.Cookies.Add(cookie);
                        return PartialView("_debtorsList", model);
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
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [HttpGet]
        [ChildActionOnly]
        public ActionResult _getModuleDebtorsFilter(VM_DebtorsFilter filter)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_debtorsFilter", filter == null ? new VM_DebtorsFilter() : filter);
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult New(VM_Debtor debtor)
        {
            return new EmptyResult();
            //try
            //{
            //    if (_connected)
            //    {
            //        UserManager _manager = new UserManager();
            //        if (_manager.DeleteUser(user_id))
            //        {
            //            ViewBag.InfoMessage = String.Format("Пользователь успешно удален.");
            //            VM_DebtorsFilter f = _getDebtorsFiltersFromCookie(HttpContext.Request.Cookies["user_filters"]);
            //            if (f == null)
            //                return RedirectToAction("List");
            //            else
            //                return RedirectToAction("List", f.GetFilterAsRouteValues(_getDebtorsListPageFromCookie(HttpContext.Request.Cookies["user_filters"])));
            //        }
            //        else
            //        {
            //            ViewBag.ErrorMessage = _manager.LastError;
            //            return RedirectToAction("Error", "Error", null);
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.ErrorMessage = _errMassage;
            //        return RedirectToAction("Error", "Error", null);
            //    }
            //}
            //catch (Exception e)
            //{
            //    ViewBag.ErrorMessage = e.ToString();
            //    return RedirectToAction("Error", "Error", null);
            //}
        }
        [HttpGet]
        public ActionResult Edit(VM_Debtor debtor)
        {
            return new EmptyResult();
            //try
            //{
            //    if (_connected)
            //    {
            //        UserManager _manager = new UserManager();
            //        if (_manager.DeleteUser(user_id))
            //        {
            //            ViewBag.InfoMessage = String.Format("Пользователь успешно удален.");
            //            VM_DebtorsFilter f = _getDebtorsFiltersFromCookie(HttpContext.Request.Cookies["user_filters"]);
            //            if (f == null)
            //                return RedirectToAction("List");
            //            else
            //                return RedirectToAction("List", f.GetFilterAsRouteValues(_getDebtorsListPageFromCookie(HttpContext.Request.Cookies["user_filters"])));
            //        }
            //        else
            //        {
            //            ViewBag.ErrorMessage = _manager.LastError;
            //            return RedirectToAction("Error", "Error", null);
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.ErrorMessage = _errMassage;
            //        return RedirectToAction("Error", "Error", null);
            //    }
            //}
            //catch (Exception e)
            //{
            //    ViewBag.ErrorMessage = e.ToString();
            //    return RedirectToAction("Error", "Error", null);
            //}
        }
        [HttpGet]
        public ActionResult Del(int debtor_id)
        {
            return new EmptyResult();
            //try
            //{
            //    if (_connected)
            //    {
            //        UserManager _manager = new UserManager();
            //        if (_manager.DeleteUser(user_id))
            //        {
            //            ViewBag.InfoMessage = String.Format("Пользователь успешно удален.");
            //            VM_DebtorsFilter f = _getDebtorsFiltersFromCookie(HttpContext.Request.Cookies["user_filters"]);
            //            if (f == null)
            //                return RedirectToAction("List");
            //            else
            //                return RedirectToAction("List", f.GetFilterAsRouteValues(_getDebtorsListPageFromCookie(HttpContext.Request.Cookies["user_filters"])));
            //        }
            //        else
            //        {
            //            ViewBag.ErrorMessage = _manager.LastError;
            //            return RedirectToAction("Error", "Error", null);
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.ErrorMessage = _errMassage;
            //        return RedirectToAction("Error", "Error", null);
            //    }
            //}
            //catch (Exception e)
            //{
            //    ViewBag.ErrorMessage = e.ToString();
            //    return RedirectToAction("Error", "Error", null);
            //}
        }
        
        #region ДОЧЕРНИЕ МЕТОДЫ
        [HttpGet]
        public ActionResult _getDebtorsList(VM_DebtorsFilter filter, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    if (Request.IsAjaxRequest())
                    {
                        DebtorsManager manager = new DebtorsManager();
                        VM_Debtors model = manager.GetDebtors(filter, page);
                        if (model != null)
                            return PartialView("_debtorsList", model);
                        else
                        {
                            log.Error(manager.LastError);
                            return PartialView(_errPartialPage);
                        }
                    }
                    else
                    {
                        log.Error("Попытка обратиться к методу _debtorsList не через Ajax!");
                        return PartialView(_errPartialPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        #endregion

        #region COOKIES
        private HttpCookie _setDebtorsFiltersCookie(VM_DebtorsFilter filter, int page)
        {
            try
            {
                HttpCookie cookie = new HttpCookie("debtors_filters");
                //cookie["is_active"] = ((int)filter.IsActive).ToString();
                //cookie["name"] = filter.Name;
                //cookie["nic"] = filter.Nic;
                //cookie["email"] = filter.Email;
                //cookie["page"] = page.ToString();
                cookie.Expires = DateTime.Now.AddDays(1);
                return cookie;
            }
            catch(Exception ex)
            {
                log.Error("Ошибка во время установки куки фильтра поисков!\r\n" + ex.ToString());
                return null;
            }
        }
        private VM_DebtorsFilter _getDebtorsFiltersFromCookie(HttpCookie cookie)
        {
            try
            {
                if (cookie != null)
                {
                    VM_DebtorsFilter filter = new VM_DebtorsFilter();
                    //if (cookie["is_active"] != null)
                    //    filter.IsActive = (EnumBoolType)(Convert.ToInt32(cookie["is_active"]));
                    //else
                    //    filter.IsActive = EnumBoolType.None;
                    //if (cookie["name"] != null)
                    //    filter.Name = cookie["name"].ToString();
                    //else
                    //    filter.Name = String.Empty;
                    //if (cookie["nic"] != null)
                    //    filter.Nic = cookie["nic"].ToString();
                    //else
                    //    filter.Nic = String.Empty;
                    //if (cookie["email"] != null)
                    //    filter.Email = cookie["email"].ToString();
                    //else
                    //    filter.Email = String.Empty;
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
        private int _getDebtorsListPageFromCookie(HttpCookie cookie)
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
