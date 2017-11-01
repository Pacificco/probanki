using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    public class ErrorController : BaseController
    {        
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        #region Http404
        public ActionResult Http404(string url)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            var model = new NotFoundViewModel();
            // Если путь относительный ('NotFound' route), тогда его нужно заменить на запрошенный путь
            model.RequestedUrl = Request.Url.OriginalString.Contains(url) & Request.Url.OriginalString != url ?
                Request.Url.OriginalString : url;
            // Предотвращаем зацикливание при равенстве Referrer и Request
            model.ReferrerUrl = Request.UrlReferrer != null &&
                Request.UrlReferrer.OriginalString != model.RequestedUrl ?
                Request.UrlReferrer.OriginalString : null;

            // TODO: добавить реализацию ILogger
            log.Error("NotFound - " + model.RequestedUrl);

            return View("NotFound", model);
        }
        public class NotFoundViewModel
        {
            public string RequestedUrl { get; set; }
            public string ReferrerUrl { get; set; }
        }
        #endregion  

        #region Http500
        public ActionResult Http500(string url)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var model = new InternalServerErrorViewModel();
            // Если путь относительный ('NotFound' route), тогда его нужно заменить на запрошенный путь
            model.RequestedUrl = Request.Url.OriginalString.Contains(url) & Request.Url.OriginalString != url ?
                Request.Url.OriginalString : url;
            // Предотвращаем зацикливание при равенстве Referrer и Request
            model.ReferrerUrl = Request.UrlReferrer != null &&
                Request.UrlReferrer.OriginalString != model.RequestedUrl ?
                Request.UrlReferrer.OriginalString : null;

            // TODO: добавить реализацию ILogger
            log.Error("InternalServerError - " + model.RequestedUrl);

            return View("InternalError", model);
        }
        public class InternalServerErrorViewModel
        {
            public string RequestedUrl { get; set; }
            public string ReferrerUrl { get; set; }
        }
        #endregion  
    }
}
