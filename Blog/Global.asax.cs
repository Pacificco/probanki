using Bankiru.Controllers;
using Bankiru.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Bankiru
{
    // Примечание: Инструкции по включению классического режима IIS6 или IIS7 
    // см. по ссылке http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
            GlobalParams.Initialize();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;

            Exception ex = ctx.Server.GetLastError();
            var httpException = ex as HttpException;
            ctx.Response.Clear();
            RequestContext rc = ((MvcHandler)ctx.CurrentHandler).RequestContext;
            rc.RouteData.Values.Clear();
            rc.RouteData.Values.Add("controller", "Error");
            rc.RouteData.Values.Add("url", ctx.Request.Url.OriginalString);
            IController controller = new ErrorController();

            var context = new ControllerContext(rc, (ControllerBase)controller);

            var viewResult = new ViewResult();
            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        viewResult.ViewName = "NotFound";
                        rc.RouteData.Values.Add("action", "Http404");
                        break;
                    case 500:
                        viewResult.ViewName = "InternalError";
                        rc.RouteData.Values.Add("action", "Http404");
                        break;
                    default:
                        viewResult.ViewName = "Error";
                        rc.RouteData.Values.Add("action", "Http404");
                        break;
                }
            }
            else
            {
                viewResult.ViewName = "Error";
                rc.RouteData.Values.Add("action", "Http404");
            }

            viewResult.ViewData.Model = null;
            //viewResult.ViewData.Model = new HandleErrorInfo(ex, context.RouteData.GetRequiredString("controller"), context.RouteData.GetRequiredString("action"));
            viewResult.ExecuteResult(context);

            ctx.Server.ClearError();
        }
    }
}