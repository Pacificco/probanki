using Bankiru.Controllers;
using Bankiru.Models.Domain;
using Bankiru.Models.Domain.Club;
using Bankiru.Models.Helpers;
using Bankiru.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class ForecastController : BaseController
    {
        [HttpGet]
        public ActionResult List(VM_ForecastsFilters filter, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    HttpCookie cookie = _setForecastFiltersCookie(filter, page);
                    ForecastManager manager = new ForecastManager();
                    VM_Forecasts model = manager.GetForecasts(filter, page);
                    if (model != null)
                    {
                        if (cookie != null)
                            HttpContext.Response.Cookies.Add(cookie);
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = manager.LastError;
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
                    return View(new VM_Forecast());
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
        public ActionResult New(VM_Forecast forecast)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        ForecastManager manager = new ForecastManager();
                        if (manager.CreateForecasts(forecast))
                        {
                            ViewBag.InfoMessage = "Прогноз успешно создан!";
                            VM_ForecastsFilters f = _getForecastFiltersFromCookie(HttpContext.Request.Cookies["forecast_filters"]);
                            if (f == null)
                                return RedirectToAction("List");
                            else
                                return RedirectToAction("List", f.GetFilterAsRouteValues(_getForecastListPageFromCookie(HttpContext.Request.Cookies["forecast_filters"])));
                        }
                        else
                        {
                            ViewBag.ErrorMessage = manager.LastError;
                            return RedirectToAction("Error", "Error", null);
                        }
                    }
                    else
                    {
                        return View(forecast);
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
                    ForecastManager manager = new ForecastManager();
                    VM_Forecast model = manager.GetForecast(id);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = manager.LastError;
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
        public ActionResult Edit(VM_Forecast forecast, int id)
        {
            try
            {
                if (_connected)
                {
                    if (ModelState.IsValid)
                    {
                        ForecastManager manager = new ForecastManager();
                        if (manager.UpdateForecast(id, forecast))
                        {
                            ViewBag.InfoMessage = "Прогноз успешно изменен!";
                            VM_ForecastsFilters f = _getForecastFiltersFromCookie(HttpContext.Request.Cookies["forecast_filters"]);
                            if (f == null)
                                return RedirectToAction("List");
                            else
                                return RedirectToAction("List", f.GetFilterAsRouteValues(_getForecastListPageFromCookie(HttpContext.Request.Cookies["forecast_filters"])));
                        }
                        else
                        {
                            ViewBag.ErrorMessage = manager.LastError;
                            return RedirectToAction("Error", "Error", null);
                        }
                    }
                    else
                    {
                        return View(forecast);
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
                    ForecastManager manager = new ForecastManager();
                    if (manager.DeleteForecast(id))
                    {
                        ViewBag.InfoMessage = String.Format("Прогноз успешно удален!");
                        VM_ForecastsFilters f = _getForecastFiltersFromCookie(HttpContext.Request.Cookies["forecast_filters"]);
                        if (f == null)
                            return RedirectToAction("List");
                        else
                            return RedirectToAction("List", f.GetFilterAsRouteValues(_getForecastListPageFromCookie(HttpContext.Request.Cookies["forecast_filters"])));
                    }
                    else
                    {
                        ViewBag.ErrorMessage = manager.LastError;
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
                    ForecastManager manager = new ForecastManager();
                    //if (manager.SetForecastActive(id, is_active))
                    //{
                    //    return Json(new { resultMessage = "OK" });
                    //}
                    //else
                    //{
                    //    return Json(new { resultMessage = "ERROR" });
                    //}
                    return Json(new { resultMessage = "ERROR" });
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
        public ActionResult AjaxClose(VM_ForecastCloseInfo model)
        {
            try
            {
                if (_connected)
                {
                    if (Request.IsAjaxRequest())
                    {
                        if (ModelState.IsValid)
                        {
                            ForecastManager manager = new ForecastManager();

                            if (String.IsNullOrEmpty(model.FactValue))
                            {
                                model.SuccessMessage = String.Empty;
                                ModelState.AddModelError("", "Вы не указали фактическое значение прогноза!");
                                return PartialView("_moduleAddUserToForecast", model);
                            }

                            bool success = false;
                            double value = TextHelper.DoubleParse(model.FactValue, out success);
                            if (!success)
                            {
                                model.SuccessMessage = String.Empty;
                                ModelState.AddModelError("", "Вы задали некорректное фактическое значение!");
                                return PartialView("_forecastCloseBlock", model);
                            }

                            if (!manager.CloseForecast(model.ForecastId, value, model.NextForecastDate))
                            {
                                model.SuccessMessage = String.Empty;
                                ModelState.AddModelError("", "Ошибка во время закрытия прогноза!");
                                return PartialView("_forecastCloseBlock", model);
                            }
                            return PartialView("_forecastCloseBlock", new VM_AddUserToForecast() { SuccessMessage = "Успешно!" });
                        }
                        else
                        {
                            return PartialView("_forecastCloseBlock", model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ошибка запроса к серверу!");
                        return PartialView("_forecastCloseBlock", model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка подключения к серверу!");
                    return PartialView("_forecastCloseBlock", model);
                }
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Ошибка в методе AjaxClose!\n", ex.ToString()));
                ModelState.AddModelError("", "Ошибка запроса к серверу!");
                return PartialView("_forecastCloseBlock", model);
            }
        }

        #region ДОЧЕРНИЕ МЕТОДЫ
        [ChildActionOnly]
        public ActionResult _getForecastList(VM_Forecasts model)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_forecastList", model);
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
        public ActionResult _getForecastListFilter(VM_ForecastsFilters filter)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_forecastListFilter", filter);
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
        public ActionResult _getForecastCloseBlock(int forecast_id, DateTime next_forecast_date)
        {
            try
            {
                if (_connected)
                {
                    VM_ForecastCloseInfo model = new VM_ForecastCloseInfo();
                    model.FactValue = "";
                    model.NextForecastDate = next_forecast_date;
                    model.ForecastId = forecast_id;
                    return PartialView("_forecastCloseBlock", model);
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
        private HttpCookie _setForecastFiltersCookie(VM_ForecastsFilters filter, int page)
        {
            try
            {
                HttpCookie cookie = new HttpCookie("forecast_filters");
                cookie["is_archive"] = ((int)filter.IsArchive).ToString();
                cookie["subject_id"] = filter.SubjectId.ToString();
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
        private VM_ForecastsFilters _getForecastFiltersFromCookie(HttpCookie cookie)
        {
            try
            {
                if (cookie != null)
                {
                    VM_ForecastsFilters filter = new VM_ForecastsFilters();
                    if (cookie["is_archive"] != null)
                        filter.IsArchive = (EnumBoolType)(Convert.ToInt32(cookie["is_archive"]));
                    else
                        filter.IsArchive = EnumBoolType.None;
                    if (cookie["subject_id"] != null)
                        filter.SubjectId = (Convert.ToByte(cookie["subject_id"]));
                    else
                        filter.SubjectId = 0;
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
        private int _getForecastListPageFromCookie(HttpCookie cookie)
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
