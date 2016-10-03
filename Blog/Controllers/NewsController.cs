using Bankiru.Models.Domain.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    public class NewsController : BaseController
    {
        [HttpGet]
        public ActionResult List(int page = 1)
        {
            try
            {
                if (_connected)
                {
                    NewsManager _manager = new NewsManager();
                    VM_NewsList model = _manager.GetNewsList(new VM_NewsFilters(), page, 30);
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
        public ActionResult Show(string news_id)
        {
            try
            {
                if (_connected)
                {
                    NewsManager _manager = new NewsManager();
                    VM_News model = _manager.GetNews(news_id);
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
    }
}
