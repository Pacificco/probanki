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
        [HttpGet]
        public ActionResult List(VM_DebtorsFilter filter, int page = 1, bool back_to_list = false)
        {
            try
            {
                if (_connected)
                {
                    if (back_to_list)
                    {
                        if (HttpContext.Request.Cookies["debtors_filters"] != null)
                            filter = _getDebtorsFiltersFromCookie(HttpContext.Request.Cookies["debtors_filters"]);
                        else
                            filter = new VM_DebtorsFilter();
                    }
                    else
                    {
                        HttpCookie cookie = _setDebtorsFiltersCookie(filter, page);
                        ViewBag.DebtorsListPage = page;
                        if (cookie != null)
                            HttpContext.Response.Cookies.Add(cookie);
                    }
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
        public ActionResult New()
        {
            try
            {
                if (_connected)
                {
                    return View(new VM_Debtor());
                }
                else
                {
                    log.Error("Соединение с сервером не установлено! " + _errMassage);
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
        public ActionResult New(VM_Debtor model)
        {
            try
            {
                if (_connected)
                {
                    ModelState["CaptchaCode"].Errors.Clear();
                    if (!ModelState.IsValid)
                        return View(model);

                    DebtorsManager manager = new DebtorsManager();
                    Debtor debtor = new Debtor();
                    Dictionary<string, string> modelErrors = new Dictionary<string, string>();
                    if (!debtor.Assign(model, out modelErrors))
                    {
                        foreach (var err in modelErrors)
                            ModelState.AddModelError(err.Key, err.Value);
                        return View(model);
                    }
                    if (!manager.EditDebtor(debtor, 1))
                    {
                        log.Error("Ошибка во время создания должника! " + manager.LastError);
                        return RedirectToAction("Error", "Error", null);
                    }

                    ViewBag.InfoMessage = "Должник успешно создан.";
                    VM_DebtorsFilter f = _getDebtorsFiltersFromCookie(HttpContext.Request.Cookies["debtors_filters"]);
                    if (f == null)
                        return RedirectToAction("List");
                    else
                        return RedirectToAction("List", f.GetFilterAsRouteValues(_getDebtorsListPageFromCookie(HttpContext.Request.Cookies["debtors_filters"])));
                }
                else
                {
                    log.Error("Соединение с сервером не установлено! " + _errMassage);
                    return RedirectToAction("Error", "Error", null);
                }
            }
            catch (Exception e)
            {
                log.Error("Ошибка во время создания должника! " + e.ToString());
                return RedirectToAction("Error", "Error", null);
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                if (_connected)
                {
                    DebtorsManager manager = new DebtorsManager();
                    Debtor debtor = manager.GetDebtor(id);
                    if (debtor == null)
                    {
                        log.Error("Должник с Id=" + id.ToString() + " не найден! " + manager.LastError);
                        return RedirectToAction("Error", "Error", null);
                    }
                    VM_Debtor model = new VM_Debtor();
                    model.Assign(debtor);
                    return View(model);
                }
                else
                {
                    log.Error("Должник с Id=" + id.ToString() + " не найден! " + _errMassage);
                    return RedirectToAction("Error", "Error", null);
                }
            }
            catch (Exception e)
            {
                log.Error("Должник с Id=" + id.ToString() + " не найден! " + e.ToString());
                return RedirectToAction("Error", "Error", null);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VM_Debtor model, int id)
        {
            try
            {
                if (_connected)
                {
                    ModelState["CaptchaCode"].Errors.Clear();
                    if (!ModelState.IsValid)
                        return View(model);

                    DebtorsManager manager = new DebtorsManager();
                    Debtor debtor = new Debtor();
                    Dictionary<string, string> modelErrors = new Dictionary<string, string>();
                    if (!debtor.Assign(model, out modelErrors))
                    {
                        foreach (var err in modelErrors)
                            ModelState.AddModelError(err.Key, err.Value);
                        return View(model);
                    }
                    if (!manager.EditDebtor(debtor, 2))                    
                    {
                        log.Error("Ошибка во время изменения должника с Id=" + id.ToString() + "! " + manager.LastError);
                        return RedirectToAction("Error", "Error", null);
                    }

                    ViewBag.InfoMessage = "Должник успешно изменен.";
                    VM_DebtorsFilter f = _getDebtorsFiltersFromCookie(HttpContext.Request.Cookies["debtors_filters"]);
                    if (f == null)
                        return RedirectToAction("List");
                    else
                        return RedirectToAction("List", f.GetFilterAsRouteValues(_getDebtorsListPageFromCookie(HttpContext.Request.Cookies["debtors_filters"])));

                    //return View(model);
                }
                else
                {
                    log.Error("Должник с Id=" + id.ToString() + " не найден! " + _errMassage);
                    return RedirectToAction("Error", "Error", null);
                }
            }
            catch (Exception e)
            {
                log.Error("Должник с Id=" + id.ToString() + " не найден! " + e.ToString());
                return RedirectToAction("Error", "Error", null);
            }
        }
        [HttpGet]
        public ActionResult Del(int id)
        {
            try
            {
                if (_connected)
                {
                    DebtorsManager manager = new DebtorsManager();
                    if (manager.DeleteDebtor(id))
                    {
                        ViewBag.InfoMessage = String.Format("Должник успешно удален.");
                        VM_DebtorsFilter f = _getDebtorsFiltersFromCookie(HttpContext.Request.Cookies["debtors_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getDebtorsListPageFromCookie(HttpContext.Request.Cookies["debtors_filters"])));
                    }
                    else
                    {
                        log.Error("Ошибка во время удаления должника с Id=" + id.ToString() + "! " + manager.LastError);
                        return RedirectToAction("Error", "Error", null);
                    }
                }
                else
                {
                    log.Error("Должник с Id=" + id.ToString() + " не найден! " + _errMassage);
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
        public ActionResult AjaxActive(int id, bool is_active)
        {
            try
            {
                if (_connected)
                {
                    DebtorsManager manager = new DebtorsManager();
                    if (manager.SetDebtorActive(id, is_active))
                    {
                        return Json(new { resultMessage = "OK" });
                    }
                    else
                    {
                        return Json(new { resultMessage = "ERROR" });
                    }
                }
                else
                {
                    return Json(new { resultMessage = "ERROR" });
                }
            }
            catch
            {
                return Json(new { resultMessage = "ERROR" });
            }
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
                cookie["is_active"] = ((int)filter.Published).ToString();
                cookie["region_id"] = filter.RegionId.ToString();
                cookie["sale_price_range"] = filter.SalePriceRange.ToString();
                cookie["original_creditor_type"] = ((int)filter.OriginalCreditorType).ToString();
                cookie["debt_seller_type"] = ((int)filter.DebtSellerType).ToString();
                cookie["debtor_type"] = ((int)filter.DebtorType).ToString();
                cookie["debt_essence_type"] = ((int)filter.DebtEssenceType).ToString();
                cookie["court_decision_type"] = ((int)filter.CourtDecisionType).ToString();
                cookie["debt_amount_range"] = filter.DebtAmountRange.ToString();
                cookie["debt_created_range"] = filter.DebtCreatedRange.ToString();
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
        private VM_DebtorsFilter _getDebtorsFiltersFromCookie(HttpCookie cookie)
        {
            try
            {
                if (cookie != null)
                {
                    VM_DebtorsFilter filter = new VM_DebtorsFilter();
                    if (cookie["is_active"] != null)
                        filter.Published = (EnumBoolType)(Convert.ToInt32(cookie["is_active"]));
                    else
                        filter.Published = EnumBoolType.None;
                    if (cookie["region_id"] != null)
                        filter.RegionId = Guid.Parse(cookie["region_id"]);
                    else
                        filter.RegionId = Guid.Empty;
                    if (cookie["original_creditor_type"] != null)
                        filter.OriginalCreditorType = (EnumOriginalCreditorType)(Convert.ToInt32(cookie["original_creditor_type"]));
                    else
                        filter.OriginalCreditorType = EnumOriginalCreditorType.Undefind;
                    if (cookie["debtor_type"] != null)
                        filter.DebtorType = (EnumDebtorType)(Convert.ToInt32(cookie["debtor_type"]));
                    else
                        filter.DebtorType = EnumDebtorType.Undefind;
                    if (cookie["court_decision_type"] != null)
                        filter.CourtDecisionType = (EnumCourtDecisionType)(Convert.ToInt32(cookie["court_decision_type"]));
                    else
                        filter.CourtDecisionType = EnumCourtDecisionType.Undefind;
                    if (cookie["debt_essence_type"] != null)
                        filter.DebtEssenceType = (EnumDebtEssenceType)(Convert.ToInt32(cookie["debt_essence_type"]));
                    else
                        filter.DebtEssenceType = EnumDebtEssenceType.Undefind;
                    if (cookie["debt_seller_type"] != null)
                        filter.DebtSellerType = (EnumDebtSellerType)(Convert.ToInt32(cookie["debt_seller_type"]));
                    else
                        filter.DebtSellerType = EnumDebtSellerType.Undefind;
                    if (cookie["sale_price_range"] != null)
                        filter.SalePriceRange = Convert.ToInt32(cookie["sale_price_range"]);
                    else
                        filter.SalePriceRange = 0;
                    if (cookie["debt_amount_range"] != null)
                        filter.DebtAmountRange = Convert.ToInt32(cookie["debt_amount_range"]);
                    else
                        filter.DebtAmountRange = 0;
                    if (cookie["debt_created_range"] != null)
                        filter.DebtCreatedRange = Convert.ToInt32(cookie["debt_created_range"]);
                    else
                        filter.DebtCreatedRange = 0;
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
