using Bankiru.Controllers;
using Bankiru.Models.Domain;
using Bankiru.Models.Domain.Orgs;
using Bankiru.Models.Domain.Other;
using Bankiru.Models.Helpers;
using Bankiru.Models.Infrastructure;
using Bankiru.Models.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Areas.Admin.Controllers
{    
    [CustomAuthorize(Roles="admin")]
    public class OrgsController : BaseController
    {
        #region ОРГАНИЗАЦИИ
        public ActionResult List(VM_OrgsFilters filter, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    HttpCookie cookie = _setOrgFiltersCookie(filter, page);
                    OrgsManager _manager = new OrgsManager();
                    VM_Orgs model = _manager.GetOrgs(filter, page);
                    if (model != null)
                    {
                        if (cookie != null)
                        {
                            HttpContext.Request.Cookies.Remove("org_filters");
                            HttpContext.Response.Cookies.Add(cookie);
                        }                                                    
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
        public ActionResult New()
        {
            try
            {
                if (_connected)
                {
                    return View(new VM_Org() { IsActive = true, RegionId = Guid.Parse("b756fe6b-bbd3-44d5-9302-5bfcc740f46e")});
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
        public ActionResult New(VM_Org org, HttpPostedFileBase newIcon)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        OrgsManager _manager = new OrgsManager();
                        if (_manager.CreateOrg(ref org, newIcon))
                        {
                            ViewBag.InfoMessage = String.Format("Организация \"{0}\" успешно создана.", org.Title);
                            VM_OrgsFilters f = _getOrgFiltersFromCookie(HttpContext.Request.Cookies["org_filters"]);
                            if (f == null)
                                return RedirectToAction("List");
                            else
                                return RedirectToAction("List", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["org_filters"])));
                        }
                        else
                        {
                            ViewBag.ErrorMessage = _manager.LastError;
                            return RedirectToAction("Error", "Error", null);
                        }
                    }
                    else
                    {
                        return View(org);
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
        public ActionResult Edit(int id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    VM_Org model = _manager.GetOrg(id);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VM_Org org, int id, HttpPostedFileBase newIcon)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        OrgsManager _manager = new OrgsManager();
                        if (_manager.UpdateOrg(id, org, newIcon))
                        {                
                            ViewBag.InfoMessage = String.Format("Организация \"{0}\" успешно сохранена.", org.Title);
                            VM_OrgsFilters f = _getOrgFiltersFromCookie(HttpContext.Request.Cookies["org_filters"]);
                            if (f == null)
                                return RedirectToAction("List");
                            else
                                return RedirectToAction("List", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["org_filters"])));
                        }
                        else
                        {   
                            ViewBag.ErrorMessage = _manager.LastError;
                            return RedirectToAction("Error", "Error", null);
                        }
                    }
                    else
                    {
                        return View(org);
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
        public ActionResult Del(int id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    if (_manager.DeleteOrg(id))
                    {
                        ViewBag.InfoMessage = String.Format("Организация успешно удалена.");
                        VM_OrgsFilters f = _getOrgFiltersFromCookie(HttpContext.Request.Cookies["org_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["org_filters"])));
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
        public ActionResult Activate(int id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    if (_manager.SetOrgActive(id, true))
                        {
                            VM_OrgsFilters f = _getOrgFiltersFromCookie(HttpContext.Request.Cookies["org_filters"]);
                            if (f == null)
                                return RedirectToAction("List");
                            else
                                return RedirectToAction("List", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["org_filters"])));
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
        public ActionResult Disactivate(int id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    if (_manager.SetOrgActive(id, false))
                    {
                        VM_OrgsFilters f = _getOrgFiltersFromCookie(HttpContext.Request.Cookies["org_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["org_filters"])));
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
        #endregion

        #region ОФИСЫ И БАНКОМАТЫ
        public ActionResult PointsList(int org_id, VM_OrgsPointsFilters filter, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    HttpCookie cookie = _setOrgPointsFiltersCookie(filter, page);
                    OrgsManager _manager = new OrgsManager();
                    VM_OrgPoints model = _manager.GetOrgPoints(org_id, filter, page);
                    if (model != null)
                    {                        
                        if (cookie != null)
                        {
                            HttpContext.Request.Cookies.Remove("point_filters");
                            HttpContext.Response.Cookies.Add(cookie);
                        }
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
        public ActionResult PointNew(int org_id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    VM_OrgItem org = _manager.GetOrgItem(org_id);
                    ViewBag.OrgName = org == null ? "" : org.Title;
                    return View(new VM_OrgPoint() { OrgId = org_id, IsActive = true, RegionId = Guid.Parse("b756fe6b-bbd3-44d5-9302-5bfcc740f46e") });
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
        public ActionResult PointNew(VM_OrgPoint point)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        if (point.RegionId == Guid.Empty)
                        {
                            ModelState.AddModelError("RegionId","Вы не указали регион");
                            return View(point);
                        }
                        OrgsManager _manager = new OrgsManager();
                        if (_manager.CreateOrgPoint(ref point))
                        {
                            ViewBag.InfoMessage = String.Format("Дочерний объект \"{0}\" успешно создан.", point.Title);
                            VM_OrgsPointsFilters f = _getOrgPointsFiltersFromCookie(HttpContext.Request.Cookies["point_filters"]);
                            if (f == null)
                                return RedirectToAction("PointsList");
                            else
                                return RedirectToAction("PointsList", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["point_filters"])));
                        }
                        else
                        {
                            ViewBag.ErrorMessage = _manager.LastError;
                            return RedirectToAction("Error", "Error", null);
                        }
                    }
                    else
                    {
                        return View(point);
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
        public ActionResult PointEdit(int id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    VM_OrgPoint model = _manager.GetOrgPoint(id);
                    if (model != null)
                    {
                        VM_OrgItem org = _manager.GetOrgItem(model.OrgId);
                        ViewBag.OrgName = org == null ? "" : org.Title;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PointEdit(VM_OrgPoint point, int id)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        if (point.RegionId == Guid.Empty)
                        {
                            ModelState.AddModelError("RegionId", "Вы не указали регион");
                            return View(point);
                        }
                        OrgsManager _manager = new OrgsManager();
                        if (_manager.UpdateOrgPoint(id, point))
                        {
                            ViewBag.InfoMessage = String.Format("Дочерний объект \"{0}\" успешно изменен.", point.Title);
                            VM_OrgsPointsFilters f = _getOrgPointsFiltersFromCookie(HttpContext.Request.Cookies["point_filters"]);
                            if (f == null)
                                return RedirectToAction("PointsList");
                            else
                                return RedirectToAction("PointsList", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["point_filters"])));
                        }
                        else
                        {
                            ViewBag.ErrorMessage = _manager.LastError;
                            return RedirectToAction("Error", "Error", null);
                        }
                    }
                    else
                    {
                        return View(point);
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
        public ActionResult PointDel(int org_id, int id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    if (_manager.DeleteOrgPoint(id))
                    {
                        ViewBag.InfoMessage = "Дочерний объект успешно удален.";
                        VM_OrgsPointsFilters f = _getOrgPointsFiltersFromCookie(HttpContext.Request.Cookies["point_filters"]);
                        if (f == null)
                            return RedirectToAction("PointsList");
                        else
                            return RedirectToAction("PointsList", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["point_filters"])));
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
        public ActionResult PointActivate(int id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    if (_manager.SetOrgPointActive(id, true))
                    {
                        VM_OrgsPointsFilters f = _getOrgPointsFiltersFromCookie(HttpContext.Request.Cookies["point_filters"]);
                        if (f == null)
                            return RedirectToAction("PointsList");
                        else
                            return RedirectToAction("PointsList", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["point_filters"])));
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
        public ActionResult PointDisactivate(int id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    if (_manager.SetOrgPointActive(id, false))
                    {
                        VM_OrgsPointsFilters f = _getOrgPointsFiltersFromCookie(HttpContext.Request.Cookies["point_filters"]);
                        if (f == null)
                            return RedirectToAction("PointsList");
                        else
                            return RedirectToAction("PointsList", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["point_filters"])));
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
        #endregion

        #region ОТЗЫВЫ
        public ActionResult ReviewsList(VM_OrgsReviewsFilters filter, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    HttpCookie cookie = _setOrgReviewsFiltersCookie(filter, page);
                    OrgsManager _manager = new OrgsManager();
                    VM_OrgReviews model = _manager.GetOrgReviews(null, filter, page);
                    if (model != null)
                    {
                        if (cookie != null)
                        {
                            HttpContext.Request.Cookies.Remove("review_filters");
                            HttpContext.Response.Cookies.Add(cookie);
                        }
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
        public ActionResult ReviewNew(int org_id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    VM_OrgItem org = _manager.GetOrgItem(org_id);
                    ViewBag.OrgName = org == null ? "" : org.Title;
                    return View(new VM_OrgReview());
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
        public ActionResult ReviewNew(VM_Review review)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {                        
                        OrgsManager _manager = new OrgsManager();                        
                        if (_manager.CreateOrgReview(ref review))
                        {
                            ViewBag.InfoMessage = "Отзыв успешно создан";
                            VM_OrgsPointsFilters f = _getOrgPointsFiltersFromCookie(HttpContext.Request.Cookies["review_filters"]);
                            if (f == null)
                                return RedirectToAction("ReviewsList");
                            else
                                return RedirectToAction("ReviewsList", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["review_filters"])));
                        }
                        else
                        {
                            ViewBag.ErrorMessage = _manager.LastError;
                            return RedirectToAction("Error", "Error", null);
                        }
                    }
                    else
                    {
                        return View(review);
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
        public ActionResult ReviewEdit(int id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    VM_Review model = _manager.GetReview(id);
                    if (model != null)
                    {
                        VM_OrgItem org = _manager.GetOrgItem(model.Org.Id);
                        ViewBag.OrgName = org == null ? "" : org.Title;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReviewEdit(VM_Review review, int id)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {                        
                        OrgsManager _manager = new OrgsManager();
                        if (_manager.UpdateOrgReview(id, review))
                        {
                            //ViewBag.InfoMessage = "Отзыв успешно изменен";
                            ViewData["InfoMessage"] = "Отзыв успешно изменен";
                            VM_OrgsPointsFilters f = _getOrgPointsFiltersFromCookie(HttpContext.Request.Cookies["review_filters"]);
                            if (f == null)
                                return RedirectToAction("ReviewsList");
                            else
                                return RedirectToAction("ReviewsList", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["review_filters"])));
                        }
                        else
                        {
                            ViewBag.ErrorMessage = _manager.LastError;
                            return RedirectToAction("Error", "Error", null);
                        }
                    }
                    else
                    {
                        return View(review);
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
        public ActionResult ReviewDel(int id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    if (_manager.DeleteOrgReview(id))
                    {
                        ViewBag.InfoMessage = String.Format("Отзыв успешно удален.");
                        VM_OrgsPointsFilters f = _getOrgPointsFiltersFromCookie(HttpContext.Request.Cookies["review_filters"]);
                        if (f == null)
                            return RedirectToAction("ReviewsList");
                        else
                            return RedirectToAction("ReviewsList", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["review_filters"])));
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
        public ActionResult AjaxReviewActive(int id, bool is_active)
        {
            try
            {
                if (_connected)
                {                    
                    OrgsManager _manager = new OrgsManager();
                    if (_manager.SetOrgReviewActive(id, is_active))
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
        [HttpPost]
        public ActionResult AjaxReviewConfirm(int id, bool is_confirmed)
        {
            try
            {
                if (_connected)
                {                    
                    OrgsManager _manager = new OrgsManager();
                    if (_manager.SetReviewComfirmed(id, is_confirmed))
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
        [HttpGet]
        public ActionResult ReviewActivate(int id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    if (_manager.SetOrgReviewActive(id, true))
                    {
                        VM_OrgsReviewsFilters f = _getOrgReviewsFiltersFromCookie(HttpContext.Request.Cookies["review_filters"]);
                        if (f == null)
                            return RedirectToAction("ReviewsList");
                        else
                            return RedirectToAction("ReviewsList", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["review_filters"])));
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
        public ActionResult ReviewDisactivate(int id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    if (_manager.SetOrgReviewActive(id, false))
                    {
                        VM_OrgsReviewsFilters f = _getOrgReviewsFiltersFromCookie(HttpContext.Request.Cookies["review_filters"]);
                        if (f == null)
                            return RedirectToAction("ReviewsList");
                        else
                            return RedirectToAction("ReviewsList", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["review_filters"])));
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
        public ActionResult ReviewConfirm(int id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    if (_manager.SetReviewComfirmed(id, true))
                    {
                        VM_OrgsReviewsFilters f = _getOrgReviewsFiltersFromCookie(HttpContext.Request.Cookies["review_filters"]);
                        if (f == null)
                            return RedirectToAction("ReviewsList");
                        else
                            return RedirectToAction("ReviewsList", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["review_filters"])));
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
        public ActionResult ReviewNotConfirm(int id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager _manager = new OrgsManager();
                    if (_manager.SetReviewComfirmed(id, false))
                    {
                        VM_OrgsReviewsFilters f = _getOrgReviewsFiltersFromCookie(HttpContext.Request.Cookies["review_filters"]);
                        if (f == null)
                            return RedirectToAction("ReviewsList");
                        else
                            return RedirectToAction("ReviewsList", f.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["review_filters"])));
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
        #endregion
        
        #region ДОЧЕРНИЕ МЕТОДЫ
        //Организации
        [ChildActionOnly]
        public ActionResult _getOrgList(VM_Orgs model)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_orgList", model);
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return PartialView(_errPartialPage);
            }
        }        
        [ChildActionOnly]        
        public ActionResult _getOrgListFilter(VM_OrgsFilters filter)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_orgListFilter", filter);
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _backToOrgList()
        {
            try
            {
                VM_OrgsFilters model = _getOrgFiltersFromCookie(HttpContext.Request.Cookies["org_filters"]);
                if (model != null)
                {
                    object routeValues = model.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["org_filters"]));
                    return PartialView("_backToOrgList", routeValues);
                }
                else
                {
                    return PartialView("_backToOrgList", null);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return PartialView(_errPartialPage);
            }
        }        
        //Офисы
        [ChildActionOnly]
        public ActionResult _getOrgPointsListFilter(VM_OrgsPointsFilters filter)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_orgPointsListFilter", filter);
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getOrgPointsList(VM_OrgPoints model)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_orgPointsList", model);
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _backToOrgPointsList()
        {
            try
            {
                VM_OrgsFilters model = _getOrgFiltersFromCookie(HttpContext.Request.Cookies["point_filters"]);
                if (model != null)
                {
                    object routeValues = model.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["point_filters"]));
                    return PartialView("_backToOrgPointsList", routeValues);
                }
                else
                {
                    return PartialView("_backToOrgPointsList", null);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return PartialView(_errPartialPage);
            }
        }    
        //Отзывы
        [ChildActionOnly]
        public ActionResult _getOrgReviewsListFilter(VM_OrgsReviewsFilters filter)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_orgReviewsListFilter", filter);
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getOrgReviewsList(VM_OrgReviews model)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_orgReviewsList", model);
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _backToOrgReviewsList()
        {
            try
            {
                VM_OrgsFilters model = _getOrgFiltersFromCookie(HttpContext.Request.Cookies["review_filters"]);
                if (model != null)
                {
                    object routeValues = model.GetFilterAsRouteValues(_getOrgListPageFromCookie(HttpContext.Request.Cookies["point_filters"]));
                    return PartialView("_backToOrgReviewsList", routeValues);
                }
                else
                {
                    return PartialView("_backToOrgReviewsList", null);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return PartialView(_errPartialPage);
            }
        }            
        #endregion

        #region ДОП МЕТОДЫ
        //Организации
        private HttpCookie _setOrgFiltersCookie(VM_OrgsFilters filter, int page)
        {
            try
            {
                HttpCookie cookie = new HttpCookie("org_filters");
                cookie["is_active"] = ((int)filter.IsActive).ToString();
                cookie["category_id"] = filter.CategoryId.ToString();
                cookie["region_id"] = filter.RegionId.ToString();
                cookie["title"] = filter.Title;
                cookie["page"] = page.ToString();
                cookie.Expires = DateTime.Now.AddDays(1);
                return cookie;
            }
            catch
            { 
                //Логирование
                return null;
            }
        }
        private VM_OrgsFilters _getOrgFiltersFromCookie(HttpCookie cookie)
        {
            try
            {                
                if (cookie != null)             
                {
                    VM_OrgsFilters filter = new VM_OrgsFilters();
                    if (cookie["is_active"] != null)
                        filter.IsActive = (EnumBoolType)(Convert.ToInt32(cookie["is_active"]));
                    else
                        filter.IsActive = EnumBoolType.None;
                    if (cookie["category_id"] != null)
                        filter.CategoryId = (Convert.ToInt32(cookie["category_id"]));
                    else
                        filter.CategoryId = 0;
                    if (cookie["region_id"] != null)
                        filter.RegionId = Guid.Parse(cookie["region_id"]);
                    else
                        filter.RegionId = Guid.Empty;
                    if (cookie["title"] != null)
                        filter.Title = cookie["title"].ToString(CultureInfo.CurrentCulture);
                    else
                        filter.Title = String.Empty;
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
        //Офисы
        private HttpCookie _setOrgPointsFiltersCookie(VM_OrgsPointsFilters filter, int page)
        {
            try
            {
                HttpCookie cookie = new HttpCookie("point_filters");
                cookie["is_active"] = ((int)filter.IsActive).ToString();
                cookie["address"] = filter.Address;
                cookie["region_id"] = filter.RegionId.ToString();
                cookie["title"] = filter.Title;
                cookie["page"] = page.ToString();
                cookie.Expires = DateTime.Now.AddDays(1);
                return cookie;
            }
            catch
            {
                //Логирование
                return null;
            }
        }
        private VM_OrgsPointsFilters _getOrgPointsFiltersFromCookie(HttpCookie cookie)
        {
            try
            {
                if (cookie != null)
                {
                    VM_OrgsPointsFilters filter = new VM_OrgsPointsFilters();
                    if (cookie["is_active"] != null)
                        filter.IsActive = (EnumBoolType)(Convert.ToInt32(cookie["is_active"]));
                    else
                        filter.IsActive = EnumBoolType.None;
                    if (cookie["address"] != null)
                        filter.Address = cookie["address"];
                    else
                        filter.Address = "";
                    if (cookie["region_id"] != null)
                        filter.RegionId = Guid.Parse(cookie["region_id"]);
                    else
                        filter.RegionId = Guid.Empty;
                    if (cookie["title"] != null)
                        filter.Title = cookie["title"];
                    else
                        filter.Title = String.Empty;
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
        //Отзывы
        private HttpCookie _setOrgReviewsFiltersCookie(VM_OrgsReviewsFilters filter, int page)
        {
            try
            {
                HttpCookie cookie = new HttpCookie("review_filters");
                cookie["is_active"] = ((int)filter.IsActive).ToString();
                cookie["confirm_status"] = ((int)filter.Confirmed).ToString();                
                cookie["review_text"] = filter.ReviewText;
                cookie["page"] = page.ToString();
                cookie.Expires = DateTime.Now.AddDays(1);
                return cookie;
            }
            catch
            {
                //Логирование
                return null;
            }
        }
        private VM_OrgsReviewsFilters _getOrgReviewsFiltersFromCookie(HttpCookie cookie)
        {
            try
            {
                if (cookie != null)
                {
                    VM_OrgsReviewsFilters filter = new VM_OrgsReviewsFilters();
                    if (cookie["is_active"] != null)
                        filter.IsActive = (EnumBoolType)(Convert.ToInt32(cookie["is_active"]));
                    else
                        filter.IsActive = EnumBoolType.None;
                    if (cookie["confirm_status"] != null)
                        filter.Confirmed = (EnumBoolType)(Convert.ToInt32(cookie["confirm_status"]));
                    else
                        filter.Confirmed = EnumBoolType.None;
                    if (cookie["review_text"] != null)
                        filter.ReviewText = cookie["review_text"];
                    else
                        filter.ReviewText = String.Empty;
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
        //Общее
        private int _getOrgListPageFromCookie(HttpCookie cookie)
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
