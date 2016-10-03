using Bankiru.Models.Domain.Orgs;
using Bankiru.Models.Domain.OrgsCategories;
using Bankiru.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    public class OrgsController : BaseController
    {
        [HttpGet]
        public ActionResult List(string cat_id = "", string letter = "", int page = 1)
        {
            try
            {
                if (_connected)
                {                    
                    OrgsManager _manager = new OrgsManager();
                    VM_Orgs model = _manager.GetOrgs(cat_id, letter, page);
                    if (!String.IsNullOrEmpty(cat_id))
                    {
                        model.CurrentCategory = _manager.GetOrgCategory(cat_id);
                        if (model.CurrentCategory == null)
                        {
                            return RedirectToAction("NotFound","Error", null);
                        }
                    }
                    model.Filters.Letter = letter;
                    if (model != null)
                    {                        
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
        public ActionResult Org(string org_id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    VM_Org model = _manager.GetOrg(org_id);                    
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
        public ActionResult Points(string org_id, string type = "office", int page = 1)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    VM_OrgPoints model = _manager.GetOrgPoints(org_id, type, Guid.Empty, page);
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
        public ActionResult Point()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SendReview(VM_ReviewResponse review, string org_id, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    VM_OrgItem org = _manager.GetOrgItem(review.OrgId);
                    if (org != null)
                    {
                        VM_OrgReviews model = _manager.GetOrgReviews(org.Alias, 1);
                        model.FormReviewData = review;
                        if (model != null)
                        {
                            return View(model);
                        }
                        else
                        {
                            ViewBag.ErrorMessage = _manager.LastError;
                            return RedirectToAction("NotFound", "Error", null);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Ошибка во время загрузки организации!";
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
                return RedirectToRoute("org_reviews", new { @org_id = org_id, @page = page });
                //return RedirectToAction("Error", "Error", null);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewReview(VM_ReviewResponse model)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    if (ModelState.IsValid)
                    {                        
                        if (Session["captcha_code"] != null)
                        {
                            string captchaCode = Session["captcha_code"].ToString();
                            if (!model.CaptchaCode.Equals(captchaCode, StringComparison.CurrentCultureIgnoreCase))
                            {
                                ModelState.AddModelError("", "Код с картинки указан не верно!");
                                ModelState.AddModelError("CaptchaCode", "Код с картинки указан не верно!");
                                ViewData["WarningMessage"] = "Код с картинки указан не верно!";
                                //VM_Org org = _manager.GetOrg(model.OrgId);
                                VM_OrgReviews org = _manager.GetOrgReviews(model.OrgAlias, model.Page);
                                org.FormReviewData = model;
                                //return RedirectToRoute("org_reviews", new { org_id = org.Alias, cat_id = org.CategoryAlias });
                                return View("Reviews", org);
                            }
                        }                        
                        if (_manager.CreateReview(model))
                        {
                            ViewData["InfoMessage"] = "Ваш отзыв успешно отправлен! После модерации он будет размещен на сайте.";
                            VM_Org org = _manager.GetOrg(model.OrgId);                            
                            return RedirectToRoute("org_reviews", new { org_id = org.Alias, cat_id = org.CategoryAlias });
                        }
                        else
                        {
                            ViewBag.ErrorMessage = _manager.LastError;
                            return RedirectToAction("Error", "Error", null);
                        }
                    }
                    else
                    {
                        model.CaptchaCode = "";
                        VM_Org org = _manager.GetOrg(model.OrgId);
                        return RedirectToRoute("org_send_review_org", new { org_id = org.Alias, cat_id = org.CategoryAlias, review = model });
                        //return RedirectToRoute("SendReview", model);
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
        public ActionResult NewReviewAjax(VM_ReviewResponse model)
        {
            try
            {
                if (_connected)
                {
                    if (Request.IsAjaxRequest())
                    {
                        OrgsManager _manager = new OrgsManager();
                        if (ModelState.IsValid)
                        {
                            if (Session["captcha_code"] != null)
                            {
                                string captchaCode = Session["captcha_code"].ToString();
                                if (!model.CaptchaCode.Equals(captchaCode, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    ModelState.AddModelError("CaptchaCode", "Код с картинки указан не верно!");
                                    return PartialView("_moduleFormReview", model);
                                }
                            }
                            if (_manager.CreateReview(model))
                            {
                                return PartialView("_moduleFormReview", new VM_ReviewResponse()
                                {                                    
                                    SuccessMessage = "Ваш комментарий успешно отправлен!",                                    
                                });
                            }
                            else
                            {
                                return PartialView("_partialView", "<p class=\"text-red\">Ошибка во время создания отзыва!</p>");
                            }
                        }
                        else
                        {
                            return PartialView("_moduleFormReview", model);
                        }
                    }
                    else
                    {
                        return PartialView("_partialView", "<p class=\"text-red\">Ошибка во время создания отзыва!</p>");
                    }
                }
                else
                {
                    return PartialView("_partialView", "<p class=\"text-red\">Ошибка во время создания отзыва!</p>");
                }
            }
            catch //(Exception e)
            {
                return PartialView("_partialView", "<p class=\"text-red\">Ошибка во время создания отзыва!</p>");
            }
        }
        [HttpGet]
        public ActionResult Reviews(string org_id, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    VM_OrgReviews model = _manager.GetOrgReviews(org_id, page);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        return RedirectToAction("NotFound", "Error", null);
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
        public ActionResult Review(string org_id, int review_id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    VM_OrgReview model = _manager.GetOrgReview(org_id, review_id);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;
                        return RedirectToAction("NotFound", "Error", null);
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
        public ActionResult _getModuleLeaveOrgReview(int org_id, int page, string org_alias)
        {
            try
            {
                VM_ReviewResponse model = new VM_ReviewResponse();
                model.OrgId = org_id;
                model.Page = page;
                model.OrgAlias = org_alias;
                //model.CaptchaCode = "123456";
                //Session["captcha_code"] = "123456";
                return PartialView("_moduleFormReview", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getModuleSendOrgReview(VM_ReviewResponse data)
        {
            try
            {
                data.CaptchaCode = "";
                return PartialView("_moduleFormReview", data);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
    }
}
