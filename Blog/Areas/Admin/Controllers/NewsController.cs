using Bankiru.Controllers;
using Bankiru.Models.Domain;
using Bankiru.Models.Domain.News;
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
    public class NewsController : BaseController
    {
        public ActionResult List(VM_NewsFilters filter, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    HttpCookie cookie = _setNewsFiltersCookie(filter, page);
                    NewsManager _manager = new NewsManager();
                    VM_NewsList model = _manager.GetNewsList(filter, page);
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
                    return View(new VM_News() { Author = "probanki.net", SourceUrl = "http://probanki.net" });
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
        public ActionResult New(VM_News art)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        NewsManager _manager = new NewsManager();
                        if (_manager.CreateNews(ref art))
                        {
                            ViewBag.InfoMessage = String.Format("Новость \"{0}\" успешно создана.", art.Title);
                            VM_NewsFilters f = _getNewsFiltersFromCookie(HttpContext.Request.Cookies["news_filters"]);
                            if (f == null)
                                return RedirectToAction("List");
                            else
                                return RedirectToAction("List", f.GetFilterAsRouteValues(_getNewsListPageFromCookie(HttpContext.Request.Cookies["news_filters"])));
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
                    NewsManager _manager = new NewsManager();
                    VM_News model = _manager.GetNews(id);
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
        public ActionResult Edit(VM_News org, int id)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        NewsManager _manager = new NewsManager();
                        if (_manager.UpdateNews(id, org))
                        {
                            ViewBag.InfoMessage = String.Format("Новость \"{0}\" успешно сохранена.", org.Title);
                            VM_NewsFilters f = _getNewsFiltersFromCookie(HttpContext.Request.Cookies["news_filters"]);
                            if (f == null)
                                return RedirectToAction("List");
                            else
                                return RedirectToAction("List", f.GetFilterAsRouteValues(_getNewsListPageFromCookie(HttpContext.Request.Cookies["news_filters"])));
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Del(int id)
        {
            return View();
        }
        [HttpGet]
        public ActionResult Activate(int id)
        {
            try
            {
                if (_connected)
                {
                    NewsManager _manager = new NewsManager();
                    if (_manager.SetNewsActive(id, true))
                    {
                        VM_NewsFilters f = _getNewsFiltersFromCookie(HttpContext.Request.Cookies["news_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getNewsListPageFromCookie(HttpContext.Request.Cookies["news_filters"])));
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
                    NewsManager _manager = new NewsManager();
                    if (_manager.SetNewsActive(id, false))
                    {
                        VM_NewsFilters f = _getNewsFiltersFromCookie(HttpContext.Request.Cookies["news_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getNewsListPageFromCookie(HttpContext.Request.Cookies["news_filters"])));
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
        public ActionResult _getNewsList(VM_NewsList model)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_newsList", model);
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
        public ActionResult _getNewsListFilter(VM_NewsFilters filter)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_newsListFilter", filter);
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
        private HttpCookie _setNewsFiltersCookie(VM_NewsFilters filter, int page)
        {
            try
            {
                HttpCookie cookie = new HttpCookie("news_filters");
                cookie["is_active"] = ((int)filter.IsActive).ToString();
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
        private VM_NewsFilters _getNewsFiltersFromCookie(HttpCookie cookie)
        {
            try
            {
                if (cookie != null)
                {
                    VM_NewsFilters filter = new VM_NewsFilters();
                    if (cookie["is_active"] != null)
                        filter.IsActive = (EnumBoolType)(Convert.ToInt32(cookie["is_active"]));
                    else
                        filter.IsActive = EnumBoolType.None;                    
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
        private int _getNewsListPageFromCookie(HttpCookie cookie)
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
