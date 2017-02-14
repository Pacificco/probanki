﻿using Bankiru.Models.Domain;
using Bankiru.Models.Domain.Club;
using Bankiru.Models.Domain.Users;
using Bankiru.Models.OutApi;
using Bankiru.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    //[CustomAuthorize(Roles = "admin,club_member")]
    public class ForecastController : BaseController
    {
        [HttpGet]
        public ActionResult List()
        {
            try
            {
                if (_connected)
                {
                    ForecastManager manager = new ForecastManager();
                    List<VM_Forecast> model = manager.GetCurrentForecasts(true);                    
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        log.Error("[Forecasts] Не удалось загрузить текущие прогнозы!\n" + manager.LastError);
                        return View("Error");
                    }
                }
                else
                {                    
                    log.Error("[Forecasts] Не удалось загрузить текущие прогнозы!\n" + _errMassage);
                    return View("Error");
                }
            }
            catch (Exception e)
            {
                log.Error("[Forecasts] Не удалось загрузить текущие прогнозы!\n" + e.ToString());
                return View("Error");
            }
        }
        [HttpGet]        
        public ActionResult Forecast(string subject_id, int id)
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
                        log.Error("[Forecasts] Не удалось загрузить прогноз!\n" + manager.LastError);
                        return View("Error");
                    }
                }
                else
                {
                    log.Error("[Forecasts] Не удалось загрузить прогноз!\n" + _errMassage);
                    return View("Error");
                }
            }
            catch (Exception e)
            {
                log.Error("[Forecasts] Не удалось загрузить прогноз!\n" + e.ToString());
                return View("Error");
            }
        }
        [HttpGet]
        public ActionResult Archive()
        {
            return View();
        }

        #region ДОЧЕРНИЕ МЕТОДЫ
        [ChildActionOnly]
        [OutputCache(Duration = 60, VaryByParam = "cur_item")]
        public ActionResult _getModuleClubMenu(string cur_item = "none")
        {
            try
            {
                return PartialView("_moduleClubMenu", cur_item);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());                
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60)]
        public ActionResult _getModuleClubMembers()
        {
            try
            {
                if (_connected)
                {
                    ForecastManager manager = new ForecastManager();
                    List<VM_User> model = new List<VM_User>();
                    model = manager.GetUsers(5);
                    if (model != null)
                    {
                        return PartialView("_moduleClubMembers", model);
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
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60, VaryByParam = "cur_item")]
        public PartialViewResult _getModuleForecastAsHeader(int cur_item)
        {
            try
            {
                if (_connected)
                {
                    ForecastManager manager = new ForecastManager();
                    VM_ForecastHeader model = new VM_ForecastHeader();
                    model.Forecasts = manager.GetCurrentForecasts(false);                    
                    model.CurItemId = cur_item;
                    if (model.Forecasts != null)
                    {
                        return PartialView("_moduleForecastAsHeader", model);
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
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }            
        }
        [ChildActionOnly]        
        [OutputCache(Duration = 60, VaryByParam = "subjectId")]
        public PartialViewResult _getModuleChart(byte subjectId)
        {
            try
            {
                if (_connected)
                {
                    ChartManager manager = new ChartManager();
                    List<ChartObject> model = manager.LoadChartDataFromDb(subjectId);
                    if (model != null)
                    {
                        return PartialView("_moduleChart", model);
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
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        //[OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public PartialViewResult _getModuleAddUserToForecast(int forecastId)
        {
            try
            {
                if (_connected)
                {
                    if (String.IsNullOrEmpty(SessionPersister.Username))
                    {
                        VM_UserForecastState noneAuthModel = new VM_UserForecastState();
                        noneAuthModel.AddEnable = false;
                        noneAuthModel.DisableState = EnumDisableAddUserToForecast.NonAuthorization;
                        return PartialView("_moduleDisableAddUserToForecast", noneAuthModel);
                    }

                    ForecastManager manager = new ForecastManager();
                    VM_UserForecastState model = manager.GetUserForecastState(forecastId, SessionPersister.CurrentUser.Id);

                    if (model.AddEnable)
                    {
                        VM_AddUserToForecast user = new VM_AddUserToForecast();
                        user.ForecastId = forecastId;
                        user.UserId = SessionPersister.CurrentUser.Id;
                        user.Value = 0.0F;
                        return PartialView("_moduleAddUserToForecast", user);
                    }
                    else
                    {
                        return PartialView("_moduleDisableAddUserToForecast", model);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
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
