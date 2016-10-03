using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Areas.Admin.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error(string err_message)
        {
            return View("Error", null, err_message);
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}
