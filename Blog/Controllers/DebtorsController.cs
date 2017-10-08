using Bankiru.Models.Domain;
using Bankiru.Models.Domain.Debtors;
using Bankiru.Models.Domain.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    public class DebtorsController : BaseController
    {
        [HttpGet]
        public ActionResult Index(VM_DebtorsFilter filter, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    return View();                    
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
                        return PartialView("_moduleDebtorsList", model);
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
                    return PartialView("_moduleDebtorsFilter", filter == null ? new VM_DebtorsFilter() : filter);
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
        [ChildActionOnly]
        public ActionResult _getModuleDebtorForm()
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_moduleDebtorForm", new VM_Debtor());
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

        [HttpPost]
        public ActionResult _getModuleDebtorsList(VM_DebtorsFilter filter, int page = 1)
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
                            return PartialView("_moduleDebtorsList", model);
                        else
                        {
                            log.Error(manager.LastError);
                            return PartialView(_errPartialPage);
                        }
                    }
                    else
                    {
                        log.Error("Попытка обратиться к методу _getModuleDebtorsList не через Ajax!");
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
        [HttpPost]
        public ActionResult _getModuleDebtorDetails(int debtor_id)
        {
            try
            {
                if (_connected)
                {
                    if (Request.IsAjaxRequest())
                    {
                        DebtorsManager manager = new DebtorsManager();
                        VM_Debtor model = manager.GetDebtor(debtor_id);
                        if (model != null)
                        {
                            return PartialView("_moduleDebtorDetails", model);
                        }
                        else
                        {
                            return PartialView(_errPartialPage);
                        }
                    }
                    else
                    {
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

        #region СПИСКИ ДЛЯ ФИЛЬТРОВ
        [HttpGet]
        [ChildActionOnly]
        public ActionResult _getDebtCreatedRangeDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                VM_StringDropDown model = new VM_StringDropDown();
                model.FirstItem = firstItem;
                model.Name = id;
                model.SelectedId = selectedId;
                return PartialView("_debtDebtCreatedRangeDropDownList", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [HttpGet]
        [ChildActionOnly]
        public ActionResult _getDebtAmountRangeDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                VM_StringDropDown model = new VM_StringDropDown();
                model.FirstItem = firstItem;
                model.Name = id;
                model.SelectedId = selectedId;
                return PartialView("_debtDebtAmountRangeDropDownList", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [HttpGet]
        [ChildActionOnly]
        public ActionResult _getDebtSalePriceRangeDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                VM_StringDropDown model = new VM_StringDropDown();
                model.FirstItem = firstItem;
                model.Name = id;
                model.SelectedId = selectedId;
                return PartialView("_debtSalePriceRangeDropDownList", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [HttpGet]
        [ChildActionOnly]
        public ActionResult _getCourtDecisionTypesDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                VM_StringDropDown model = new VM_StringDropDown();
                model.FirstItem = firstItem;
                model.Name = id;
                model.SelectedId = selectedId;
                return PartialView("_debtCourtDecisionTypesDropDownList", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [HttpGet]
        [ChildActionOnly]
        public ActionResult _getDebtorTypesDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                VM_StringDropDown model = new VM_StringDropDown();
                model.FirstItem = firstItem;
                model.Name = id;
                model.SelectedId = selectedId;
                return PartialView("_debtDebtorTypesDropDownList", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [HttpGet]
        [ChildActionOnly]
        public ActionResult _getDebtEssenceTypesDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                VM_StringDropDown model = new VM_StringDropDown();
                model.FirstItem = firstItem;
                model.Name = id;
                model.SelectedId = selectedId;
                return PartialView("_debtDebtEssenceTypesDropDownList", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [HttpGet]
        [ChildActionOnly]
        public ActionResult _getOriginalCreditorTypesDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                VM_StringDropDown model = new VM_StringDropDown();
                model.FirstItem = firstItem;
                model.Name = id;
                model.SelectedId = selectedId;
                return PartialView("_debtOriginalCreditorTypesDropDownList", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [HttpGet]
        [ChildActionOnly]
        public ActionResult _getDebtSellerTypesDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                VM_StringDropDown model = new VM_StringDropDown();
                model.FirstItem = firstItem;
                model.Name = id;
                model.SelectedId = selectedId;
                return PartialView("_debtDebtSellerTypesDropDownList", model);
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
