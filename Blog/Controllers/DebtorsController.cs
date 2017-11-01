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
                    ViewBag.DebtorsListPage = page;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public ActionResult Debtor(VM_Debtor model)
        {
            try
            {
                if (!_connected)
                {
                    log.Error(_errMassage);
                    return View("Error");
                }

                if (!Request.IsAjaxRequest())
                    return PartialView("_moduleDebtorForm", model);

                if (!ModelState.IsValid)
                    return PartialView("_moduleDebtorForm", model);

                Debtor debtor = new Debtor();
                Dictionary<string, string> modelErrors = null;
                if (!debtor.Assign(model, out modelErrors))
                {
                    foreach (var err in modelErrors)
                        ModelState.AddModelError(err.Key, err.Value);
                    return PartialView("_moduleDebtorForm", model);
                }

                if (Session["captcha_code"] != null)
                {
                    string captchaCode = Session["captcha_code"].ToString();
                    if (!model.CaptchaCode.Equals(captchaCode, StringComparison.CurrentCultureIgnoreCase))
                    {
                        ModelState.AddModelError("", "Код с картинки указан не верно!");
                        ModelState.AddModelError("CaptchaCode", "Код с картинки указан не верно!");
                        return PartialView("_moduleDebtorForm", model);
                    }
                }

                DebtorsManager manager = new DebtorsManager();
                // Создаем должника
                if (manager.EditDebtor(debtor, 1))
                {
                    VM_Debtor newDebtor = new VM_Debtor();
                    newDebtor.EditState = EnumEditState.Created;
                    return PartialView("_moduleDebtorForm", newDebtor);
                }
                else
                {
                    ModelState.AddModelError("", "По техническим причинам Ваше объявление не было отправлено! Повторите попытку позже или обратитесь в центр технической поддержки." + manager.LastError);
                    return PartialView("_moduleDebtorForm", model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ModelState.AddModelError("", "По техническим причинам Ваше объявление не было отправлено! Повторите попытку позже или обратитесь в центр технической поддержки." + ex.ToString());
                return PartialView("_moduleDebtorForm", model);
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
        //[OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
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

        //[HttpPost]
        [HttpGet]
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
        public ActionResult _getModuleDebtorDetailsById(int debtor_id)
        {
            try
            {
                if (_connected)
                {
                    if (Request.IsAjaxRequest())
                    {
                        DebtorsManager manager = new DebtorsManager();
                        Debtor model = manager.GetDebtor(debtor_id);
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
        [HttpGet]
        [ChildActionOnly]
        public ActionResult _getModuleDebtorDetails(Debtor model)
        {
            try
            {
                if (model != null)
                    return PartialView("_moduleDebtorDetails", model);
                else
                    return PartialView(_errPartialPage);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }

    }
}
