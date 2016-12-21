using Bankiru.Controllers;
using Bankiru.Models.Domain;
using Bankiru.Models.Domain.Articles;
using Bankiru.Models.Infrastructure;
using Bankiru.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class ArticlesController : BaseController
    {
        [HttpGet]
        public ActionResult List(VM_ArtsFilters filter, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    HttpCookie cookie = _setArtFiltersCookie(filter, page);
                    ArtsManager _manager = new ArtsManager();
                    VM_Articles model = _manager.GetArts(filter, page);
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
        public ActionResult New()
        {
            try
            {
                if (_connected)
                {
                    return View(new VM_Article());
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
        public ActionResult New(VM_Article art)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        ArtsManager _manager = new ArtsManager();
                        if (_manager.CreateArt(ref art))
                        {
                            ViewBag.InfoMessage = String.Format("Статья \"{0}\" успешно создана.", art.Title);
                            VM_ArtsFilters f = _getArtFiltersFromCookie(HttpContext.Request.Cookies["art_filters"]);
                            if (f == null)
                                return RedirectToAction("List");
                            else
                                return RedirectToAction("List", f.GetFilterAsRouteValues(_getArtListPageFromCookie(HttpContext.Request.Cookies["art_filters"])));
                        }
                        else
                        {
                            ViewBag.ErrorMessage = _manager.LastError;
                            return RedirectToAction("Error", "Error", null);
                        }
                    }
                    else
                    {
                        return View(art);
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
                    ArtsManager _manager = new ArtsManager();
                    VM_Article model = _manager.GetArt(id);
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
        public ActionResult Edit(VM_Article org, int id)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        ArtsManager _manager = new ArtsManager();
                        if (_manager.UpdateArt(id, org))
                        {
                            ViewBag.InfoMessage = String.Format("Статья \"{0}\" успешно сохранена.", org.Title);
                            VM_ArtsFilters f = _getArtFiltersFromCookie(HttpContext.Request.Cookies["art_filters"]);
                            if (f == null)
                                return RedirectToAction("List");
                            else
                                return RedirectToAction("List", f.GetFilterAsRouteValues(_getArtListPageFromCookie(HttpContext.Request.Cookies["art_filters"])));
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
                    ArtsManager _manager = new ArtsManager();
                    if (_manager.DeleteArt(id))
                    {
                        ViewBag.InfoMessage = String.Format("Публикация успешно удалена.");
                        VM_ArtsFilters f = _getArtFiltersFromCookie(HttpContext.Request.Cookies["art_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getArtListPageFromCookie(HttpContext.Request.Cookies["art_filters"])));
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
                    ArtsManager _manager = new ArtsManager();
                    if (_manager.SetArtActive(id, true))
                    {
                        VM_ArtsFilters f = _getArtFiltersFromCookie(HttpContext.Request.Cookies["art_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getArtListPageFromCookie(HttpContext.Request.Cookies["art_filters"])));
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
        public ActionResult AjaxActive(int id, bool is_active)
        {
            try
            {
                if (_connected)
                {
                    ArtsManager _manager = new ArtsManager();
                    if (_manager.SetArtActive(id, is_active))
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
        public ActionResult Disactivate(int id)
        {
            try
            {
                if (_connected)
                {
                    ArtsManager _manager = new ArtsManager();
                    if (_manager.SetArtActive(id, false))
                    {
                        VM_ArtsFilters f = _getArtFiltersFromCookie(HttpContext.Request.Cookies["art_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getArtListPageFromCookie(HttpContext.Request.Cookies["art_filters"])));
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

        #region ДОЧЕРНИЕ МЕТОДЫ
        [ChildActionOnly]
        public ActionResult _getArtList(VM_Articles model)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_artList", model);
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
        public ActionResult _getArtListFilter(VM_ArtsFilters filter)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_artListFilter", filter);
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
        #endregion

        #region ДОП МЕТОДЫ
        private HttpCookie _setArtFiltersCookie(VM_ArtsFilters filter, int page)
        {
            try
            {
                HttpCookie cookie = new HttpCookie("art_filters");
                cookie["is_active"] = ((int)filter.IsActive).ToString();
                cookie["is_central"] = filter.IsCentral ? "1" : "0";
                cookie["category_id"] = filter.CategoryId.ToString();                
                cookie["title"] = filter.Title.ToString();
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
        private VM_ArtsFilters _getArtFiltersFromCookie(HttpCookie cookie)
        {
            try
            {
                if (cookie != null)
                {
                    VM_ArtsFilters filter = new VM_ArtsFilters();
                    if (cookie["is_active"] != null)
                        filter.IsActive = (EnumBoolType)(Convert.ToInt32(cookie["is_active"]));
                    else
                        filter.IsActive = EnumBoolType.None;
                    if (cookie["is_active"] != null)
                        filter.IsCentral = cookie["is_central"] == "1" ? true : false;
                    else
                        filter.IsCentral = false;
                    if (cookie["category_id"] != null)
                        filter.CategoryId = (Convert.ToInt32(cookie["category_id"]));
                    else
                        filter.CategoryId = 0;                    
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
        private int _getArtListPageFromCookie(HttpCookie cookie)
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
