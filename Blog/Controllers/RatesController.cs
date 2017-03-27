using Bankiru.Models.Domain.Club;
using Bankiru.Models.OutApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    public class RatesController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                if (_connected)
                {
                    ForecastManager manager = new ForecastManager();
                    List<VM_ForecastSubject> model = manager.GetForecastSubjects();
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        log.Error(manager.LastError);
                        return View(_errPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return View(_errPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return View(_errPage);
            }
        }

        #region ДОЧЕРНИЕ МЕТОДЫ
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
        #endregion
    }
}
