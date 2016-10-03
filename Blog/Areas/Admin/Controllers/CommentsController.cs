using Bankiru.Controllers;
using Bankiru.Models.Domain;
using Bankiru.Models.Domain.Comments;
using Bankiru.Models.Infrastructure;
using Bankiru.Models.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class CommentsController : BaseController
    {
        [HttpGet]
        public ActionResult List(VM_CommentsFilters filter, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    HttpCookie cookie = _setCommentFiltersCookie(filter, page);
                    CommentsManager _manager = new CommentsManager();
                    VM_Comments model = _manager.GetComments(filter, page);
                    if (model != null)
                    {
                        if (cookie != null)
                        {
                            HttpContext.Request.Cookies.Remove("comments_filters");
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
                    return View(new VM_Comment());
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
        public ActionResult New(VM_Comment art)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        CommentsManager _manager = new CommentsManager();
                        if (_manager.CreateComment(ref art))
                        {
                            ViewBag.InfoMessage = String.Format("Комментарий успешно создан.");
                            VM_CommentsFilters f = _getCommentFiltersFromCookie(HttpContext.Request.Cookies["comments_filters"]);
                            if (f == null)
                                return RedirectToAction("List");
                            else
                                return RedirectToAction("List", f.GetFilterAsRouteValues(_getCommentListPageFromCookie(HttpContext.Request.Cookies["comments_filters"])));
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
                    CommentsManager _manager = new CommentsManager();
                    VM_Comment model = _manager.GetComment(id);
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
        public ActionResult Edit(VM_Comment org, int id)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        CommentsManager _manager = new CommentsManager();
                        if (_manager.UpdateComment(id, org))
                        {
                            ViewBag.InfoMessage = "Комментарицй успешно сохранен.";
                            VM_CommentsFilters f = _getCommentFiltersFromCookie(HttpContext.Request.Cookies["comments_filters"]);
                            if (f == null)
                                return RedirectToAction("List");
                            else
                                return RedirectToAction("List", f.GetFilterAsRouteValues(_getCommentListPageFromCookie(HttpContext.Request.Cookies["comments_filters"])));
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
                    CommentsManager _manager = new CommentsManager();
                    if (_manager.DeleteComment(id))
                    {
                        ViewBag.InfoMessage = String.Format("Комментарий успешно удален.");
                        VM_CommentsFilters f = _getCommentFiltersFromCookie(HttpContext.Request.Cookies["comments_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getCommentListPageFromCookie(HttpContext.Request.Cookies["comments_filters"])));
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
                    CommentsManager _manager = new CommentsManager();
                    if (_manager.SetCommentActive(id, true))
                    {
                        VM_CommentsFilters f = _getCommentFiltersFromCookie(HttpContext.Request.Cookies["comments_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getCommentListPageFromCookie(HttpContext.Request.Cookies["comments_filters"])));
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
                    CommentsManager _manager = new CommentsManager();
                    if (_manager.SetCommentActive(id, false))
                    {
                        VM_CommentsFilters f = _getCommentFiltersFromCookie(HttpContext.Request.Cookies["comments_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getCommentListPageFromCookie(HttpContext.Request.Cookies["comments_filters"])));
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
        public ActionResult Confirm(int id)
        {
            try
            {
                if (_connected)
                {
                    CommentsManager _manager = new CommentsManager();
                    if (_manager.SetComfirmed(id, true))
                    {
                        VM_CommentsFilters f = _getCommentFiltersFromCookie(HttpContext.Request.Cookies["comments_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getCommentListPageFromCookie(HttpContext.Request.Cookies["comments_filters"])));
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
        public ActionResult NotConfirm(int id)
        {
            try
            {
                if (_connected)
                {
                    CommentsManager _manager = new CommentsManager();
                    if (_manager.SetComfirmed(id, false))
                    {
                        VM_CommentsFilters f = _getCommentFiltersFromCookie(HttpContext.Request.Cookies["comments_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getCommentListPageFromCookie(HttpContext.Request.Cookies["comments_filters"])));
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
        public ActionResult _getCommentList(VM_Comments model)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_commentList", model);
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
        public ActionResult _getCommentListFilter(VM_CommentsFilters filter)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_commentListFilter", filter);
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
        private HttpCookie _setCommentFiltersCookie(VM_CommentsFilters filter, int page)
        {
            try
            {
                HttpCookie cookie = new HttpCookie("comments_filters");
                cookie["is_active"] = ((int)filter.IsActive).ToString();
                cookie["confirm_status"] = ((int)filter.Confirmed).ToString();                
                cookie["text"] = filter.CommentText.ToString(CultureInfo.CurrentCulture);
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
        private VM_CommentsFilters _getCommentFiltersFromCookie(HttpCookie cookie)
        {
            try
            {
                if (cookie != null)
                {
                    VM_CommentsFilters filter = new VM_CommentsFilters();
                    if (cookie["is_active"] != null)
                        filter.IsActive = (EnumBoolType)(Convert.ToInt32(cookie["is_active"]));
                    else
                        filter.IsActive = EnumBoolType.None;
                    if (cookie["confirm_status"] != null)
                        filter.Confirmed = (EnumBoolType)(Convert.ToInt32(cookie["confirm_status"]));
                    else
                        filter.Confirmed = EnumBoolType.None;
                    if (cookie["text"] != null)
                        filter.CommentText = cookie["text"].ToString(CultureInfo.CurrentCulture);
                    else
                        filter.CommentText = String.Empty;
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
        private int _getCommentListPageFromCookie(HttpCookie cookie)
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
